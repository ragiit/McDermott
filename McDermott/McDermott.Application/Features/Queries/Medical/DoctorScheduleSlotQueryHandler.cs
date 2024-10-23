using McDermott.Application.Features.Services;
using Microsoft.EntityFrameworkCore;

using static McDermott.Application.Features.Commands.Medical.DoctorScheduleSlotCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DoctorScheduleSlotQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDoctorScheduleSlotQuery, (List<DoctorScheduleSlotDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleDoctorScheduleSlotQuery, DoctorScheduleSlotDto>, IRequestHandler<BulkValidateDoctorScheduleSlotQuery, List<DoctorScheduleSlotDto>>,
        IRequestHandler<CreateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<CreateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<UpdateDoctorScheduleSlotRequest, DoctorScheduleSlotDto>,
        IRequestHandler<UpdateListDoctorScheduleSlotRequest, List<DoctorScheduleSlotDto>>,
        IRequestHandler<DeleteDoctorScheduleSlotRequest, bool>
    {
        #region GET

        public async Task<List<DoctorScheduleSlotDto>> Handle(BulkValidateDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            var DoctorScheduleSlotDtos = request.DoctorScheduleSlotsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DoctorScheduleSlotNames = DoctorScheduleSlotDtos.Select(x => x.PhysicianId).Distinct().ToList();
            var a = DoctorScheduleSlotDtos.Select(x => x.StartDate).Distinct().ToList();
            var b = DoctorScheduleSlotDtos.Select(x => x.WorkFrom).Distinct().ToList();
            var c = DoctorScheduleSlotDtos.Select(x => x.WorkTo).Distinct().ToList();
            var d = DoctorScheduleSlotDtos.Select(x => x.Quota).Distinct().ToList();
            var e = DoctorScheduleSlotDtos.Select(x => x.ServiceId).Distinct().ToList();
            var f = DoctorScheduleSlotDtos.Select(x => x.DayOfWeek).Distinct().ToList();

            var existingDoctorScheduleSlots = await _unitOfWork.Repository<DoctorScheduleSlot>()
            .Entities
            .AsNoTracking()
            .Where(v => DoctorScheduleSlotNames.Contains(v.PhysicianId.GetValueOrDefault())
                        && a.Contains(v.StartDate)
                        && b.Contains(v.WorkFrom)
                        && c.Contains(v.WorkTo)
                        && d.Contains(v.Quota)
                        && e.Contains(v.ServiceId)
                        && f.Contains(v.DayOfWeek))
            .ToListAsync(cancellationToken);

            return existingDoctorScheduleSlots.Adapt<List<DoctorScheduleSlotDto>>();
        }

        public async Task<(List<DoctorScheduleSlotDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorScheduleSlot>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<DoctorScheduleSlot>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DoctorScheduleSlot>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                            EF.Functions.Like(v.Physician.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DoctorScheduleSlot
                    {
                        Id = x.Id,
                        PhysicianId = x.PhysicianId,
                        Physician = new User
                        {
                            Name = x.Physician == null ? "" : x.Physician.Name,
                        },
                        StartDate = x.StartDate,
                        DayOfWeek = x.DayOfWeek,
                        WorkFrom = x.WorkFrom,
                        WorkTo = x.WorkTo,
                        Quota = x.Quota,
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? "" : x.Service.Name
                        }
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<DoctorScheduleSlotDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DoctorScheduleSlotDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DoctorScheduleSlotDto> Handle(GetSingleDoctorScheduleSlotQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorScheduleSlot>().Entities.AsNoTracking();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply ordering
                if (request.OrderByList.Count != 0)
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<DoctorScheduleSlot>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DoctorScheduleSlot>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                            EF.Functions.Like(v.Physician.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DoctorScheduleSlot
                    {
                        Id = x.Id,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DoctorScheduleSlotDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
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
                var aa = request.DoctorScheduleSlotDto.Adapt<CreateUpdateDoctorScheduleSlotDto>().Adapt<DoctorScheduleSlot>();
                var result = await _unitOfWork.Repository<DoctorScheduleSlot>().UpdateAsync(aa);

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