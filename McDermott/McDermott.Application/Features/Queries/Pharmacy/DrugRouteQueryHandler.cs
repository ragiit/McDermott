using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class DrugRouteQueryHandler
        (IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetDrugRouteQuery, (List<DrugRouteDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllDrugRouteQuery, List<DrugRouteDto>>,
        IRequestHandler<ValidateDrugRouteQuery, bool>,
        IRequestHandler<BulkValidateDrugRouteQuery, List<DrugRouteDto>>,
        IRequestHandler<CreateDrugRouteRequest, DrugRouteDto>,
        IRequestHandler<CreateListDrugRouteRequest, List<DrugRouteDto>>,
        IRequestHandler<UpdateDrugRouteRequest, DrugRouteDto>,
        IRequestHandler<UpdateListDrugRouteRequest, List<DrugRouteDto>>,
        IRequestHandler<DeleteDrugRouteRequest, bool>
    {
        #region GET

        public async Task<bool> Handle(ValidateDrugRouteQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<DrugRoute>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<List<DrugRouteDto>> Handle(BulkValidateDrugRouteQuery request, CancellationToken cancellationToken)
        {
            var DrugRouteDtos = request.DrugRoutesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var DrugRouteNames = DrugRouteDtos.Select(x => x.Route).Distinct().ToList();
            var a = DrugRouteDtos.Select(x => x.Code).Distinct().ToList();

            var existingDrugRoutes = await _unitOfWork.Repository<DrugRoute>()
                .Entities
                .AsNoTracking()
                .Where(v => DrugRouteNames.Contains(v.Route)
                            && a.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingDrugRoutes.Adapt<List<DrugRouteDto>>();
        }

        public async Task<List<DrugRouteDto>> Handle(GetAllDrugRouteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDrugRouteQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<DrugRoute>? result))
                {
                    result = await _unitOfWork.Repository<DrugRoute>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DrugRouteDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<DrugRouteDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetDrugRouteQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<DrugRoute>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Route, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Code, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Route), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<DrugRouteDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
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