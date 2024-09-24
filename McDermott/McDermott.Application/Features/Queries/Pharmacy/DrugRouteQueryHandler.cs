
namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class DrugRouteQueryHandler
        (IUnitOfWork _unitOfWork, IMemoryCache _cache) :
         IRequestHandler<GetDrugRouteQuery, (List<DrugRouteDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllDrugRouteQuery, List<DrugRouteDto>>,
        IRequestHandler<ValidateDrugRouteQuery, bool>,
        IRequestHandler<CreateDrugRouteRequest, DrugRouteDto>,
        IRequestHandler<CreateListDrugRouteRequest, List<DrugRouteDto>>,
        IRequestHandler<UpdateDrugRouteRequest, DrugRouteDto>,
        IRequestHandler<UpdateListDrugRouteRequest, List<DrugRouteDto>>,
        IRequestHandler<DeleteDrugRouteRequest, bool>
    {
        #region GET

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
                var query = _unitOfWork.Repository<DrugRoute>().Entities
                    
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Route, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Route);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<DrugRouteDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
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
