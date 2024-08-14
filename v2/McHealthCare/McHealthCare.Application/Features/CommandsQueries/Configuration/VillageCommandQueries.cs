using Mapster;
using McHealthCare.Application.Dtos.Configuration;
using McHealthCare.Application.Extentions;
using McHealthCare.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class VillageCommand
    {
        public sealed record GetVillageQuery(Expression<Func<Village, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<VillageDto>>;
        public sealed record CreateVillageRequest(VillageDto VillageDto, bool ReturnNewData = false) : IRequest<VillageDto>;
        public sealed record CreateListVillageRequest(List<VillageDto> VillageDtos, bool ReturnNewData = false) : IRequest<List<VillageDto>>;
        public sealed record UpdateVillageRequest(VillageDto VillageDto, bool ReturnNewData = false) : IRequest<VillageDto>;
        public sealed record UpdateListVillageRequest(List<VillageDto> VillageDtos, bool ReturnNewData = false) : IRequest<List<VillageDto>>;
        public sealed record DeleteVillageRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class VillageQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetVillageQuery, List<VillageDto>>,
        IRequestHandler<CreateVillageRequest, VillageDto>,
        IRequestHandler<CreateListVillageRequest, List<VillageDto>>,
        IRequestHandler<UpdateVillageRequest, VillageDto>,
        IRequestHandler<UpdateListVillageRequest, List<VillageDto>>,
        IRequestHandler<DeleteVillageRequest, bool>
    {
        private string CacheKey = "GetVillageQuery_";

        private async Task<(VillageDto, List<VillageDto>)> Result(Village? result = null, List<Village>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<VillageDto>(), []);
                else
                    return ((await unitOfWork.Repository<Village>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .Include(x => x.District)
                        .Include(x => x.City)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<VillageDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<VillageDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Village>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .Include(x => x.District)
                        .Include(x => x.City)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<VillageDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<VillageDto>> Handle(GetVillageQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Village> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                result = await unitOfWork.Repository<Village>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .Include(x => x.City)
                        .Include(x => x.District)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<VillageDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<VillageDto> Handle(CreateVillageRequest request, CancellationToken cancellationToken)
        {
            var req = request.VillageDto.Adapt<CreateUpdateVillageDto>();
            var result = await unitOfWork.Repository<Village>().AddAsync(req.Adapt<Village>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<VillageDto>> Handle(CreateListVillageRequest request, CancellationToken cancellationToken)
        {
            var req = request.VillageDtos.Adapt<List<CreateUpdateVillageDto>>();
            var result = await unitOfWork.Repository<Village>().AddAsync(req.Adapt<List<Village>>());
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

        public async Task<VillageDto> Handle(UpdateVillageRequest request, CancellationToken cancellationToken)
        {
            var req = request.VillageDto.Adapt<CreateUpdateVillageDto>();
            var result = await unitOfWork.Repository<Village>().UpdateAsync(req.Adapt<Village>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<VillageDto>> Handle(UpdateListVillageRequest request, CancellationToken cancellationToken)
        {
            var req = request.VillageDtos.Adapt<CreateUpdateVillageDto>();
            var result = await unitOfWork.Repository<Village>().UpdateAsync(req.Adapt<List<Village>>());
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

        public async Task<bool> Handle(DeleteVillageRequest request, CancellationToken cancellationToken)
        {
            List<Village> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Village = await unitOfWork.Repository<Village>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Village != null)
                {
                    deletedCountries.Add(Village);
                    await unitOfWork.Repository<Village>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Village>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Village>().DeleteAsync(x => request.Ids.Contains(x.Id));
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