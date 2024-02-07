
namespace McDermott.Application.Features.Queries.Medical
{
    public class DoctorScheduleQueryHandler
    {
        internal class GetAllDoctorScheduleQueryHandler : IRequestHandler<GetDoctorScheduleQuery, List<DoctorScheduleDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllDoctorScheduleQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DoctorScheduleDto>> Handle(GetDoctorScheduleQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<DoctorSchedule>().Entities
                        .Include(x => x.Service)
                        .Select(DoctorSchedule => DoctorSchedule.Adapt<DoctorScheduleDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }

        internal class GetDoctorScheduleSlotQueryHandler : IRequestHandler<GetDoctorScheduleSlotQuery, List<DoctorScheduleSlotDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDoctorScheduleSlotQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DoctorScheduleSlotDto>> Handle(GetDoctorScheduleSlotQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<DoctorScheduleSlot>().Entities
                        .Select(DoctorSchedule => DoctorSchedule.Adapt<DoctorScheduleSlotDto>())
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
            }
        }


        internal class GetDoctorScheduleSlotByDoctorScheduleIdRequestHandler : IRequestHandler<GetDoctorScheduleSlotByDoctorScheduleIdRequest, List<DoctorScheduleSlotDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDoctorScheduleSlotByDoctorScheduleIdRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DoctorScheduleSlotDto>> Handle(GetDoctorScheduleSlotByDoctorScheduleIdRequest query, CancellationToken cancellationToken)
            {
                if (query.PhysicianId == 0)
                {
                    return await _unitOfWork.Repository<DoctorScheduleSlot>().Entities
                       .Where(x => x.DoctorScheduleId == query.DoctorScheduleId)
                       .Select(DoctorSchedule => DoctorSchedule.Adapt<DoctorScheduleSlotDto>())
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);
                }
                else
                {
                    return await _unitOfWork.Repository<DoctorScheduleSlot>().Entities
                       .Where(x => x.DoctorScheduleId == query.DoctorScheduleId && x.PhysicianId == query.PhysicianId)
                       .Select(DoctorSchedule => DoctorSchedule.Adapt<DoctorScheduleSlotDto>())
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);
                }
            }
        }

        internal class DeleteDoctorScheduleHandler : IRequestHandler<DeleteDoctorScheduleRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDoctorScheduleHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDoctorScheduleRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<DoctorSchedule>().DeleteAsync(request.Id);

                    var doctorScheduleDetailsa = await _unitOfWork.Repository<DoctorScheduleDetail>().GetAllAsync();
                    var doctorScheduleSlots = await _unitOfWork.Repository<DoctorScheduleSlot>().GetAllAsync();

                    foreach (var item in doctorScheduleDetailsa.Where(x => x.DoctorScheduleId == request.Id).ToList())
                    {
                        await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(item.Id);
                    }

                    foreach (var item in doctorScheduleSlots.Where(x => x.DoctorScheduleId == request.Id).ToList())
                    {
                        await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(item.Id);
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        internal class DeleteListDoctorScheduleHandler : IRequestHandler<DeleteListDoctorScheduleRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDoctorScheduleHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDoctorScheduleRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DoctorSchedule>().DeleteAsync(request.Id);

                foreach (var item1 in request.Id)
                {
                    var doctorScheduleDetailsa = await _unitOfWork.Repository<DoctorScheduleDetail>().GetAllAsync();
                    var doctorScheduleSlots = await _unitOfWork.Repository<DoctorScheduleSlot>().GetAllAsync();

                    foreach (var item in doctorScheduleDetailsa.Where(x => x.DoctorScheduleId == item1).ToList())
                    {
                        await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(item.Id);
                    }

                    foreach (var item in doctorScheduleSlots.Where(x => x.DoctorScheduleId == item1).ToList())
                    {
                        await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(item.Id);
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class CreateDoctorScheduleDetailHandler : IRequestHandler<CreateDoctorScheduleDetailRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDoctorScheduleDetailHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(CreateDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var item in request.DoctorScheduleDetailDtos)
                    {
                        await _unitOfWork.Repository<DoctorScheduleDetail>().AddAsync(item.Adapt<DoctorScheduleDetail>());
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        internal class CreateDoctorScheduleSlotRequestHandler : IRequestHandler<CreateDoctorScheduleSlotRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDoctorScheduleSlotRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(CreateDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    request.DoctorScheduleSlotDto.ForEach(async x =>
                    {
                        await _unitOfWork.Repository<DoctorScheduleSlot>().AddAsync(x.Adapt<DoctorScheduleSlot>());
                    });

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        internal class DeleteDoctorScheduleDetailByScheduleIdHandler : IRequestHandler<DeleteDoctorScheduleDetailByScheduleIdRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDoctorScheduleDetailByScheduleIdHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDoctorScheduleDetailByScheduleIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(request.Id);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        internal class GetDoctorScheduleDetailByScheduleIdQueryHandler : IRequestHandler<GetDoctorScheduleDetailByScheduleIdQuery, List<DoctorScheduleDetailDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDoctorScheduleDetailByScheduleIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<DoctorScheduleDetailDto>> Handle(GetDoctorScheduleDetailByScheduleIdQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<DoctorScheduleDetail>().Entities
                     .Include(x => x.DoctorSchedule)
                     .Where(x => x.DoctorScheduleId == query.DoctorScheduleId)
                     .Select(x => x.Adapt<DoctorScheduleDetailDto>())
                     .AsNoTracking()
                     .ToListAsync(cancellationToken);
            }
        }

        internal class GetDoctorScheduleByIdQueryHandler : IRequestHandler<GetDoctorScheduleByIdQuery, DoctorScheduleDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetDoctorScheduleByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DoctorScheduleDto> Handle(GetDoctorScheduleByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<DoctorSchedule>().Entities.Include(x => x.Service).AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

                return result.Adapt<DoctorScheduleDto>();
            }
        }

        internal class CreateDoctorScheduleHandler : IRequestHandler<CreateDoctorScheduleRequest, DoctorScheduleDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateDoctorScheduleHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<DoctorScheduleDto> Handle(CreateDoctorScheduleRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<DoctorSchedule>().AddAsync(request.DoctorScheduleDto.Adapt<DoctorSchedule>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<DoctorScheduleDto>();
            }
        }

        internal class UpdateDoctorScheduleHandler : IRequestHandler<UpdateDoctorScheduleRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateDoctorScheduleHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateDoctorScheduleRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DoctorSchedule>().UpdateAsync(request.DoctorScheduleDto.Adapt<DoctorSchedule>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        #region Delete
        internal class DeleteDoctorScheduleSlotRequestHandler : IRequestHandler<DeleteDoctorScheduleSlotRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDoctorScheduleSlotRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        internal class DeleteDoctorScheduleSlotByPhysicionIdRequestHandler : IRequestHandler<DeleteDoctorScheduleSlotByPhysicionIdRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDoctorScheduleSlotByPhysicionIdRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDoctorScheduleSlotByPhysicionIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(x => x.DoctorScheduleId == request.DoctorScheduleId && !request.PhysicianIds.Contains((int)x.PhysicianId!));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        internal class DeleteListDoctorScheduleSlotRequestHandler : IRequestHandler<DeleteListDoctorScheduleSlotRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDoctorScheduleSlotRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
            {
                foreach (var id in request.Id)
                {
                    await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(id);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        internal class DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequestHandler : IRequestHandler<DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteDoctorScheduleSlotByScheduleIdPhysicionIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var schedules in request.ScheduleId.Distinct())
                    {
                        await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(x => x.PhysicianId == request.PhysicionId && x.DoctorScheduleId == schedules);
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        internal class DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequestHandler : IRequestHandler<DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequestHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListDoctorScheduleSlotByScheduleIdPhysicionIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    foreach (var physician in request.PhysicionId)
                    {
                        foreach (var schedules in request.ScheduleId.Distinct())
                        {
                            await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(x => x.PhysicianId == physician && x.DoctorScheduleId == schedules);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return true;
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }
        #endregion
    }
}