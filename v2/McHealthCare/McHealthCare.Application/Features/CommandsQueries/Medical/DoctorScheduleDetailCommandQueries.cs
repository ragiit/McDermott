using McHealthCare.Domain.Entities.Medical;
using static McHealthCare.Application.Features.CommandsQueries.Medical.DoctorScheduleDetailCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class DoctorScheduleDetailCommand
    {
        public sealed record GetDoctorScheduleDetailQuery(Expression<Func<DoctorScheduleDetail, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DoctorScheduleDetailDto>>;
        public sealed record CreateDoctorScheduleDetailRequest(DoctorScheduleDetailDto DoctorScheduleDetailDto, bool ReturnNewData = false) : IRequest<DoctorScheduleDetailDto>;
        public sealed record CreateListDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos, bool ReturnNewData = false) : IRequest<List<DoctorScheduleDetailDto>>;
        public sealed record UpdateDoctorScheduleDetailRequest(DoctorScheduleDetailDto DoctorScheduleDetailDto, bool ReturnNewData = false) : IRequest<DoctorScheduleDetailDto>;
        public sealed record UpdateListDoctorScheduleDetailRequest(List<DoctorScheduleDetailDto> DoctorScheduleDetailDtos, bool ReturnNewData = false) : IRequest<List<DoctorScheduleDetailDto>>;
        public sealed record DeleteDoctorScheduleDetailRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DoctorScheduleDetailQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataDoctorScheduleDetail) :
        IRequestHandler<GetDoctorScheduleDetailQuery, List<DoctorScheduleDetailDto>>,
        IRequestHandler<CreateDoctorScheduleDetailRequest, DoctorScheduleDetailDto>,
        IRequestHandler<CreateListDoctorScheduleDetailRequest, List<DoctorScheduleDetailDto>>,
        IRequestHandler<UpdateDoctorScheduleDetailRequest, DoctorScheduleDetailDto>,
        IRequestHandler<UpdateListDoctorScheduleDetailRequest, List<DoctorScheduleDetailDto>>,
        IRequestHandler<DeleteDoctorScheduleDetailRequest, bool>
    {
        private string CacheKey = "GetDoctorScheduleDetailQuery_";

        private async Task<(DoctorScheduleDetailDto, List<DoctorScheduleDetailDto>)> Result(DoctorScheduleDetail? result = null, List<DoctorScheduleDetail>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DoctorScheduleDetailDto>(), []);
                else
                    return ((await unitOfWork.Repository<DoctorScheduleDetail>().Entities
                        .AsNoTracking()
                        .Include(x => x.DoctorSchedule)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DoctorScheduleDetailDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DoctorScheduleDetailDto>>());
                else
                    return (new(), (await unitOfWork.Repository<DoctorScheduleDetail>().Entities
                        .AsNoTracking()
                        .Include(x => x.DoctorSchedule)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DoctorScheduleDetailDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DoctorScheduleDetailDto>> Handle(GetDoctorScheduleDetailQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<DoctorScheduleDetail>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<DoctorScheduleDetail>().Entities
                        .AsNoTracking()
                        .Include(x => x.DoctorSchedule)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DoctorScheduleDetailDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DoctorScheduleDetailDto> Handle(CreateDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDetailDto.Adapt<CreateUpdateDoctorScheduleDetailDto>();
            var result = await unitOfWork.Repository<DoctorScheduleDetail>().AddAsync(req.Adapt<DoctorScheduleDetail>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorScheduleDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DoctorScheduleDetailDto>> Handle(CreateListDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDetailDtos.Adapt<List<CreateUpdateDoctorScheduleDetailDto>>();
            var result = await unitOfWork.Repository<DoctorScheduleDetail>().AddAsync(req.Adapt<List<DoctorScheduleDetail>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorScheduleDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DoctorScheduleDetailDto> Handle(UpdateDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDetailDto.Adapt<CreateUpdateDoctorScheduleDetailDto>();
            var result = await unitOfWork.Repository<DoctorScheduleDetail>().UpdateAsync(req.Adapt<DoctorScheduleDetail>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataDoctorScheduleDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DoctorScheduleDetailDto>> Handle(UpdateListDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleDetailDtos.Adapt<CreateUpdateDoctorScheduleDetailDto>();
            var result = await unitOfWork.Repository<DoctorScheduleDetail>().UpdateAsync(req.Adapt<List<DoctorScheduleDetail>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorScheduleDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            List<DoctorScheduleDetail> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var DoctorScheduleDetail = await unitOfWork.Repository<DoctorScheduleDetail>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (DoctorScheduleDetail != null)
                {
                    deletedCountries.Add(DoctorScheduleDetail);
                    await unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<DoctorScheduleDetail>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataDoctorScheduleDetail.Clients.All.ReceiveNotification(new ReceiveDataDto
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