using Microsoft.Extensions.DependencyInjection;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.CityCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class CityCommand
    {
        public sealed record GetCityQuery(Expression<Func<City, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<CityDto>>;
        public sealed record CreateCityRequest(CityDto CityDto, bool ReturnNewData = false) : IRequest<CityDto>;
        public sealed record CreateListCityRequest(List<CityDto> CityDtos, bool ReturnNewData = false) : IRequest<List<CityDto>>;
        public sealed record UpdateCityRequest(CityDto CityDto, bool ReturnNewData = false) : IRequest<CityDto>;
        public sealed record UpdateListCityRequest(List<CityDto> CityDtos, bool ReturnNewData = false) : IRequest<List<CityDto>>;
        public sealed record DeleteCityRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class CityQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetCityQuery, List<CityDto>>,
        IRequestHandler<CreateCityRequest, CityDto>,
        IRequestHandler<CreateListCityRequest, List<CityDto>>,
        IRequestHandler<UpdateCityRequest, CityDto>,
        IRequestHandler<UpdateListCityRequest, List<CityDto>>,
        IRequestHandler<DeleteCityRequest, bool>
    {
        private string CacheKey = "GetCityQuery_";

        private async Task<(CityDto, List<CityDto>)> Result(City? result = null, List<City>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<CityDto>(), []);
                else
                    return ((await unitOfWork.Repository<City>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<CityDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<CityDto>>());
                else
                    return (new(), (await unitOfWork.Repository<City>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<CityDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<CityDto>> Handle(GetCityQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<City> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<City>().Entities
                        .AsNoTracking()
                        .Include(x => x.Province)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<CityDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<CityDto> Handle(CreateCityRequest request, CancellationToken cancellationToken)
        {
            var req = request.CityDto.Adapt<CreateUpdateCityDto>();
            var result = await unitOfWork.Repository<City>().AddAsync(req.Adapt<City>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CityDto>> Handle(CreateListCityRequest request, CancellationToken cancellationToken)
        {
            var req = request.CityDtos.Adapt<List<CreateUpdateCityDto>>();
            var result = await unitOfWork.Repository<City>().AddAsync(req.Adapt<List<City>>());
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

        public async Task<CityDto> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
        {
            var req = request.CityDto.Adapt<CreateUpdateCityDto>();
            var result = await unitOfWork.Repository<City>().UpdateAsync(req.Adapt<City>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CityDto>> Handle(UpdateListCityRequest request, CancellationToken cancellationToken)
        {
            var req = request.CityDtos.Adapt<CreateUpdateCityDto>();
            var result = await unitOfWork.Repository<City>().UpdateAsync(req.Adapt<List<City>>());
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

        public async Task<bool> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        {
            List<City> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var City = await unitOfWork.Repository<City>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (City != null)
                {
                    deletedCountries.Add(City);
                    await unitOfWork.Repository<City>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<City>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<City>().DeleteAsync(x => request.Ids.Contains(x.Id));
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