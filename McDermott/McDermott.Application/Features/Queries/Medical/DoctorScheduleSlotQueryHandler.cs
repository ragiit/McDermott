using static McDermott.Application.Features.Commands.Medical.DoctorScheduleSlotCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DoctorScheduleSlotQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDoctorScheduleSlotQuery, List<DoctorScheduleSlotDto>>,
        IRequestHandler<CreateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<CreateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<UpdateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<UpdateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<DeleteDoctorScheduleSlotRequest, bool>
    {
        #region GET

        public async Task<List<DoctorScheduleSlotDto>> Handle(GetDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDoctorScheduleSlotQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<DoctorScheduleSlot>? result))
                {
                    result = await _unitOfWork.Repository<DoctorScheduleSlot>().Entities
                       .Include(x => x.DoctorSchedule)
                       .Include(x => x.Physician)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DoctorScheduleSlotDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DoctorScheduleSlotDto> Handle(CreateDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleSlot>().AddAsync(request.DoctorScheduleSlotDto.Adapt<DoctorScheduleSlot>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleSlotQuery_");

                return result.Adapt<DoctorScheduleSlotDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DoctorScheduleSlotDto>> Handle(CreateListDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleSlot>().AddAsync(request.DoctorScheduleSlotDtos.Adapt<List<DoctorScheduleSlot>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleSlotQuery_");

                return result.Adapt<List<DoctorScheduleSlotDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DoctorScheduleSlotDto> Handle(UpdateDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleSlot>().UpdateAsync(request.DoctorScheduleSlotDto.Adapt<DoctorScheduleSlot>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleSlotQuery_");

                return result.Adapt<DoctorScheduleSlotDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DoctorScheduleSlotDto>> Handle(UpdateListDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleSlot>().UpdateAsync(request.DoctorScheduleSlotDtos.Adapt<List<DoctorScheduleSlot>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleSlotQuery_");

                return result.Adapt<List<DoctorScheduleSlotDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDoctorScheduleSlotRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DoctorScheduleSlot>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleSlotQuery_");

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
    }
}
