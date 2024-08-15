using McHealthCare.Domain.Entities.Medical;
using static McHealthCare.Application.Features.CommandsQueries.Medical.InsuranceCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class InsuranceCommand
    {
        public sealed record GetInsuranceQuery(Expression<Func<Insurance, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<InsuranceDto>>;
        public sealed record CreateInsuranceRequest(InsuranceDto InsuranceDto, bool ReturnNewData = false) : IRequest<InsuranceDto>;
        public sealed record CreateListInsuranceRequest(List<InsuranceDto> InsuranceDtos, bool ReturnNewData = false) : IRequest<List<InsuranceDto>>;
        public sealed record UpdateInsuranceRequest(InsuranceDto InsuranceDto, bool ReturnNewData = false) : IRequest<InsuranceDto>;
        public sealed record UpdateListInsuranceRequest(List<InsuranceDto> InsuranceDtos, bool ReturnNewData = false) : IRequest<List<InsuranceDto>>;
        public sealed record DeleteInsuranceRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class InsuranceQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataInsurance) :
        IRequestHandler<GetInsuranceQuery, List<InsuranceDto>>,
        IRequestHandler<CreateInsuranceRequest, InsuranceDto>,
        IRequestHandler<CreateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<UpdateInsuranceRequest, InsuranceDto>,
        IRequestHandler<UpdateListInsuranceRequest, List<InsuranceDto>>,
        IRequestHandler<DeleteInsuranceRequest, bool>
    {
        private string CacheKey = "GetInsuranceQuery_";

        private async Task<(InsuranceDto, List<InsuranceDto>)> Result(Insurance? result = null, List<Insurance>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<InsuranceDto>(), []);
                else
                    return ((await unitOfWork.Repository<Insurance>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<InsuranceDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<InsuranceDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Insurance>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<InsuranceDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<InsuranceDto>> Handle(GetInsuranceQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Insurance>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Insurance>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<InsuranceDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<InsuranceDto> Handle(CreateInsuranceRequest request, CancellationToken cancellationToken)
        {
            var req = request.InsuranceDto.Adapt<CreateUpdateInsuranceDto>();
            var result = await unitOfWork.Repository<Insurance>().AddAsync(req.Adapt<Insurance>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataInsurance.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<InsuranceDto>> Handle(CreateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            var req = request.InsuranceDtos.Adapt<List<CreateUpdateInsuranceDto>>();
            var result = await unitOfWork.Repository<Insurance>().AddAsync(req.Adapt<List<Insurance>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataInsurance.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InsuranceDto> Handle(UpdateInsuranceRequest request, CancellationToken cancellationToken)
        {
            var req = request.InsuranceDto.Adapt<CreateUpdateInsuranceDto>();
            var result = await unitOfWork.Repository<Insurance>().UpdateAsync(req.Adapt<Insurance>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataInsurance.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<InsuranceDto>> Handle(UpdateListInsuranceRequest request, CancellationToken cancellationToken)
        {
            var req = request.InsuranceDtos.Adapt<CreateUpdateInsuranceDto>();
            var result = await unitOfWork.Repository<Insurance>().UpdateAsync(req.Adapt<List<Insurance>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataInsurance.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInsuranceRequest request, CancellationToken cancellationToken)
        {
            List<Insurance> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Insurance = await unitOfWork.Repository<Insurance>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Insurance != null)
                {
                    deletedCountries.Add(Insurance);
                    await unitOfWork.Repository<Insurance>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Insurance>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Insurance>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataInsurance.Clients.All.ReceiveNotification(new ReceiveDataDto
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