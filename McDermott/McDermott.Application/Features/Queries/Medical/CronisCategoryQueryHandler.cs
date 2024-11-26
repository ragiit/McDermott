using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Medical
{
    public class CronisCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetCronisCategoryQuery, (List<CronisCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleCronisCategoryQuery, CronisCategoryDto>,
        IRequestHandler<CreateCronisCategoryRequest, CronisCategoryDto>,
        IRequestHandler<BulkValidateCronisCategoryQuery, List<CronisCategoryDto>>,
        IRequestHandler<CreateListCronisCategoryRequest, List<CronisCategoryDto>>,
        IRequestHandler<UpdateCronisCategoryRequest, CronisCategoryDto>,
        IRequestHandler<UpdateListCronisCategoryRequest, List<CronisCategoryDto>>,
        IRequestHandler<DeleteCronisCategoryRequest, bool>
    {
        #region GET

        public async Task<List<CronisCategoryDto>> Handle(BulkValidateCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            var CronisCategoryDtos = request.CronisCategorysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var CronisCategoryNames = CronisCategoryDtos.Select(x => x.Name).Distinct().ToList();

            var existingCronisCategorys = await _unitOfWork.Repository<CronisCategory>()
                .Entities
                .AsNoTracking()
                .Where(v => CronisCategoryNames.Contains(v.Name))
                .ToListAsync(cancellationToken);

            return existingCronisCategorys.Adapt<List<CronisCategoryDto>>();
        }

        public async Task<(List<CronisCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<CronisCategory>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<CronisCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<CronisCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                       EF.Functions.Like(v.Description, $"%{request.SearchTerm}%")
                       );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new CronisCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<CronisCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<CronisCategoryDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<CronisCategoryDto> Handle(GetSingleCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<CronisCategory>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<CronisCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<CronisCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                       EF.Functions.Like(v.Description, $"%{request.SearchTerm}%")
                       );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new CronisCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<CronisCategoryDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<CronisCategory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<CronisCategoryDto> Handle(CreateCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<CronisCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CronisCategoryDto>> Handle(CreateListCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDtos.Adapt<List<CronisCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CronisCategoryDto> Handle(UpdateCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<CronisCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CronisCategoryDto>> Handle(UpdateListCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDtos.Adapt<List<CronisCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<CronisCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<CronisCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

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