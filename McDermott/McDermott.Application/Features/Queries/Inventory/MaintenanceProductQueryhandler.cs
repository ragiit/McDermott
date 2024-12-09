using McDermott.Application.Features.Commands;
using McDermott.Application.Features.Services;
using McDermott.Domain.Common;
using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintenanceProductQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetMaintenanceProductQuery, (List<MaintenanceProductDto>, int pageIndex, int pageSize, int pageCount)>, //Maintenance
     IRequestHandler<GetQueryMaintenanceProduct, IQueryable<MaintenanceProduct>>,
     IRequestHandler<GetSingleMaintenanceProductQuery, MaintenanceProductDto>,
     IRequestHandler<GetAllMaintenanceProductQuery, List<MaintenanceProductDto>>,
    IRequestHandler<ValidateMaintenanceProductQuery, bool>,
    IRequestHandler<CreateMaintenanceProductRequest, MaintenanceProductDto>,
    IRequestHandler<CreateListMaintenanceProductRequest, List<MaintenanceProductDto>>,
    IRequestHandler<UpdateMaintenanceProductRequest, MaintenanceProductDto>,
    IRequestHandler<UpdateListMaintenanceProductRequest, List<MaintenanceProductDto>>,
    IRequestHandler<DeleteMaintenanceProductRequest, bool>
    {
        #region GET

        public async Task<List<MaintenanceProductDto>> Handle(GetAllMaintenanceProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMaintenanceProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MaintenanceProduct>? result))
                {
                    result = await _unitOfWork.Repository<MaintenanceProduct>().Entities
                    .Include(x => x.Product)
                    .Include(x => x.Maintenance)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintenanceProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<MaintenanceProductDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintenanceProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaintenanceProduct>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<MaintenanceProduct>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<MaintenanceProduct>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.SerialNumber, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Product.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Note, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MaintenanceProduct
                    {
                        SerialNumber = x.SerialNumber,
                        Note = x.Note,
                        Expired = x.Expired,
                        Status = x.Status,
                        MaintenanceId = x.MaintenanceId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name
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

                    return (pagedItems.Adapt<List<MaintenanceProductDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<MaintenanceProductDto>>(), 0, 1, 1);
                }
            }
            catch (Exception)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<MaintenanceProductDto> Handle(GetSingleMaintenanceProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaintenanceProduct>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<MaintenanceProduct>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<MaintenanceProduct>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.SerialNumber, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Product.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Note, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MaintenanceProduct
                    {
                        Id = x.Id,
                        SerialNumber = x.SerialNumber,
                        Note = x.Note,
                        Expired = x.Expired,
                        Status = x.Status,
                        MaintenanceId = x.MaintenanceId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name
                        }

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MaintenanceProductDto>();
            }
            catch (Exception)
            {
                // Consider logging the exception
                throw;
            }
        }

        public Task<IQueryable<MaintenanceProduct>> Handle(GetQueryMaintenanceProduct request, CancellationToken cancellationToken)
        {
            return HandleQuery<MaintenanceProduct>(request, cancellationToken, request.Select is null ? x => new MaintenanceProduct
            {
                Id = x.Id,
                SerialNumber = x.SerialNumber,
                Note = x.Note,
                Expired = x.Expired,
                Status = x.Status,
                MaintenanceId = x.MaintenanceId,
                ProductId = x.ProductId,
                Product = new Product
                {
                    Name = x.Product == null ? string.Empty : x.Product.Name
                }

            } : request.Select);
        }
        private Task<IQueryable<TEntity>> HandleQuery<TEntity>(BaseQuery<TEntity> request, CancellationToken cancellationToken, Expression<Func<TEntity, TEntity>>? select = null)
    where TEntity : BaseAuditableEntity // Add the constraint here
        {
            try
            {
                var query = _unitOfWork.Repository<TEntity>().Entities.AsNoTracking();

                // Apply Predicate (filtering)
                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                // Apply Ordering
                if (request.OrderByList.Any())
                {
                    var firstOrderBy = request.OrderByList.First();
                    query = firstOrderBy.IsDescending
                        ? query.OrderByDescending(firstOrderBy.OrderBy)
                        : query.OrderBy(firstOrderBy.OrderBy);

                    foreach (var additionalOrderBy in request.OrderByList.Skip(1))
                    {
                        query = additionalOrderBy.IsDescending
                            ? ((IOrderedQueryable<TEntity>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TEntity>)query).ThenBy(additionalOrderBy.OrderBy);
                    }
                }

                // Apply Includes (eager loading)
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                // Apply Search Term
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = ApplySearchTerm(query, request.SearchTerm);
                }

                // Apply Select if provided, else return the entity as it is
                if (select is not null)
                    query = query.Select(select);

                return Task.FromResult(query.Adapt<IQueryable<TEntity>>());
            }
            catch (Exception)
            {
                // Return empty IQueryable<TEntity> if there's an exception
                return Task.FromResult(Enumerable.Empty<TEntity>().AsQueryable());
            }
        }

        private IQueryable<TEntity> ApplySearchTerm<TEntity>(IQueryable<TEntity> query, string searchTerm) where TEntity : class
        {
            // This method applies the search term based on the entity type
            if (typeof(TEntity) == typeof(MaintenanceProduct))
            {
                var MaintenanceProductQuery = query as IQueryable<MaintenanceProduct>;
                return (IQueryable<TEntity>)MaintenanceProductQuery.Where(v =>
                    EF.Functions.Like(v.Product.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.SerialNumber, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Expired.ToString(), $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Status.ToString(), $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Note, $"%{searchTerm}%"));
            }
            return query; // No filtering if the type doesn't match
        }

        public async Task<bool> Handle(ValidateMaintenanceProductQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<MaintenanceProduct>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<MaintenanceProductDto> Handle(CreateMaintenanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceProduct>().AddAsync(request.MaintenanceProductDto.Adapt<MaintenanceProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceProductDto>> Handle(CreateListMaintenanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceProduct>().AddAsync(request.MaintenanceProductDtos.Adapt<List<MaintenanceProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintenanceProductDto> Handle(UpdateMaintenanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceProduct>().UpdateAsync(request.MaintenanceProductDto.Adapt<MaintenanceProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceProductDto>> Handle(UpdateListMaintenanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceProduct>().UpdateAsync(request.MaintenanceProductDtos.Adapt<List<MaintenanceProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintenanceProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<MaintenanceProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MaintenanceProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceProductQuery_"); // Ganti dengan key yang sesuai

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