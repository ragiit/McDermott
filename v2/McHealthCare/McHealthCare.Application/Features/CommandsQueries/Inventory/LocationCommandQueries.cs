using static McHealthCare.Application.Features.CommandsQueries.Inventory.LocationCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Inventory
{
    public sealed class LocationCommand
    {
        public sealed record GetLocationQuery(Expression<Func<Location, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<LocationDto>>;
        public sealed record CreateLocationRequest(LocationDto LocationDto, bool ReturnNewData = false) : IRequest<LocationDto>;
        public sealed record CreateListLocationRequest(List<LocationDto> LocationDtos, bool ReturnNewData = false) : IRequest<List<LocationDto>>;
        public sealed record UpdateLocationRequest(LocationDto LocationDto, bool ReturnNewData = false) : IRequest<LocationDto>;
        public sealed record UpdateListLocationRequest(List<LocationDto> LocationDtos, bool ReturnNewData = false) : IRequest<List<LocationDto>>;
        public sealed record DeleteLocationRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class LocationQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataLocation) :
        IRequestHandler<GetLocationQuery, List<LocationDto>>,
        IRequestHandler<CreateLocationRequest, LocationDto>,
        IRequestHandler<CreateListLocationRequest, List<LocationDto>>,
        IRequestHandler<UpdateLocationRequest, LocationDto>,
        IRequestHandler<UpdateListLocationRequest, List<LocationDto>>,
        IRequestHandler<DeleteLocationRequest, bool>
    {
        private string CacheKey = "GetLocationQuery_";

        private async Task<(LocationDto, List<LocationDto>)> Result(Location? result = null, List<Location>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<LocationDto>(), []);
                else
                    return ((await unitOfWork.Repository<Location>().Entities
                        .AsNoTracking()
                        .Include(x => x.ParentLocation)
                        .Include(x => x.Company)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<LocationDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<LocationDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Location>().Entities
                        .Include(x => x.ParentLocation)
                        .Include(x => x.Company)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<LocationDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<LocationDto>> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Location>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Location>().Entities
                        .AsNoTracking()
                        .Include(x => x.ParentLocation)
                        .Include(x => x.Company)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<LocationDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<LocationDto> Handle(CreateLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.LocationDto.Adapt<CreateUpdateLocationDto>();
            var result = await unitOfWork.Repository<Location>().AddAsync(req.Adapt<Location>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LocationDto>> Handle(CreateListLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.LocationDtos.Adapt<List<CreateUpdateLocationDto>>();
            var result = await unitOfWork.Repository<Location>().AddAsync(req.Adapt<List<Location>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<LocationDto> Handle(UpdateLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.LocationDto.Adapt<CreateUpdateLocationDto>();
            var result = await unitOfWork.Repository<Location>().UpdateAsync(req.Adapt<Location>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<LocationDto>> Handle(UpdateListLocationRequest request, CancellationToken cancellationToken)
        {
            var req = request.LocationDtos.Adapt<CreateUpdateLocationDto>();
            var result = await unitOfWork.Repository<Location>().UpdateAsync(req.Adapt<List<Location>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteLocationRequest request, CancellationToken cancellationToken)
        {
            List<Location> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Location = await unitOfWork.Repository<Location>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Location != null)
                {
                    deletedCountries.Add(Location);
                    await unitOfWork.Repository<Location>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Location>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Location>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataLocation.Clients.All.ReceiveNotification(new ReceiveDataDto
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