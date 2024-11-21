using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class TransferStockQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
       IRequestHandler<GetTransferStockQuery, (List<TransferStockDto>, int pageIndex, int pageSize, int pageCount)>, //TransferStock
        IRequestHandler<GetSingleTransferStockQuery, TransferStockDto>, IRequestHandler<ValidateTransferStockQuery, bool>,
        IRequestHandler<BulkValidateTransferStockQuery, List<TransferStockDto>>,
        IRequestHandler<CreateTransferStockRequest, TransferStockDto>,
        IRequestHandler<CreateListTransferStockRequest, List<TransferStockDto>>,
        IRequestHandler<UpdateTransferStockRequest, TransferStockDto>,
        IRequestHandler<UpdateListTransferStockRequest, List<TransferStockDto>>,
        IRequestHandler<DeleteTransferStockRequest, bool>,
        IRequestHandler<GetAllTransferStockProductQuery, List<TransferStockProductDto>>,//TransferStockProduct
        IRequestHandler<GetTransferStockProductQuery, (List<TransferStockProductDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleTransferStockProductQuery, TransferStockProductDto>, IRequestHandler<ValidateTransferStockProductQuery, bool>,
        IRequestHandler<BulkValidateTransferStockProductQuery, List<TransferStockProductDto>>, IRequestHandler<CreateTransferStockProductRequest, TransferStockProductDto>,
        IRequestHandler<CreateListTransferStockProductRequest, List<TransferStockProductDto>>,
        IRequestHandler<UpdateTransferStockProductRequest, TransferStockProductDto>,
        IRequestHandler<UpdateListTransferStockProductRequest, List<TransferStockProductDto>>,
        IRequestHandler<DeleteTransferStockProductRequest, bool>,
        IRequestHandler<GetAllTransferStockLogQuery, List<TransferStockLogDto>>,//TransferStockLog
        IRequestHandler<GetTransferStockLogQuery, (List<TransferStockLogDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleTransferStockLogQuery, TransferStockLogDto>, IRequestHandler<ValidateTransferStockLogQuery, bool>,
        IRequestHandler<BulkValidateTransferStockLogQuery, List<TransferStockLogDto>>,
        IRequestHandler<CreateTransferStockLogRequest, TransferStockLogDto>,
        IRequestHandler<CreateListTransferStockLogRequest, List<TransferStockLogDto>>,
        IRequestHandler<UpdateTransferStockLogRequest, TransferStockLogDto>,
        IRequestHandler<UpdateListTransferStockLogRequest, List<TransferStockLogDto>>,
        IRequestHandler<DeleteTransferStockLogRequest, bool>

    {
        #region GET Transfer Stock

        public async Task<List<TransferStockDto>> Handle(BulkValidateTransferStockQuery request, CancellationToken cancellationToken)
        {
            var TransferStockDtos = request.TransferStockToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var TransferStockNames = TransferStockDtos.Select(x => x.DestinationId).Distinct().ToList();

            var existingTransferStocks = await _unitOfWork.Repository<TransferStock>()
                .Entities
                .AsNoTracking()
                .Where(v => TransferStockNames.Contains(v.DestinationId))
                .ToListAsync(cancellationToken);

            return existingTransferStocks.Adapt<List<TransferStockDto>>();
        }

        public async Task<bool> Handle(ValidateTransferStockQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<TransferStock>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<TransferStockDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetTransferStockQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<TransferStock>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<TransferStock>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TransferStock>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.KodeTransaksi, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new TransferStock
                    {
                        Id = x.Id,
                        KodeTransaksi = x.KodeTransaksi,
                        Reference = x.Reference,
                        SchenduleDate = x.SchenduleDate,
                        Status = x.Status,
                        StockRequest = x.StockRequest,
                        SourceId = x.SourceId,
                        Source = new Locations
                        {
                            Name = x.Source == null ? string.Empty : x.Source.Name,
                        },
                        DestinationId = x.DestinationId,
                        Destination = new Locations
                        {
                            Name = x.Destination == null ? string.Empty : x.Destination.Name
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

                    return (pagedItems.Adapt<List<TransferStockDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<TransferStockDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<TransferStockDto> Handle(GetSingleTransferStockQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<TransferStock>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<TransferStock>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TransferStock>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.KodeTransaksi, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new TransferStock
                    {
                        Id = x.Id,
                        KodeTransaksi = x.KodeTransaksi,
                        Reference = x.Reference,
                        SchenduleDate = x.SchenduleDate,
                        Status = x.Status,
                        StockRequest = x.StockRequest,
                        SourceId = x.SourceId,
                        Source = new Locations
                        {
                            Name = x.Source == null ? string.Empty : x.Source.Name,
                        },
                        DestinationId = x.DestinationId,
                        Destination = new Locations
                        {
                            Name = x.Destination == null ? string.Empty : x.Destination.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<TransferStockDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Transfer Stock

        #region GET Transfer StockDetail

        public async Task<List<TransferStockProductDto>> Handle(GetAllTransferStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllTransferStockProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransferStockProduct>? result))
                {
                    result = await _unitOfWork.Repository<TransferStockProduct>().Entities
                        .Include(x => x.TransferStock)
                        .Include(x => x.Product)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransferStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockProductDto>> Handle(BulkValidateTransferStockProductQuery request, CancellationToken cancellationToken)
        {
            var TransferStockProductDtos = request.TransferStockProductToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var TransferStockProductNames = TransferStockProductDtos.Select(x => x.Batch).Distinct().ToList();
            var a = TransferStockProductDtos.Select(x => x.ProductId).Distinct().ToList();

            var existingTransferStockProducts = await _unitOfWork.Repository<TransferStockProduct>()
                .Entities
                .AsNoTracking()
                .Where(v => TransferStockProductNames.Contains(v.Batch)
                            && a.Contains(v.ProductId))
                .ToListAsync(cancellationToken);

            return existingTransferStockProducts.Adapt<List<TransferStockProductDto>>();
        }

        public async Task<bool> Handle(ValidateTransferStockProductQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<TransferStockProduct>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<TransferStockProductDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetTransferStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<TransferStockProduct>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<TransferStockProduct>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TransferStockProduct>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Batch, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Product.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new TransferStockProduct
                    {
                        Id = x.Id,
                        Batch = x.Batch,
                        QtyStock = x.QtyStock,
                        ExpiredDate = x.ExpiredDate,
                        TransferStockId = x.TransferStockId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                            UomId = x.Product.UomId,
                            Uom = new Uom
                            {
                                Name = x.Product.Uom == null ? string.Empty : x.Product.Uom.Name
                            },
                            PurchaseUomId = x.Product.PurchaseUomId,
                            PurchaseUom = new Uom
                            {
                                Name = x.Product.PurchaseUom == null ? string.Empty : x.Product.PurchaseUom.Name
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

                    return (pagedItems.Adapt<List<TransferStockProductDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<TransferStockProductDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<TransferStockProductDto> Handle(GetSingleTransferStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<TransferStockProduct>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<TransferStockProduct>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TransferStockProduct>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Batch, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Product.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new TransferStockProduct
                    {
                        Id = x.Id,
                        Batch = x.Batch,
                        QtyStock = x.QtyStock,
                        ExpiredDate = x.ExpiredDate,
                        TransferStockId = x.TransferStockId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                            UomId = x.Product.UomId,
                            Uom = new Uom
                            {
                                Name = x.Product.Uom == null ? string.Empty : x.Product.Uom.Name
                            },
                            PurchaseUomId = x.Product.PurchaseUomId,
                            PurchaseUom = new Uom
                            {
                                Name = x.Product.PurchaseUom == null ? string.Empty : x.Product.PurchaseUom.Name
                            }
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<TransferStockProductDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Transfer StockDetail

        #region GET Transfer StockLog

        public async Task<List<TransferStockLogDto>> Handle(GetAllTransferStockLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllTransferStockLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransferStockLog>? result))
                {
                    result = await _unitOfWork.Repository<TransferStockLog>().Entities
                        .Include(x => x.TransferStock)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransferStockLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockLogDto>> Handle(BulkValidateTransferStockLogQuery request, CancellationToken cancellationToken)
        {
            var TransferStockLogDtos = request.TransferStockLogToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var TransferStockLogNames = TransferStockLogDtos.Select(x => x.TransferStockId).Distinct().ToList();
            var a = TransferStockLogDtos.Select(x => x.DestinationId).Distinct().ToList();

            var existingTransferStockLogs = await _unitOfWork.Repository<TransferStockLog>()
                .Entities
                .AsNoTracking()
                .Where(v => TransferStockLogNames.Contains(v.TransferStockId)
                            && a.Contains(v.DestinationId))
                .ToListAsync(cancellationToken);

            return existingTransferStockLogs.Adapt<List<TransferStockLogDto>>();
        }

        public async Task<bool> Handle(ValidateTransferStockLogQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<TransferStockLog>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<TransferStockLogDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetTransferStockLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<TransferStockLog>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<TransferStockLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TransferStockLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TransferStock.KodeTransaksi, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new TransferStockLog
                    {
                        Id = x.Id,
                        TransferStockId = x.TransferStockId,
                        UserById = x.UserById,
                        Status = x.Status,
                        UserBy = new User
                        {
                            Name = x.UserBy == null ? string.Empty : x.UserBy.Name
                        },
                        DestinationId = x.DestinationId,
                        Destination = new Locations
                        {
                            Name = x.Destination == null ? string.Empty : x.Destination.Name
                        },
                        SourceId = x.SourceId,
                        Source = new Locations
                        {
                            Name = x.Source == null ? string.Empty : x.Source.Name
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

                    return (pagedItems.Adapt<List<TransferStockLogDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<TransferStockLogDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<TransferStockLogDto> Handle(GetSingleTransferStockLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<TransferStockLog>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<TransferStockLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<TransferStockLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TransferStock.KodeTransaksi, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new TransferStockLog
                    {
                        Id = x.Id,
                        TransferStockId = x.TransferStockId,
                        UserById = x.UserById,
                        Status = x.Status,
                        UserBy = new User
                        {
                            Name = x.UserBy == null ? string.Empty : x.UserBy.Name
                        },
                        DestinationId = x.DestinationId,
                        Destination = new Locations
                        {
                            Name = x.Destination == null ? string.Empty : x.Destination.Name
                        },
                        SourceId = x.SourceId,
                        Source = new Locations
                        {
                            Name = x.Source == null ? string.Empty : x.Source.Name
                        },
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<TransferStockLogDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Transfer StockLog

        #region CREATE

        public async Task<TransferStockDto> Handle(CreateTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().AddAsync(request.TransferStockDto.Adapt<TransferStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockDto>> Handle(CreateListTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().AddAsync(request.TransferStockDtos.Adapt<List<TransferStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region CREATE Product

        public async Task<TransferStockProductDto> Handle(CreateTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().AddAsync(request.TransferStockProductDto.Adapt<TransferStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockProductDto>> Handle(CreateListTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().AddAsync(request.TransferStockProductDtos.Adapt<List<TransferStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Product

        #region CREATE Detail

        public async Task<TransferStockLogDto> Handle(CreateTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().AddAsync(request.TransferStockLogDto.Adapt<TransferStockLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockLogDto>> Handle(CreateListTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().AddAsync(request.TransferStockLogDtos.Adapt<List<TransferStockLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Detail

        #region UPDATE

        public async Task<TransferStockDto> Handle(UpdateTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().UpdateAsync(request.TransferStockDto.Adapt<TransferStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockDto>> Handle(UpdateListTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().UpdateAsync(request.TransferStockDtos.Adapt<List<TransferStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region UPDATE Product

        public async Task<TransferStockProductDto> Handle(UpdateTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().UpdateAsync(request.TransferStockProductDto.Adapt<TransferStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockProductDto>> Handle(UpdateListTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().UpdateAsync(request.TransferStockProductDtos.Adapt<List<TransferStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Product

        #region UPDATE Detail

        public async Task<TransferStockLogDto> Handle(UpdateTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().UpdateAsync(request.TransferStockLogDto.Adapt<TransferStockLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockLogDto>> Handle(UpdateListTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().UpdateAsync(request.TransferStockLogDtos.Adapt<List<TransferStockLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Detail

        #region DELETE

        public async Task<bool> Handle(DeleteTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransferStock>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransferStock>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #region DELETE Product

        public async Task<bool> Handle(DeleteTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransferStockProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransferStockProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Product

        #region DELETE Detail

        public async Task<bool> Handle(DeleteTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransferStockLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransferStockLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Detail
    }
}