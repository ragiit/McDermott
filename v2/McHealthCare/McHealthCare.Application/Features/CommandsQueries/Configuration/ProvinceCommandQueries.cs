﻿using Microsoft.Extensions.DependencyInjection;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.ProvinceCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class ProvinceCommand
    {
        public sealed record GetProvinceQuery(Expression<Func<Province, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<ProvinceDto>>;
        public sealed record CreateProvinceRequest(ProvinceDto ProvinceDto, bool ReturnNewData = false) : IRequest<ProvinceDto>;
        public sealed record CreateListProvinceRequest(List<ProvinceDto> ProvinceDtos, bool ReturnNewData = false) : IRequest<List<ProvinceDto>>;
        public sealed record UpdateProvinceRequest(ProvinceDto ProvinceDto, bool ReturnNewData = false) : IRequest<ProvinceDto>;
        public sealed record UpdateListProvinceRequest(List<ProvinceDto> ProvinceDtos, bool ReturnNewData = false) : IRequest<List<ProvinceDto>>;
        public sealed record DeleteProvinceRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class ProvinceQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetProvinceQuery, List<ProvinceDto>>,
        IRequestHandler<CreateProvinceRequest, ProvinceDto>,
        IRequestHandler<CreateListProvinceRequest, List<ProvinceDto>>,
        IRequestHandler<UpdateProvinceRequest, ProvinceDto>,
        IRequestHandler<UpdateListProvinceRequest, List<ProvinceDto>>,
        IRequestHandler<DeleteProvinceRequest, bool>
    {
        private string CacheKey = "GetProvinceQuery_";

        private async Task<(ProvinceDto, List<ProvinceDto>)> Result(Province? result = null, List<Province>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<ProvinceDto>(), []);
                else
                    return ((await unitOfWork.Repository<Province>().Entities
                        .AsNoTracking()
                        .Include(x => x.Country)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<ProvinceDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<ProvinceDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Province>().Entities
                        .AsNoTracking()
                        .Include(x => x.Country)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<ProvinceDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<ProvinceDto>> Handle(GetProvinceQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Province> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                result = await unitOfWork.Repository<Province>().Entities
                        .AsNoTracking()
                        .Include(x => x.Country)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<ProvinceDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<ProvinceDto> Handle(CreateProvinceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProvinceDto.Adapt<CreateUpdateProvinceDto>();
            var result = await unitOfWork.Repository<Province>().AddAsync(req.Adapt<Province>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ProvinceDto>> Handle(CreateListProvinceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProvinceDtos.Adapt<List<CreateUpdateProvinceDto>>();
            var result = await unitOfWork.Repository<Province>().AddAsync(req.Adapt<List<Province>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProvinceDto> Handle(UpdateProvinceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProvinceDto.Adapt<CreateUpdateProvinceDto>();
            var result = await unitOfWork.Repository<Province>().UpdateAsync(req.Adapt<Province>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ProvinceDto>> Handle(UpdateListProvinceRequest request, CancellationToken cancellationToken)
        {
            var req = request.ProvinceDtos.Adapt<CreateUpdateProvinceDto>();
            var result = await unitOfWork.Repository<Province>().UpdateAsync(req.Adapt<List<Province>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProvinceRequest request, CancellationToken cancellationToken)
        {
            List<Province> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Province = await unitOfWork.Repository<Province>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Province != null)
                {
                    deletedCountries.Add(Province);
                    await unitOfWork.Repository<Province>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Province>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Province>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
                {
                    Type = EnumTypeReceiveData.Delete,
                    Data = deletedCountries,
                });
            }

            return true;
        }

        #endregion DELETE
    }
}