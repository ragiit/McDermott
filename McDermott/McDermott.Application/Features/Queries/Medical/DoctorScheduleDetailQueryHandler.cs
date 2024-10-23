using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Medical.DoctorScheduleDetailCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DoctorScheduleDetailQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
       IRequestHandler<GetDoctorScheduleDetailQuery, (List<DoctorScheduleDetailDto>, int pageIndex, int pageSize, int pageCount)>,
       IRequestHandler<GetSingleDoctorScheduleDetailQuery, DoctorScheduleDetailDto>, IRequestHandler<ValidateDoctorScheduleDetailQuery, bool>,
       IRequestHandler<CreateDoctorScheduleDetailRequest, DoctorScheduleDetailDto>,
       IRequestHandler<BulkValidateDoctorScheduleDetailQuery, List<DoctorScheduleDetailDto>>,
       IRequestHandler<CreateListDoctorScheduleDetailRequest, List<DoctorScheduleDetailDto>>,
       IRequestHandler<UpdateDoctorScheduleDetailRequest, DoctorScheduleDetailDto>,
       IRequestHandler<UpdateListDoctorScheduleDetailRequest, List<DoctorScheduleDetailDto>>,
       IRequestHandler<DeleteDoctorScheduleDetailRequest, bool>
    {
        #region GET

        public async Task<List<DoctorScheduleDetailDto>> Handle(BulkValidateDoctorScheduleDetailQuery request, CancellationToken cancellationToken)
        {
            var DoctorScheduleDetailDtos = request.DoctorScheduleDetailsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var DoctorScheduleDetailNames = DoctorScheduleDetailDtos.Select(x => x.Name).Distinct().ToList();
            //var a = DoctorScheduleDetailDtos.Select(x => x.CountryId).Distinct().ToList();

            //var existingDoctorScheduleDetails = await _unitOfWork.Repository<DoctorScheduleDetail>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => DoctorScheduleDetailNames.Contains(v.Name)
            //                && a.Contains(v.CountryId))
            //    .ToListAsync(cancellationToken);

            //return existingDoctorScheduleDetails.Adapt<List<DoctorScheduleDetailDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateDoctorScheduleDetailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DoctorScheduleDetail>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<DoctorScheduleDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDoctorScheduleDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorScheduleDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DoctorScheduleDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DoctorScheduleDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new DoctorScheduleDetail
                    {
                        Id = x.Id,
                        DoctorScheduleId = x.DoctorScheduleId,
                        DayOfWeek = x.DayOfWeek,
                        WorkFrom = x.WorkFrom,
                        WorkTo = x.WorkTo,
                        Quota = x.Quota,
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? "" : x.Service.Name,
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

                    return (pagedItems.Adapt<List<DoctorScheduleDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DoctorScheduleDetailDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DoctorScheduleDetailDto> Handle(GetSingleDoctorScheduleDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DoctorScheduleDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DoctorScheduleDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DoctorScheduleDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new DoctorScheduleDetail
                    {
                        Id = x.Id,
                        DoctorScheduleId = x.DoctorScheduleId,
                        DayOfWeek = x.DayOfWeek,
                        WorkFrom = x.WorkFrom,
                        WorkTo = x.WorkTo,
                        Quota = x.Quota,
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? "" : x.Service.Name,
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DoctorScheduleDetailDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DoctorScheduleDetailDto> Handle(CreateDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleDetail>().AddAsync(request.DoctorScheduleDetailDto.Adapt<CreateUpdateDoctorScheduleDetailDto>().Adapt<DoctorScheduleDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DoctorScheduleDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DoctorScheduleDetailDto>> Handle(CreateListDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleDetail>().AddAsync(request.DoctorScheduleDetailDtos.Adapt<List<DoctorScheduleDetail>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DoctorScheduleDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DoctorScheduleDetailDto> Handle(UpdateDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleDetail>().UpdateAsync(request.DoctorScheduleDetailDto.Adapt<CreateUpdateDoctorScheduleDetailDto>().Adapt<DoctorScheduleDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DoctorScheduleDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DoctorScheduleDetailDto>> Handle(UpdateListDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DoctorScheduleDetail>().UpdateAsync(request.DoctorScheduleDetailDtos.Adapt<List<DoctorScheduleDetail>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DoctorScheduleDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDoctorScheduleDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DoctorScheduleDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDoctorScheduleDetailQuery_"); // Ganti dengan key yang sesuai

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