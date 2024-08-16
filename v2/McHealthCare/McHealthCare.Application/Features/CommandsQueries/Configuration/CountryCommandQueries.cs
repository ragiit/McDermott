using Microsoft.Extensions.DependencyInjection;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class CountryCommand
    {
        public sealed record GetCountryQuery(Expression<Func<Country, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<CountryDto>>;
        public sealed record CreateCountryRequest(CountryDto CountryDto, bool ReturnNewData = false) : IRequest<CountryDto>;
        public sealed record CreateListCountryRequest(List<CountryDto> CountryDtos, bool ReturnNewData = false) : IRequest<List<CountryDto>>;
        public sealed record UpdateCountryRequest(CountryDto CountryDto, bool ReturnNewData = false) : IRequest<CountryDto>;
        public sealed record UpdateListCountryRequest(List<CountryDto> CountryDtos, bool ReturnNewData = false) : IRequest<List<CountryDto>>;
        public sealed record DeleteCountryRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class CountryQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetCountryQuery, List<CountryDto>>,
        IRequestHandler<CreateCountryRequest, CountryDto>,
        IRequestHandler<CreateListCountryRequest, List<CountryDto>>,
        IRequestHandler<UpdateCountryRequest, CountryDto>,
        IRequestHandler<UpdateListCountryRequest, List<CountryDto>>,
        IRequestHandler<DeleteCountryRequest, bool>
    {
        private string CacheKey = "GetCountryQuery_";

        private async Task<(CountryDto, List<CountryDto>)> Result(Country? result = null, List<Country>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<CountryDto>(), []);
                else
                    return ((await unitOfWork.Repository<Country>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<CountryDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<CountryDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Country>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<CountryDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<CountryDto>> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Country> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<Country>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<CountryDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<CountryDto> Handle(CreateCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDto.Adapt<CreateUpdateCountryDto>();
            var result = await unitOfWork.Repository<Country>().AddAsync(req.Adapt<Country>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CountryDto>> Handle(CreateListCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDtos.Adapt<List<CreateUpdateCountryDto>>();
            var result = await unitOfWork.Repository<Country>().AddAsync(req.Adapt<List<Country>>());
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

        public async Task<CountryDto> Handle(UpdateCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDto.Adapt<CreateUpdateCountryDto>();
            var result = await unitOfWork.Repository<Country>().UpdateAsync(req.Adapt<Country>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<CountryDto>> Handle(UpdateListCountryRequest request, CancellationToken cancellationToken)
        {
            var req = request.CountryDtos.Adapt<CreateUpdateCountryDto>();
            var result = await unitOfWork.Repository<Country>().UpdateAsync(req.Adapt<List<Country>>());
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

        public async Task<bool> Handle(DeleteCountryRequest request, CancellationToken cancellationToken)
        {
            List<Country> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var country = await unitOfWork.Repository<Country>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (country != null)
                {
                    deletedCountries.Add(country);
                    await unitOfWork.Repository<Country>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Country>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Country>().DeleteAsync(x => request.Ids.Contains(x.Id));
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