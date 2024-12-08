using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Medical
{
    public class NursingDiagnosesQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetNursingDiagnosesQuery, (List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)>,
         IRequestHandler<GetSingleNursingDiagnosesQuery, NursingDiagnosesDto>,
        IRequestHandler<CreateNursingDiagnosesRequest, NursingDiagnosesDto>,
        IRequestHandler<BulkValidateNursingDiagnosesQuery, List<NursingDiagnosesDto>>,
        IRequestHandler<CreateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<UpdateNursingDiagnosesRequest, NursingDiagnosesDto>,
        IRequestHandler<UpdateListNursingDiagnosesRequest, List<NursingDiagnosesDto>>,
        IRequestHandler<DeleteNursingDiagnosesRequest, bool>
    {
        #region GET

        public async Task<List<NursingDiagnosesDto>> Handle(BulkValidateNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            var NursingDiagnosesDtos = request.NursingDiagnosessToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var NursingDiagnosesNames = NursingDiagnosesDtos.Select(x => x.Problem).Distinct().ToList();
            var A = NursingDiagnosesDtos.Select(x => x.Code).Distinct().ToList();

            var existingNursingDiagnosess = await _unitOfWork.Repository<NursingDiagnoses>()
                .Entities
                .AsNoTracking()
                .Where(v => NursingDiagnosesNames.Contains(v.Problem)
                            && A.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingNursingDiagnosess.Adapt<List<NursingDiagnosesDto>>();
        }

        public async Task<(List<NursingDiagnosesDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<NursingDiagnoses>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<NursingDiagnoses>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<NursingDiagnoses>)query).ThenBy(additionalOrderBy.OrderBy);
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
                         EF.Functions.Like(v.Problem, $"%{request.SearchTerm}%") ||
                         EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new NursingDiagnoses
                    {
                        Id = x.Id,
                        Problem = x.Problem,
                        Code = x.Code
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<NursingDiagnosesDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<NursingDiagnosesDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<NursingDiagnosesDto> Handle(GetSingleNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<NursingDiagnoses>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<NursingDiagnoses>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<NursingDiagnoses>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Problem, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new NursingDiagnoses
                    {
                        Id = x.Id,
                        Problem = x.Problem,
                        Code = x.Code
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<NursingDiagnosesDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateNursingDiagnosesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<NursingDiagnoses>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<NursingDiagnosesDto> Handle(CreateNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnoses>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<NursingDiagnosesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NursingDiagnosesDto>> Handle(CreateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().AddAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<NursingDiagnosesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<NursingDiagnosesDto> Handle(UpdateNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDto.Adapt<NursingDiagnoses>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<NursingDiagnosesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NursingDiagnosesDto>> Handle(UpdateListNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<NursingDiagnoses>().UpdateAsync(request.NursingDiagnosesDtos.Adapt<List<NursingDiagnoses>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<NursingDiagnosesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteNursingDiagnosesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<NursingDiagnoses>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetNursingDiagnosesQuery_"); // Ganti dengan key yang sesuai

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