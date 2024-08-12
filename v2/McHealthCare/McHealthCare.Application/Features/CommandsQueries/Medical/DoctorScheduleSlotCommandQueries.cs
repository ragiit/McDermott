using Mapster;
using McHealthCare.Application.Dtos.Medical;
using McHealthCare.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McHealthCare.Application.Features.CommandsQueries.Medical.DoctorScheduleSlotCommand;

namespace McHealthCare.Application.Features.CommandsQueries.Medical
{
    public sealed class DoctorScheduleSlotCommand
    {
        public sealed record GetDoctorScheduleSlotQuery(Expression<Func<DoctorScheduleSlot, bool>>? Predicate = null, bool RemoveCache = false) : IRequest<List<DoctorScheduleSlotDto>>;
        public sealed record CreateDoctorScheduleSlotRequest(DoctorScheduleSlotDto DoctorScheduleSlotDto, bool ReturnNewData = false) : IRequest<DoctorScheduleSlotDto>;
        public sealed record CreateListDoctorScheduleSlotRequest(List<DoctorScheduleSlotDto> DoctorScheduleSlotDtos, bool ReturnNewData = false) : IRequest<List<DoctorScheduleSlotDto>>;
        public sealed record UpdateDoctorScheduleSlotRequest(DoctorScheduleSlotDto DoctorScheduleSlotDto, bool ReturnNewData = false) : IRequest<DoctorScheduleSlotDto>;
        public sealed record UpdateListDoctorScheduleSlotRequest(List<DoctorScheduleSlotDto> DoctorScheduleSlotDtos, bool ReturnNewData = false) : IRequest<List<DoctorScheduleSlotDto>>;
        public sealed record DeleteDoctorScheduleSlotRequest(Guid? Id = null, List<Guid>? Ids = null) : IRequest<bool>;
    }

    public sealed class DoctorScheduleSlotQueryHandler(IUnitOfWork unitOfWork, IMemoryCache cache, IHubContext<NotificationHub, INotificationClient> dataDoctorScheduleSlot) :
        IRequestHandler<GetDoctorScheduleSlotQuery, List<DoctorScheduleSlotDto>>,
        IRequestHandler<CreateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<CreateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<UpdateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<UpdateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<DeleteDoctorScheduleSlotRequest, bool>
    {
        private string CacheKey = "GetDoctorScheduleSlotQuery_";

        private async Task<(DoctorScheduleSlotDto, List<DoctorScheduleSlotDto>)> Result(DoctorScheduleSlot? result = null, List<DoctorScheduleSlot>? results = null, bool ReturnNewData = false, CancellationToken cancellationToken = default)
        {
            if (result is not null)
            {
                if (!ReturnNewData)
                    return (result.Adapt<DoctorScheduleSlotDto>(), []);
                else
                    return ((await unitOfWork.Repository<DoctorScheduleSlot>().Entities
                        .AsNoTracking()
                        .Include(x=>x.DoctorSchedule)
                        .Include(x=>x.Physician)
                        .FirstOrDefaultAsync(x => x.Id == result.Id, cancellationToken: cancellationToken)).Adapt<DoctorScheduleSlotDto>(), []);
            }
            else if (results is not null)
            {
                if (!ReturnNewData)
                    return (new(), results.Adapt<List<DoctorScheduleSlotDto>>());
                else
                    return (new(), (await unitOfWork.Repository<DoctorScheduleSlot>().Entities
                        .AsNoTracking()
                        .Include(x=>x.DoctorSchedule)
                        .Include(x=>x.Physician)
                        .FirstOrDefaultAsync(x => results.Select(z => z.Id).Contains(x.Id), cancellationToken: cancellationToken)).Adapt<List<DoctorScheduleSlotDto>>());
            }

            return (new(), []);
        }

        #region GET

        public async Task<List<DoctorScheduleSlotDto>> Handle(GetDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            if (request.RemoveCache)
                cache.Remove(CacheKey);

            List<DoctorScheduleSlot>? result = [];

            if (!cache.TryGetValue(CacheKey, out result))
            {
                result = await unitOfWork.Repository<DoctorScheduleSlot>().Entities
                        .AsNoTracking()
                        .Include(x=>x.DoctorSchedule)
                        .Include(x=>x.Physician)
                        .ToListAsync(cancellationToken);
                cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));
            }

            if (request.Predicate is not null)
                result = result?.AsQueryable().Where(request.Predicate).ToList();

            return result?.Adapt<List<DoctorScheduleSlotDto>>() ?? [];
        }

        #endregion GET

        #region CREATE

        public async Task<DoctorScheduleSlotDto> Handle(CreateDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleSlotDto.Adapt<CreateUpdateDoctorScheduleSlotDto>();
            var result = await unitOfWork.Repository<DoctorScheduleSlot>().AddAsync(req.Adapt<DoctorScheduleSlot>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorScheduleSlot.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DoctorScheduleSlotDto>> Handle(CreateListDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleSlotDtos.Adapt<List<CreateUpdateDoctorScheduleSlotDto>>();
            var result = await unitOfWork.Repository<DoctorScheduleSlot>().AddAsync(req.Adapt<List<DoctorScheduleSlot>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorScheduleSlot.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Create,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DoctorScheduleSlotDto> Handle(UpdateDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleSlotDto.Adapt<CreateUpdateDoctorScheduleSlotDto>();
            var result = await unitOfWork.Repository<DoctorScheduleSlot>().UpdateAsync(req.Adapt<DoctorScheduleSlot>());
            await unitOfWork.SaveChangesAsync(cancellationToken);

            await dataDoctorScheduleSlot.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            cache.Remove(CacheKey);

            return (await Result(result: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item1;
        }

        public async Task<List<DoctorScheduleSlotDto>> Handle(UpdateListDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            var req = request.DoctorScheduleSlotDtos.Adapt<CreateUpdateDoctorScheduleSlotDto>();
            var result = await unitOfWork.Repository<DoctorScheduleSlot>().UpdateAsync(req.Adapt<List<DoctorScheduleSlot>>());
            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            await dataDoctorScheduleSlot.Clients.All.ReceiveNotification(new ReceiveDataDto
            {
                Type = EnumTypeReceiveData.Update,
                Data = result
            });

            return (await Result(results: result, ReturnNewData: request.ReturnNewData, cancellationToken: cancellationToken)).Item2;
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            List<DoctorScheduleSlot> deletedCountries = [];

            if (request.Id.HasValue)
            {
                var DoctorScheduleSlot = await unitOfWork.Repository<DoctorScheduleSlot>().Entities.FirstOrDefaultAsync(x => x.Id == request.Id.GetValueOrDefault());
                if (DoctorScheduleSlot != null)
                {
                    deletedCountries.Add(DoctorScheduleSlot);
                    await unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(request.Id.GetValueOrDefault());
                }
            }

            if (request.Ids?.Count > 0)
            {
                deletedCountries.AddRange(await unitOfWork.Repository<DoctorScheduleSlot>().Entities
                    .Where(x => request.Ids.Contains(x.Id))
                    .ToListAsync(cancellationToken));

                await unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(x => request.Ids.Contains(x.Id));
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
            cache.Remove(CacheKey);

            if (deletedCountries.Count > 0)
            {
                await dataDoctorScheduleSlot.Clients.All.ReceiveNotification(new ReceiveDataDto
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
