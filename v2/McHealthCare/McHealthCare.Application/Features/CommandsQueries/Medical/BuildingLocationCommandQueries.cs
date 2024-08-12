using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.BuildingLocationCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class BuildingLocationCommand
    {
        public sealed record GetBuildingLocationQuery(Expression<Func<BuildingLocation, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<BuildingLocationDto>>;
        public sealed record CreateBuildingLocationRequest(BuildingLocationDto BuildingLocationDto, bool ReturnNewData = false) : IRequest<BuildingLocationDto>;
        public sealed record CreateListBuildingLocationRequest(List<BuildingLocationDto> BuildingLocationDtos, bool ReturnNewData = false) : IRequest<List<BuildingLocationDto>>;
        public sealed record UpdateBuildingLocationRequest(BuildingLocationDto BuildingLocationDto, bool ReturnNewData = false) : IRequest<BuildingLocationDto>;
        public sealed record UpdateListBuildingLocationRequest(List<BuildingLocationDto> BuildingLocationDtos, bool ReturnNewData = false) : IRequest<List<BuildingLocationDto>>;
        public sealed record DeleteBuildingLocationRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class BuildingLocationQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataBuildingLocation) :
        IRequestHandler<GetBuildingLocationQuery, List<BuildingLocationDto>>,
        IRequestHandler<CreateBuildingLocationRequest, BuildingLocationDto>,
        IRequestHandler<CreateListBuildingLocationRequest, List<BuildingLocationDto>>,
        IRequestHandler<UpdateBuildingLocationRequest, BuildingLocationDto>,
        IRequestHandler<UpdateListBuildingLocationRequest, List<BuildingLocationDto>>,
        IRequestHandler<DeleteBuildingLocationRequest, bool>
    {
        private string CacheKey = "GetBuildingLocationQuery_";

        private async Task<(BuildingLocationDto, List<BuildingLocationDto>)> Result(BuildingLocation? result = null, List<BuildingLocation>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<BuildingLocationDto>(), []);
                else
                    return ((await unitOfWork.Repository<BuildingLocation>().Entities
                        .AsNoTracking()
                        .Include(x=>x.Building)
                        .Include(x=>x.Location)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<BuildingLocationDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<BuildingLocationDto>>());
                else
                    return (new(), (await unitOfWork.Repository<BuildingLocation>().Entities
                        .AsNoTracking()
                        .Include(x => x.Building)
                        .Include(x => x.Location)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<BuildingLocationDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<BuildingLocationDto>> Handle(GetBuildingLocationQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<BuildingLocation>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<BuildingLocation>().Entities
                        .AsNoTracking()
                        .Include(x => x.Building)
                        .Include(x => x.Location)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<BuildingLocationDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<BuildingLocationDto> Handle(CreateBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingLocationDto.Adapt<CreateUpdateBuildingLocationDto>();
            var result = await unitOfWork.Repository<BuildingLocation>().AddAsync(req.Adapt<BuildingLocation>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataBuildingLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<BuildingLocationDto>> Handle(CreateListBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingLocationDtos.Adapt<List<CreateUpdateBuildingLocationDto>>();
            var result = await unitOfWork.Repository<BuildingLocation>().AddAsync(req.Adapt<List<BuildingLocation>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataBuildingLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<BuildingLocationDto> Handle(UpdateBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingLocationDto.Adapt<CreateUpdateBuildingLocationDto>();
            var result = await unitOfWork.Repository<BuildingLocation>().UpdateAsync(req.Adapt<BuildingLocation>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataBuildingLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<BuildingLocationDto>> Handle(UpdateListBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.BuildingLocationDtos.Adapt<CreateUpdateBuildingLocationDto>();
            var result = await unitOfWork.Repository<BuildingLocation>().UpdateAsync(req.Adapt<List<BuildingLocation>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataBuildingLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteBuildingLocationRequest request, CancellationToken cancellationToken)
        {
            List<BuildingLocation> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var BuildingLocation = await unitOfWork.Repository<BuildingLocation>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (BuildingLocation != null)
                {
                    deletedCountries.Add(BuildingLocation);
                    await unitOfWork.Repository<BuildingLocation>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<BuildingLocation>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<BuildingLocation>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataBuildingLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
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
