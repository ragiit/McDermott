using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class DrugRouteQueryHandler
        (IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDrugRouteQuery, (List<DrugRouteDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleDrugRouteQuery, DrugRouteDto>,
        IRequestHandler<ValidateDrugRouteQuery, bool>,
        IRequestHandler<BulkValidateDrugRouteQuery, List<DrugRouteDto>>,
        IRequestHandler<CreateDrugRouteRequest, DrugRouteDto>,
        IRequestHandler<CreateListDrugRouteRequest, List<DrugRouteDto>>,
        IRequestHandler<UpdateDrugRouteRequest, DrugRouteDto>,
        IRequestHandler<UpdateListDrugRouteRequest, List<DrugRouteDto>>,
        IRequestHandler<DeleteDrugRouteRequest, bool>
    {
        #region GET

        public async Task<List<DrugRouteDto>> Handle(BulkValidateDrugRouteQuery request, CancellationToken cancellationToken)
        {
            var DrugRouteDtos = request.DrugRoutesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DrugRouteNames = DrugRouteDtos.Select(x => x.Route).Distinct().ToList();

            var existingDrugRoutes = await _unitOfWork.Repository<DrugRoute>()
                .Entities
                .AsNoTracking()
                .Where(v => DrugRouteNames.Contains(v.Route))
                .ToListAsync(cancellationToken);

            return existingDrugRoutes.Adapt<List<DrugRouteDto>>();
        }

        public async Task<(List<DrugRouteDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDrugRouteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugRoute>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DrugRoute>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DrugRoute>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Route, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DrugRoute
                    {
                        Id = x.Id,
                        Route = x.Route,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<DrugRouteDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DrugRouteDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DrugRouteDto> Handle(GetSingleDrugRouteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugRoute>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DrugRoute>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DrugRoute>)query).ThenBy(additionalOrderBy.OrderBy);
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
                         EF.Functions.Like(v.Route, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DrugRoute
                    {
                        Id = x.Id,
                        Route = x.Route,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DrugRouteDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDrugRouteQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DrugRoute>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DrugRouteDto> Handle(CreateDrugRouteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugRoute>().AddAsync(request.DrugRouteDto.Adapt<DrugRoute>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugRouteQuery_");

                return result.Adapt<DrugRouteDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugRouteDto>> Handle(CreateListDrugRouteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugRoute>().AddAsync(request.DrugRouteDtos.Adapt<List<DrugRoute>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugRouteQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugRouteDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DrugRouteDto> Handle(UpdateDrugRouteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugRoute>().UpdateAsync(request.DrugRouteDto.Adapt<DrugRoute>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugRouteQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugRouteDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugRouteDto>> Handle(UpdateListDrugRouteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugRoute>().UpdateAsync(request.DrugRouteDtos.Adapt<List<DrugRoute>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugRouteQuery_");

                return result.Adapt<List<DrugRouteDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDrugRouteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DrugRoute>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DrugRoute>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugRouteQuery_");

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