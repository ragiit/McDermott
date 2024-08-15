using McHealthCare.Domain.Entities.Medical;
using static McHealthCare.Application.Features.CommandsQueries.Medical.SpecialityCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class SpecialityCommand
    {
        public sealed record GetSpecialityQuery(Expression<Func<Speciality, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<SpecialityDto>>;
        public sealed record CreateSpecialityRequest(SpecialityDto SpecialityDto, bool ReturnNewData = false) : IRequest<SpecialityDto>;
        public sealed record CreateListSpecialityRequest(List<SpecialityDto> SpecialityDtos, bool ReturnNewData = false) : IRequest<List<SpecialityDto>>;
        public sealed record UpdateSpecialityRequest(SpecialityDto SpecialityDto, bool ReturnNewData = false) : IRequest<SpecialityDto>;
        public sealed record UpdateListSpecialityRequest(List<SpecialityDto> SpecialityDtos, bool ReturnNewData = false) : IRequest<List<SpecialityDto>>;
        public sealed record DeleteSpecialityRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class SpecialityQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataSpeciality) :
        IRequestHandler<GetSpecialityQuery, List<SpecialityDto>>,
        IRequestHandler<CreateSpecialityRequest, SpecialityDto>,
        IRequestHandler<CreateListSpecialityRequest, List<SpecialityDto>>,
        IRequestHandler<UpdateSpecialityRequest, SpecialityDto>,
        IRequestHandler<UpdateListSpecialityRequest, List<SpecialityDto>>,
        IRequestHandler<DeleteSpecialityRequest, bool>
    {
        private string CacheKey = "GetSpecialityQuery_";

        private async Task<(SpecialityDto, List<SpecialityDto>)> Result(Speciality? result = null, List<Speciality>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<SpecialityDto>(), []);
                else
                    return ((await unitOfWork.Repository<Speciality>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<SpecialityDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<SpecialityDto>>());
                else
                    return (new(), (await unitOfWork.Repository<Speciality>().Entities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<SpecialityDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<SpecialityDto>> Handle(GetSpecialityQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<Speciality>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<Speciality>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable()?.Where(request.Predicate)?.ToList() ?? [];

            return result?.Adapt<List<SpecialityDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<SpecialityDto> Handle(CreateSpecialityRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialityDto.Adapt<CreateUpdateSpecialityDto>();
            var result = await unitOfWork.Repository<Speciality>().AddAsync(req.Adapt<Speciality>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSpeciality.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<SpecialityDto>> Handle(CreateListSpecialityRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialityDtos.Adapt<List<CreateUpdateSpecialityDto>>();
            var result = await unitOfWork.Repository<Speciality>().AddAsync(req.Adapt<List<Speciality>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSpeciality.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<SpecialityDto> Handle(UpdateSpecialityRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialityDto.Adapt<CreateUpdateSpecialityDto>();
            var result = await unitOfWork.Repository<Speciality>().UpdateAsync(req.Adapt<Speciality>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataSpeciality.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<SpecialityDto>> Handle(UpdateListSpecialityRequest request, CancellationToken cancellationToken)
        {
            var req = request.SpecialityDtos.Adapt<CreateUpdateSpecialityDto>();
            var result = await unitOfWork.Repository<Speciality>().UpdateAsync(req.Adapt<List<Speciality>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataSpeciality.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteSpecialityRequest request, CancellationToken cancellationToken)
        {
            List<Speciality> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var Speciality = await unitOfWork.Repository<Speciality>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (Speciality != null)
                {
                    deletedCountries.Add(Speciality);
                    await unitOfWork.Repository<Speciality>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<Speciality>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<Speciality>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataSpeciality.Clients.All.ReceiveNotification(new ReceiveDataDto
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