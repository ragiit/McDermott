using Microsoft.EntityFrameworkCore;

using static McDermott.Application.Features.Commands.Medical.DoctorScheduleSlotCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DoctorScheduleSlotQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDoctorScheduleSlotQuery, (List<DoctorScheduleSlotDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<CreateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<CreateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<UpdateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<UpdateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<DeleteDoctorScheduleSlotRequest, bool>
    {
        #region GET

        public async Task<(List<DoctorScheduleSlotDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorScheduleSlot>().Entities
                    .Include(x => x.Physician)
                    .Include(x => x.DoctorSchedule)
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.StartDate.ToString(), $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.WorkFrom.ToString(), $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.WorkTo.ToString(), $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);
                var pagedResult = query
                            .OrderBy(x => x.StartDate);

                var skip = (request.PageIndex) * (request.PageSize == 0 ? totalCount : request.PageSize);

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<DoctorScheduleSlotDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DoctorScheduleSlot>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
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