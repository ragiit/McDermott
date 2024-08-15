using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.BuildingCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class BuildingCommand
    {
        public sealed record GetBuildingQuery(Expression<Func<Building, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<BuildingDto>>;
        public sealed record CreateBuildingRequest(BuildingDto BuildingDto, bool ReturnNewData = false) : IRequest<BuildingDto>;
        public sealed record CreateListBuildingRequest(List<BuildingDto> BuildingDtos, bool ReturnNewData = false) : IRequest<List<BuildingDto>>;
        public sealed record UpdateBuildingRequest(BuildingDto BuildingDto, bool ReturnNewData = false) : IRequest<BuildingDto>;
        public sealed record UpdateListBuildingRequest(List<BuildingDto> BuildingDtos, bool ReturnNewData = false) : IRequest<List<BuildingDto>>;
        public sealed record DeleteBuildingRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class BuildingQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataBuilding) :
        IRequestHandler<GetBuildingQuery, List<BuildingDto>>,
        IRequestHandler<CreateBuildingRequest, BuildingDto>,
        IRequestHandler<CreateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<UpdateBuildingRequest, BuildingDto>,
        IRequestHandler<UpdateListBuildingRequest, List<BuildingDto>>,
        IRequestHandler<DeleteBuildingRequest, bool>
    {
        private string CacheKey = "GetBuildingQuery_";

        private async Task<(BuildingDto, List<BuildingDto>)> Result(Building? result = null, List<Building>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<BuildingDto>(), []);
                else
                    return ((await unitOfWork.Repository<Building>().Entities
                        .AsNoTracking()
                        .Include(x=>x.HealthCenter)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<BuildingDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<BuildingDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Building>().Entities
                        .AsNoTracking()
                        .Include(x => x.HealthCenter)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<BuildingDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<BuildingDto>> Handle(GetBuildingQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Building>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Building>().Entities
                        .AsNoTracking()
                        .Include(x => x.HealthCenter)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<BuildingDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<BuildingDto> Handle(CreateBuildingRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingDto.Adapt<CreateUpdateBuildingDto>();
            var result = await unitOfWork.Repository<Building>().AddAsync(req.Adapt<Building>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataBuilding.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<BuildingDto>> Handle(CreateListBuildingRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingDtos.Adapt<List<CreateUpdateBuildingDto>>();
            var result = await unitOfWork.Repository<Building>().AddAsync(req.Adapt<List<Building>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataBuilding.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BuildingDto> Handle(UpdateBuildingRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingDto.Adapt<CreateUpdateBuildingDto>();
            var result = await unitOfWork.Repository<Building>().UpdateAsync(req.Adapt<Building>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataBuilding.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<BuildingDto>> Handle(UpdateListBuildingRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingDtos.Adapt<CreateUpdateBuildingDto>();
            var result = await unitOfWork.Repository<Building>().UpdateAsync(req.Adapt<List<Building>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataBuilding.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBuildingRequest request, CancellationToken cancellationToken)
        {
            List<Building> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Building = await unitOfWork.Repository<Building>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Building != null)
                {
                    deletedCountries.Add(Building);
                    await unitOfWork.Repository<Building>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Building>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Building>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataBuilding.Clients.All.ReceiveNotification(new ReceiveDataDto
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
