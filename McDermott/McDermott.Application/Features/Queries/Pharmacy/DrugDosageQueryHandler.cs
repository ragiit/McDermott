using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class DrugDosageQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDrugDosageQuery, (List<DrugDosageDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleDrugDosageQuery, DrugDosageDto>,
        IRequestHandler<ValidateDrugDosageQuery, bool>,
        IRequestHandler<BulkValidateDrugDosageQuery, List<DrugDosageDto>>,
        IRequestHandler<CreateDrugDosageRequest, DrugDosageDto>,
        IRequestHandler<CreateListDrugDosageRequest, List<DrugDosageDto>>,
        IRequestHandler<UpdateDrugDosageRequest, DrugDosageDto>,
        IRequestHandler<UpdateListDrugDosageRequest, List<DrugDosageDto>>,
        IRequestHandler<DeleteDrugDosageRequest, bool>
    {
        #region GET

        public async Task<List<DrugDosageDto>> Handle(BulkValidateDrugDosageQuery request, CancellationToken cancellationToken)
        {
            var DrugDosageDtos = request.DrugDosagesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DrugDosageNames = DrugDosageDtos.Select(x => x.Frequency).Distinct().ToList();
            var a = DrugDosageDtos.Select(x => x.DrugRouteId).Distinct().ToList();
            var b = DrugDosageDtos.Select(x => x.TotalQtyPerDay).Distinct().ToList();
            var c = DrugDosageDtos.Select(x => x.Days).Distinct().ToList();

            var existingDrugDosages = await _unitOfWork.Repository<DrugDosage>()
                .Entities
                .AsNoTracking()
                .Where(v => DrugDosageNames.Contains(v.Frequency)
                            && a.Contains(v.DrugRouteId)
                            && c.Contains(v.Days)
                            && b.Contains(v.TotalQtyPerDay))
                .ToListAsync(cancellationToken);

            return existingDrugDosages.Adapt<List<DrugDosageDto>>();
        }

        public async Task<(List<DrugDosageDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDrugDosageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugDosage>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DrugDosage>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DrugDosage>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Frequency, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.DrugRoute.Route, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.TotalQtyPerDay.ToString(), $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Days.ToString(), $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DrugDosage
                    {
                        Id = x.Id,
                        Frequency = x.Frequency,
                        TotalQtyPerDay = x.TotalQtyPerDay,
                        Days = x.Days ,
                        DrugRouteId = x.DrugRouteId,
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty: x.DrugRoute.Route,
                        },
                        
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<DrugDosageDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<DrugDosageDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<DrugDosageDto> Handle(GetSingleDrugDosageQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugDosage>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<DrugDosage>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<DrugDosage>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Frequency, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.DrugRoute.Route, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.TotalQtyPerDay.ToString(), $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Days.ToString(), $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new DrugDosage
                    {
                        Id = x.Id,
                        Frequency = x.Frequency,
                        TotalQtyPerDay = x.TotalQtyPerDay,
                        Days = x.Days,
                        DrugRouteId = x.DrugRouteId,
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty : x.DrugRoute.Route,
                        },

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<DrugDosageDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateDrugDosageQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DrugDosage>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<DrugDosageDto> Handle(CreateDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().AddAsync(request.DrugDosageDto.Adapt<DrugDosage>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

                return result.Adapt<DrugDosageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(CreateListDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().AddAsync(request.DrugDosageDtos.Adapt<List<DrugDosage>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DrugDosageDto> Handle(UpdateDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().UpdateAsync(request.DrugDosageDto.Adapt<DrugDosage>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DrugDosageDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DrugDosageDto>> Handle(UpdateListDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DrugDosage>().UpdateAsync(request.DrugDosageDtos.Adapt<List<DrugDosage>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

                return result.Adapt<List<DrugDosageDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDrugDosageRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DrugDosage>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DrugDosage>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDrugDosageQuery_");

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