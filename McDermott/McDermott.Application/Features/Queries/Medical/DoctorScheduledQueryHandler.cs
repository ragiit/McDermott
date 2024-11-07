using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Medical.DoctorScheduledCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DoctorScheduledQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDoctorScheduledQuery, (List<DoctorScheduleDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleDoctorScheduledQuery, DoctorScheduleDto>, IRequestHandler<ValidateDoctorScheduledQuery, bool>,
        IRequestHandler<CreateDoctorScheduledRequest, DoctorScheduleDto>,
        IRequestHandler<BulkValidateDoctorScheduledQuery, List<DoctorScheduleDto>>,
        IRequestHandler<CreateListDoctorScheduledRequest, List<DoctorScheduleDto>>,
        IRequestHandler<UpdateDoctorScheduledRequest, DoctorScheduleDto>,
        IRequestHandler<UpdateListDoctorScheduledRequest, List<DoctorScheduleDto>>,
        IRequestHandler<DeleteDoctorScheduledRequest, bool>,
        IRequestHandler<DeleteDoctorScheduledWithDetail, bool>
    {
        #region GET

        public async Task<List<DoctorScheduleDto>> Handle(BulkValidateDoctorScheduledQuery request, CancellationToken cancellationToken)
        {
            var DoctorScheduleDtos = request.DoctorSchedulesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DoctorScheduleNames = DoctorScheduleDtos.Select(x => x.Name).Distinct().ToList();
            //var a = DoctorScheduleDtos.Select(x => x.CountryId).Distinct().ToList();

            //var existingDoctorSchedules = await _unitOfWork.Repository<DoctorSchedule>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => DoctorScheduleNames.Contains(v.Name)
            //                && a.Contains(v.CountryId))
            //.ToListAsync(cancellationToken);

            return [];
            //return existingDoctorSchedules.Adapt<List<DoctorScheduleDto>>();
        }

        public async Task<bool> Handle(ValidateDoctorScheduledQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DoctorSchedule>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<DoctorScheduleDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDoctorScheduledQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorSchedule>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DoctorSchedule>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DoctorSchedule>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DoctorSchedule
                    {
                        Id = x.Id,
                        PhysicionId = x.PhysicionId,
                        Physicion = new User
                        {
                            Name = x.Physicion == null ? string.Empty : x.Physicion.Name,
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

                    return (pagedItems.Adapt<List<DoctorScheduleDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DoctorScheduleDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DoctorScheduleDto> Handle(GetSingleDoctorScheduledQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorSchedule>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DoctorSchedule>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DoctorSchedule>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DoctorSchedule
                    {
                        Id = x.Id,
                        PhysicionId = x.PhysicionId,
                        Physicion = new User
                        {
                            Name = x.Name,
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DoctorScheduleDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DoctorScheduleDto> Handle(CreateDoctorScheduledRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorSchedule>().AddAsync(request.DoctorScheduleDto.Adapt<DoctorScheduleDto>().Adapt<DoctorSchedule>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DoctorScheduleDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DoctorScheduleDto>> Handle(CreateListDoctorScheduledRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorSchedule>().AddAsync(request.DoctorScheduleDtos.Adapt<List<DoctorSchedule>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DoctorScheduleDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DoctorScheduleDto> Handle(UpdateDoctorScheduledRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorSchedule>().UpdateAsync(request.DoctorScheduleDto.Adapt<CreateUpdateDoctorScheduleDto>().Adapt<DoctorSchedule>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DoctorScheduleDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DoctorScheduleDto>> Handle(UpdateListDoctorScheduledRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorSchedule>().UpdateAsync(request.DoctorScheduleDtos.Adapt<List<DoctorSchedule>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DoctorScheduleDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDoctorScheduledRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DoctorSchedule>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DoctorSchedule>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(DeleteDoctorScheduledWithDetail request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    var a = await _unitOfWork.Repository<DoctorScheduleDetail>().Entities.Where(x => x.DoctorScheduleId == request.Id).Select(x => x.Id).ToListAsync();
                    await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(a);
                }

                if (request.Ids.Count > 0)
                {
                    foreach (var id in request.Ids)
                    {
                        var a = await _unitOfWork.Repository<DoctorScheduleDetail>().Entities.Where(x => x.DoctorScheduleId == id).Select(x => x.Id).ToListAsync();
                        await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(a);
                    }
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DoctorSchedule>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DoctorSchedule>().DeleteAsync(request.Ids);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleQuery_"); // Ganti dengan key yang sesuai

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