using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramAttendanceCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class WellnessProgramAttendanceHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
      IRequestHandler<GetWellnessProgramAttendanceQuery, (List<WellnessProgramAttendanceDto>, int pageIndex, int pageSize, int pageCount)>,
      IRequestHandler<GetSingleWellnessProgramAttendanceQuery, WellnessProgramAttendanceDto>,
        IRequestHandler<ValidateWellnessProgramAttendance, bool>,
      IRequestHandler<CreateWellnessProgramAttendanceRequest, WellnessProgramAttendanceDto>,
      IRequestHandler<BulkValidateWellnessProgramAttendance, List<WellnessProgramAttendanceDto>>,
      IRequestHandler<CreateListWellnessProgramAttendanceRequest, List<WellnessProgramAttendanceDto>>,
      IRequestHandler<UpdateWellnessProgramAttendanceRequest, WellnessProgramAttendanceDto>,
      IRequestHandler<UpdateListWellnessProgramAttendanceRequest, List<WellnessProgramAttendanceDto>>,
      IRequestHandler<DeleteWellnessProgramAttendanceRequest, bool>
    {
        #region GET

        public async Task<bool> Handle(ValidateWellnessProgramAttendance request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<WellnessProgramAttendance>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<List<WellnessProgramAttendanceDto>> Handle(BulkValidateWellnessProgramAttendance request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.WellnessProgramAttendancesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
            //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

            //var existingCountrys = await _unitOfWork.Repository<Country>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
            //    .ToListAsync(cancellationToken);

            //return existingCountrys.Adapt<List<CountryDto>>();

            return [];
        }

        public async Task<(List<WellnessProgramAttendanceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramAttendanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramAttendance>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramAttendance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramAttendance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //        EF.Functions.Like(v.WellnessProgramAttendance.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramAttendance
                    {
                        Id = x.Id,
                        WellnessProgramDetailId = x.WellnessProgramDetailId,
                        WellnessProgramDetail = new WellnessProgramDetail
                        {
                            Name = x.WellnessProgramDetail != null ? x.WellnessProgramDetail.Name : ""
                        },
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient == null ? "" : x.Patient.Name,
                            Email = x.Patient == null ? "" : x.Patient.Email,
                            Department = x.Patient != null && x.Patient.Department == null ? new Department() : new Department
                            {
                                Name = x.Patient != null && x.Patient.Department != null ? x.Patient.Department.Name : ""
                            }
                        },
                        Date = x.Date,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<WellnessProgramAttendanceDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramAttendanceDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<WellnessProgramAttendanceDto> Handle(GetSingleWellnessProgramAttendanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramAttendance>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramAttendance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramAttendance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.WellnessProgramAttendance.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramAttendance
                    {
                        Id = x.Id,
                        WellnessProgramDetailId = x.WellnessProgramDetailId,
                        PatientId = x.PatientId,
                        Date = x.Date,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramAttendanceDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<WellnessProgramAttendanceDto> Handle(CreateWellnessProgramAttendanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramAttendance>().AddAsync(request.WellnessProgramAttendanceDto.Adapt<CreateUpdateWellnessProgramAttendanceDto>().Adapt<WellnessProgramAttendance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramAttendanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramAttendanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramAttendanceDto>> Handle(CreateListWellnessProgramAttendanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramAttendance>().AddAsync(request.WellnessProgramAttendanceDtos.Adapt<List<WellnessProgramAttendance>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramAttendanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramAttendanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<WellnessProgramAttendanceDto> Handle(UpdateWellnessProgramAttendanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramAttendance>().UpdateAsync(request.WellnessProgramAttendanceDto.Adapt<WellnessProgramAttendanceDto>().Adapt<WellnessProgramAttendance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramAttendanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramAttendanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramAttendanceDto>> Handle(UpdateListWellnessProgramAttendanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramAttendance>().UpdateAsync(request.WellnessProgramAttendanceDtos.Adapt<List<WellnessProgramAttendance>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramAttendanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramAttendanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteWellnessProgramAttendanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramAttendance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramAttendance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramAttendanceQuery_"); // Ganti dengan key yang sesuai

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