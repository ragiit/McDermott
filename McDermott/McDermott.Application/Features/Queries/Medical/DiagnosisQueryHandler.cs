using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Medical.DiagnosisCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DiagnosisQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDiagnosisQuery, (List<DiagnosisDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleDiagnosisQuery, DiagnosisDto>,
        IRequestHandler<CreateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<BulkValidateDiagnosisQuery, List<DiagnosisDto>>,
        IRequestHandler<CreateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<UpdateDiagnosisRequest, DiagnosisDto>,
        IRequestHandler<UpdateListDiagnosisRequest, List<DiagnosisDto>>,
        IRequestHandler<ValidateDiagnosisQuery, bool>,
        IRequestHandler<DeleteDiagnosisRequest, bool>
    {
        #region GET

        public async Task<List<DiagnosisDto>> Handle(BulkValidateDiagnosisQuery request, CancellationToken cancellationToken)
        {
            var DiagnosisDtos = request.DiagnosissToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DiagnosisNames = DiagnosisDtos.Select(x => x.Name).Distinct().ToList();
            var a = DiagnosisDtos.Select(x => x.Code).Distinct().ToList();
            var b = DiagnosisDtos.Select(x => x.NameInd).Distinct().ToList();
            var c = DiagnosisDtos.Select(x => x.CronisCategoryId).Distinct().ToList();

            var existingDiagnosiss = await _unitOfWork.Repository<Diagnosis>()
                .Entities
                .AsNoTracking()
                .Where(v => DiagnosisNames.Contains(v.Name)
                            && a.Contains(v.Code)
                            && b.Contains(v.NameInd)
                            && c.Contains(v.CronisCategoryId))
                .ToListAsync(cancellationToken);

            return existingDiagnosiss.Adapt<List<DiagnosisDto>>();
        }

        public async Task<(List<DiagnosisDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDiagnosisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Diagnosis>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Diagnosis>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Diagnosis>)query).ThenBy(additionalOrderBy.OrderBy);
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
                       EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                       EF.Functions.Like(v.NameInd, $"%{request.SearchTerm}%") ||
                       EF.Functions.Like(v.CronisCategory.Name, $"%{request.SearchTerm}%") ||
                       EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Diagnosis
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NameInd = x.NameInd,
                        Code = x.Code,
                        CronisCategoryId = x.CronisCategoryId,
                        CronisCategory = new CronisCategory
                        {
                            Name = x.CronisCategory == null ? "-" : x.CronisCategory.Name,
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

                    return (pagedItems.Adapt<List<DiagnosisDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DiagnosisDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DiagnosisDto> Handle(GetSingleDiagnosisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Diagnosis>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Diagnosis>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Diagnosis>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Diagnosis
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NameInd = x.NameInd,
                        Code = x.Code,
                        CronisCategoryId = x.CronisCategoryId,
                        CronisCategory = new CronisCategory
                        {
                            Name = x.CronisCategory == null ? "" : x.CronisCategory.Name,
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DiagnosisDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDiagnosisQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Diagnosis>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DiagnosisDto> Handle(CreateDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().AddAsync(request.DiagnosisDto.Adapt<CreateUpdateDiagnosisDto>().Adapt<Diagnosis>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiagnosisDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiagnosisDto>> Handle(CreateListDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().AddAsync(request.DiagnosisDtos.Adapt<List<Diagnosis>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiagnosisDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DiagnosisDto> Handle(UpdateDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().UpdateAsync(request.DiagnosisDto.Adapt<CreateUpdateDiagnosisDto>().Adapt<Diagnosis>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiagnosisDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiagnosisDto>> Handle(UpdateListDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Diagnosis>().UpdateAsync(request.DiagnosisDtos.Adapt<List<Diagnosis>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiagnosisDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDiagnosisRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Diagnosis>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Diagnosis>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiagnosisQuery_"); // Ganti dengan key yang sesuai

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