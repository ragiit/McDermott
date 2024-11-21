using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.ProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class ProductQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetProductQuery, (List<ProductDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetProductQueryNew, (List<ProductDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleProductQueryNew, ProductDto>,
        IRequestHandler<GetAllProductQuery, List<ProductDto>>,
        IRequestHandler<ValidateProductQuery, bool>,
        IRequestHandler<CreateProductRequest, ProductDto>,
        IRequestHandler<CreateListProductRequest, List<ProductDto>>,
        IRequestHandler<UpdateProductRequest, ProductDto>,
        IRequestHandler<UpdateListProductRequest, List<ProductDto>>,
        IRequestHandler<DeleteProductRequest, bool>
    {
        #region GET

        public async Task<(List<ProductDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProductQueryNew request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Product>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Product>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Product>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new Product
                    {
                        Id = x.Id,
                        Name = x.Name,
                        InternalReference = x.InternalReference,
                        SalesPrice = x.SalesPrice,
                        TraceAbility = x.TraceAbility
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<ProductDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ProductDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ProductDto> Handle(GetSingleProductQueryNew request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Product>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Product>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Product>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new Product
                    {
                        Id = x.Id,
                        Name = x.Name,
                        InternalReference = x.InternalReference,
                        SalesPrice = x.SalesPrice
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ProductDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<List<ProductDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Product>? result))
                {
                    result = await _unitOfWork.Repository<Product>().Entities
                        .Include(x => x.ProductCategory)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<ProductDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Product>().Entities
                    .Include(x => x.Uom)
                    .Include(x => x.PurchaseUom)
                    .Include(x => x.BpjsClassification)
                    .AsNoTracking()
                    .AsQueryable();

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                var totalCount = await query.CountAsync(cancellationToken);

                var pagedResult = query
                            .OrderBy(x => x.Name);

                var skip = (request.PageIndex) * request.PageSize;

                var paged = pagedResult
                            .Skip(skip)
                            .Take(request.PageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

                return (paged.Adapt<List<ProductDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateProductQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Product>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<ProductDto> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().AddAsync(request.ProductDto.Adapt<Product>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductDto>> Handle(CreateListProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().AddAsync(request.ProductDtos.Adapt<List<Product>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ProductDto> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().UpdateAsync(request.ProductDto.Adapt<Product>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ProductDto>> Handle(UpdateListProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Product>().UpdateAsync(request.ProductDtos.Adapt<List<Product>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Product>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Product>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetProductQuery_"); // Ganti dengan key yang sesuai

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