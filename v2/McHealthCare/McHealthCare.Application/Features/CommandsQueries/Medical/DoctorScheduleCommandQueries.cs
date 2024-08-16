using McHealthCare.Domain.Entities.Medical;
using static McHealthCare.Application.Features.CommandsQueries.Medical.DoctorScheduleCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class DoctorScheduleCommand
    {
        public sealed record GetDoctorScheduleQuery(Expression<Func<DoctorSchedule, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DoctorScheduleDto>>;
        public sealed record CreateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto, bool ReturnNewData = false) : IRequest<DoctorScheduleDto>;
        public sealed record CreateListDoctorScheduleRequest(List<DoctorScheduleDto> DoctorScheduleDtos, bool ReturnNewData = false) : IRequest<List<DoctorScheduleDto>>;
        public sealed record UpdateDoctorScheduleRequest(DoctorScheduleDto DoctorScheduleDto, bool ReturnNewData = false) : IRequest<DoctorScheduleDto>;
        public sealed record UpdateListDoctorScheduleRequest(List<DoctorScheduleDto> DoctorScheduleDtos, bool ReturnNewData = false) : IRequest<List<DoctorScheduleDto>>;
        public sealed record DeleteDoctorScheduleRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DoctorScheduleQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataDoctorSchedule) :
        IRequestHandler<GetDoctorScheduleQuery, List<DoctorScheduleDto>>,
        IRequestHandler<CreateDoctorScheduleRequest, DoctorScheduleDto>,
        IRequestHandler<CreateListDoctorScheduleRequest, List<DoctorScheduleDto>>,
        IRequestHandler<UpdateDoctorScheduleRequest, DoctorScheduleDto>,
        IRequestHandler<UpdateListDoctorScheduleRequest, List<DoctorScheduleDto>>,
        IRequestHandler<DeleteDoctorScheduleRequest, bool>
    {
        private string CacheKey = "GetDoctorScheduleQuery_";

        private async Task<(DoctorScheduleDto, List<DoctorScheduleDto>)> Result(DoctorSchedule? result = null, List<DoctorSchedule>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DoctorScheduleDto>(), []);
                else
                    return ((await unitOfWork.Repository<DoctorSchedule>().Entities
                        .AsNoTracking()
                        .Include(x => x.Service)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DoctorScheduleDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DoctorScheduleDto>>());
                else
                    return (new(), (await unitOfWork.Repository<DoctorSchedule>().Entities
                        .AsNoTracking()
                        .Include(x => x.Service)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DoctorScheduleDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DoctorScheduleDto>> Handle(GetDoctorScheduleQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<DoctorSchedule>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<DoctorSchedule>().Entities
                        .AsNoTracking()
                        .Include(x => x.Service)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DoctorScheduleDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DoctorScheduleDto> Handle(CreateDoctorScheduleRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDto.Adapt<CreateUpdateDoctorScheduleDto>();
            var result = await unitOfWork.Repository<DoctorSchedule>().AddAsync(req.Adapt<DoctorSchedule>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorSchedule.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DoctorScheduleDto>> Handle(CreateListDoctorScheduleRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDtos.Adapt<List<CreateUpdateDoctorScheduleDto>>();
            var result = await unitOfWork.Repository<DoctorSchedule>().AddAsync(req.Adapt<List<DoctorSchedule>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorSchedule.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DoctorScheduleDto> Handle(UpdateDoctorScheduleRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDto.Adapt<CreateUpdateDoctorScheduleDto>();
            var result = await unitOfWork.Repository<DoctorSchedule>().UpdateAsync(req.Adapt<DoctorSchedule>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataDoctorSchedule.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DoctorScheduleDto>> Handle(UpdateListDoctorScheduleRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDtos.Adapt<CreateUpdateDoctorScheduleDto>();
            var result = await unitOfWork.Repository<DoctorSchedule>().UpdateAsync(req.Adapt<List<DoctorSchedule>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorSchedule.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDoctorScheduleRequest request, CancellationToken cancellationToken)
        {
            List<DoctorSchedule> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var DoctorSchedule = await unitOfWork.Repository<DoctorSchedule>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (DoctorSchedule != null)
                {
                    deletedCountries.Add(DoctorSchedule);
                    await unitOfWork.Repository<DoctorSchedule>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<DoctorSchedule>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<DoctorSchedule>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataDoctorSchedule.Clients.All.ReceiveNotification(new ReceiveDataDto
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