using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceProductCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintenanceProductQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
    IRequestHandler<GetAllMaintenanceProductQuery, List<MaintenanceProductDto>>,
    IRequestHandler<GetMaintenanceProductQuery, (List<MaintenanceProductDto>, int pageIndex, int pageSize, int pageCount)>, //GoodsReceipt
    IRequestHandler<GetSingleMaintenanceProductQuery, MaintenanceProductDto>,
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
                            EF.Functions.Like(v.Product.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.SerialNumber, $"%{request.SearchTerm}%")
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
                        ProductId = x.ProductId,
                        Status = x.Status,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        MaintenanceId = x.MaintenanceId,
                        Maintenance = new Maintenance
                        {
                            Title = x.Maintenance == null ? string.Empty : x.Maintenance.Title,
                            RequestById = x.Maintenance.RequestById,
                            ResponsibleById = x.Maintenance.ResponsibleById,
                            ResponsibleBy = new User
                            {
                                Name = x.Maintenance.ResponsibleBy == null ? string.Empty : x.Maintenance.ResponsibleBy.Name
                            },
                            RequestBy = new User
                            {
                                Name = x.Maintenance.RequestBy == null ? string.Empty : x.Maintenance.RequestBy.Name
                            }
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
            catch (Exception ex)
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
                            EF.Functions.Like(v.Product.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.SerialNumber, $"%{request.SearchTerm}%")
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
                        ProductId = x.ProductId,
                        Status = x.Status,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        MaintenanceId = x.MaintenanceId,
                        Maintenance = new Maintenance
                        {
                            Title = x.Maintenance == null ? string.Empty : x.Maintenance.Title,
                            RequestById = x.Maintenance.RequestById,
                            ResponsibleById = x.Maintenance.ResponsibleById,
                            ResponsibleBy = new User
                            {
                                Name = x.Maintenance.ResponsibleBy == null ? string.Empty : x.Maintenance.ResponsibleBy.Name
                            },
                            RequestBy = new User
                            {
                                Name = x.Maintenance.RequestBy == null ? string.Empty : x.Maintenance.RequestBy.Name
                            }
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MaintenanceProductDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
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