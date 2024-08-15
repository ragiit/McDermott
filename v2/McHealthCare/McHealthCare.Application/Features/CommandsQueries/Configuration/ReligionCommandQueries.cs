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
    public sealed class ReligionCommand
    {
        public sealed record GetReligionQuery(Expression<Func<Religion, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<ReligionDto>>;
        public sealed record CreateReligionRequest(ReligionDto ReligionDto, bool ReturnNewData = false) : IRequest<ReligionDto>;
        public sealed record CreateListReligionRequest(List<ReligionDto> ReligionDtos, bool ReturnNewData = false) : IRequest<List<ReligionDto>>;
        public sealed record UpdateReligionRequest(ReligionDto ReligionDto, bool ReturnNewData = false) : IRequest<ReligionDto>;
        public sealed record UpdateListReligionRequest(List<ReligionDto> ReligionDtos, bool ReturnNewData = false) : IRequest<List<ReligionDto>>;
        public sealed record DeleteReligionRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class ReligionQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetReligionQuery, List<ReligionDto>>,
        IRequestHandler<CreateReligionRequest, ReligionDto>,
        IRequestHandler<CreateListReligionRequest, List<ReligionDto>>,
        IRequestHandler<UpdateReligionRequest, ReligionDto>,
        IRequestHandler<UpdateListReligionRequest, List<ReligionDto>>,
        IRequestHandler<DeleteReligionRequest, bool>
    {
        private string CacheKey = "GetReligionQuery_";

        private async Task<(ReligionDto, List<ReligionDto>)> Result(Religion? result = null, List<Religion>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<ReligionDto>(), []);
                else
                    return ((await unitOfWork.Repository<Religion>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<ReligionDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<ReligionDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Religion>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<ReligionDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<ReligionDto>> Handle(GetReligionQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Religion> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<Religion>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<ReligionDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<ReligionDto> Handle(CreateReligionRequest request, CancellationToken cancellationToken)
        {
            var req = request.ReligionDto.Adapt<CreateUpdateReligionDto>();
            var result = await unitOfWork.Repository<Religion>().AddAsync(req.Adapt<Religion>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ReligionDto>> Handle(CreateListReligionRequest request, CancellationToken cancellationToken)
        {
            var req = request.ReligionDtos.Adapt<List<CreateUpdateReligionDto>>();
            var result = await unitOfWork.Repository<Religion>().AddAsync(req.Adapt<List<Religion>>());
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

        public async Task<ReligionDto> Handle(UpdateReligionRequest request, CancellationToken cancellationToken)
        {
            var req = request.ReligionDto.Adapt<CreateUpdateReligionDto>();
            var result = await unitOfWork.Repository<Religion>().UpdateAsync(req.Adapt<Religion>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<ReligionDto>> Handle(UpdateListReligionRequest request, CancellationToken cancellationToken)
        {
            var req = request.ReligionDtos.Adapt<CreateUpdateReligionDto>();
            var result = await unitOfWork.Repository<Religion>().UpdateAsync(req.Adapt<List<Religion>>());
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

        public async Task<bool> Handle(DeleteReligionRequest request, CancellationToken cancellationToken)
        {
            List<Religion> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Religion = await unitOfWork.Repository<Religion>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Religion != null)
                {
                    deletedCountries.Add(Religion);
                    await unitOfWork.Repository<Religion>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Religion>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Religion>().DeleteAsync(x => request.Ids.Contains(x.Id));
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