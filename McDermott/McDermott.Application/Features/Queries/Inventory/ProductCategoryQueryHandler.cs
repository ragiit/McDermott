using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.ProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class ProductCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProductCategoryQuery, (List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllProductCategoryQuery, List<ProductCategoryDto>>,
        IRequestHandler<ValidateProductCategoryQuery, bool>,
        IRequestHandler<BulkValidateProductCategoryQuery, List<ProductCategoryDto>>,
        IRequestHandler<CreateProductCategoryRequest, ProductCategoryDto>,
        IRequestHandler<CreateListProductCategoryRequest, List<ProductCategoryDto>>,
        IRequestHandler<UpdateProductCategoryRequest, ProductCategoryDto>,
        IRequestHandler<UpdateListProductCategoryRequest, List<ProductCategoryDto>>,
        IRequestHandler<DeleteProductCategoryRequest, bool>
    {
        #region GET

        public async Task<List<ProductCategoryDto>> Handle(BulkValidateProductCategoryQuery request, CancellationToken cancellationToken)
        {
            var ProductCategoryDtos = request.ProductCategorysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ProductCategoryNames = ProductCategoryDtos.Select(x => x.Name).Distinct().ToList();
            var a = ProductCategoryDtos.Select(x => x.Code).Distinct().ToList();

            var existingProductCategorys = await _unitOfWork.Repository<ProductCategory>()
                .Entities
                .AsNoTracking()
                .Where(v => ProductCategoryNames.Contains(v.Name) && a.Contains(v.Code))
                .ToListAsync(cancellationToken);

            return existingProductCategorys.Adapt<List<ProductCategoryDto>>();
        }

        public async Task<List<ProductCategoryDto>> Handle(GetAllProductCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetProductCategoryQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ProductCategory>? result))
                {
                    result = await _unitOfWork.Repository<ProductCategory>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProductCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<ProductCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProductCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ProductCategory>().Entities.AsNoTracking();

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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
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
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<ProductCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateProductCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ProductCategory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<ProductCategoryDto> Handle(CreateProductCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ProductCategory>().AddAsync(request.ProductCategoryDto.Adapt<ProductCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductCategoryQuery_");

                return result.Adapt<ProductCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductCategoryDto>> Handle(CreateListProductCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ProductCategory>().AddAsync(request.ProductCategoryDtos.Adapt<List<ProductCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProductCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProductCategoryDto> Handle(UpdateProductCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ProductCategory>().UpdateAsync(request.ProductCategoryDto.Adapt<ProductCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProductCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductCategoryDto>> Handle(UpdateListProductCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ProductCategory>().UpdateAsync(request.ProductCategoryDtos.Adapt<List<ProductCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProductCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProductCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ProductCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ProductCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductCategoryQuery_"); // Ganti dengan key yang sesuai

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