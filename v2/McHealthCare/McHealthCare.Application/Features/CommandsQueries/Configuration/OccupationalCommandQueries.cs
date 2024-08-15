using Microsoft.Extensions.DependencyInjection;

namespace McHealthCare.Application.Features.CommandsQueries.Configuration
{
    public sealed class OccupationalCommand
    {
        public sealed record GetOccupationalQuery(Expression<Func<Occupational, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<OccupationalDto>>;
        public sealed record CreateOccupationalRequest(OccupationalDto OccupationalDto, bool ReturnNewData = false) : IRequest<OccupationalDto>;
        public sealed record CreateListOccupationalRequest(List<OccupationalDto> OccupationalDtos, bool ReturnNewData = false) : IRequest<List<OccupationalDto>>;
        public sealed record UpdateOccupationalRequest(OccupationalDto OccupationalDto, bool ReturnNewData = false) : IRequest<OccupationalDto>;
        public sealed record UpdateListOccupationalRequest(List<OccupationalDto> OccupationalDtos, bool ReturnNewData = false) : IRequest<List<OccupationalDto>>;
        public sealed record DeleteOccupationalRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class OccupationalQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataService, IServiceScopeFactory _scopeFactory) :
        IRequestHandler<GetOccupationalQuery, List<OccupationalDto>>,
        IRequestHandler<CreateOccupationalRequest, OccupationalDto>,
        IRequestHandler<CreateListOccupationalRequest, List<OccupationalDto>>,
        IRequestHandler<UpdateOccupationalRequest, OccupationalDto>,
        IRequestHandler<UpdateListOccupationalRequest, List<OccupationalDto>>,
        IRequestHandler<DeleteOccupationalRequest, bool>
    {
        private string CacheKey = "GetOccupationalQuery_";

        private async Task<(OccupationalDto, List<OccupationalDto>)> Result(Occupational? result = null, List<Occupational>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<OccupationalDto>(), []);
                else
                    return ((await unitOfWork.Repository<Occupational>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<OccupationalDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<OccupationalDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Occupational>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<OccupationalDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<OccupationalDto>> Handle(GetOccupationalQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Occupational> result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                result = await unitOfWork.Repository<Occupational>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<OccupationalDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<OccupationalDto> Handle(CreateOccupationalRequest request, CancellationToken cancellationToken)
        {
            var req = request.OccupationalDto.Adapt<CreateUpdateOccupationalDto>();
            var result = await unitOfWork.Repository<Occupational>().AddAsync(req.Adapt<Occupational>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<OccupationalDto>> Handle(CreateListOccupationalRequest request, CancellationToken cancellationToken)
        {
            var req = request.OccupationalDtos.Adapt<List<CreateUpdateOccupationalDto>>();
            var result = await unitOfWork.Repository<Occupational>().AddAsync(req.Adapt<List<Occupational>>());
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

        public async Task<OccupationalDto> Handle(UpdateOccupationalRequest request, CancellationToken cancellationToken)
        {
            var req = request.OccupationalDto.Adapt<CreateUpdateOccupationalDto>();
            var result = await unitOfWork.Repository<Occupational>().UpdateAsync(req.Adapt<Occupational>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataService.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<OccupationalDto>> Handle(UpdateListOccupationalRequest request, CancellationToken cancellationToken)
        {
            var req = request.OccupationalDtos.Adapt<CreateUpdateOccupationalDto>();
            var result = await unitOfWork.Repository<Occupational>().UpdateAsync(req.Adapt<List<Occupational>>());
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

        public async Task<bool> Handle(DeleteOccupationalRequest request, CancellationToken cancellationToken)
        {
            List<Occupational> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Occupational = await unitOfWork.Repository<Occupational>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Occupational != null)
                {
                    deletedCountries.Add(Occupational);
                    await unitOfWork.Repository<Occupational>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Occupational>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Occupational>().DeleteAsync(x => request.Ids.Contains(x.Id));
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