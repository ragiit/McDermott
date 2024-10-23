using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.GoodsReceiptCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class GoodsReceiptQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetGoodsReceiptQuery, (List<GoodsReceiptDto>, int pageIndex, int pageSize, int pageCount)>, //GoodsReceipt
        IRequestHandler<GetSingleGoodsReceiptQuery, GoodsReceiptDto>, IRequestHandler<ValidateGoodsReceiptQuery, bool>,
        IRequestHandler<BulkValidateGoodsReceiptQuery, List<GoodsReceiptDto>>,
        IRequestHandler<CreateGoodsReceiptRequest, GoodsReceiptDto>,
        IRequestHandler<CreateListGoodsReceiptRequest, List<GoodsReceiptDto>>,
        IRequestHandler<UpdateGoodsReceiptRequest, GoodsReceiptDto>,
        IRequestHandler<UpdateListGoodsReceiptRequest, List<GoodsReceiptDto>>,
        IRequestHandler<DeleteGoodsReceiptRequest, bool>,
        IRequestHandler<GetAllGoodsReceiptDetailQuery, List<GoodsReceiptDetailDto>>,//GoodsReceiptDetail
        IRequestHandler<GetGoodsReceiptDetailQuery, (List<GoodsReceiptDetailDto>, int pageIndex, int pageSize, int pageCount)>, 
        IRequestHandler<GetSingleGoodsReceiptDetailQuery, GoodsReceiptDetailDto>, IRequestHandler<ValidateGoodsReceiptDetailQuery, bool>,
        IRequestHandler<BulkValidateGoodsReceiptDetailQuery, List<GoodsReceiptDetailDto>>,
        IRequestHandler<CreateGoodsReceiptDetailRequest, GoodsReceiptDetailDto>,
        IRequestHandler<CreateListGoodsReceiptDetailRequest, List<GoodsReceiptDetailDto>>,
        IRequestHandler<UpdateGoodsReceiptDetailRequest, GoodsReceiptDetailDto>,
        IRequestHandler<UpdateListGoodsReceiptDetailRequest, List<GoodsReceiptDetailDto>>,
        IRequestHandler<DeleteGoodsReceiptDetailRequest, bool>,
        IRequestHandler<GetAllGoodsReceiptLogQuery, List<GoodsReceiptLogDto>>,//GoodsReceiptLog
        IRequestHandler<GetGoodsReceiptLogQuery, (List<GoodsReceiptLogDto>, int pageIndex, int pageSize, int pageCount)>,  
        IRequestHandler<GetSingleGoodsReceiptLogQuery, GoodsReceiptLogDto>, IRequestHandler<ValidateGoodsReceiptLogQuery, bool>,
        IRequestHandler<BulkValidateGoodsReceiptLogQuery, List<GoodsReceiptLogDto>>, 
        IRequestHandler<CreateGoodsReceiptLogRequest, GoodsReceiptLogDto>,
        IRequestHandler<CreateListGoodsReceiptLogRequest, List<GoodsReceiptLogDto>>,
        IRequestHandler<UpdateGoodsReceiptLogRequest, GoodsReceiptLogDto>,
        IRequestHandler<UpdateListGoodsReceiptLogRequest, List<GoodsReceiptLogDto>>,
        IRequestHandler<DeleteGoodsReceiptLogRequest, bool>
    {

        #region GET Goods Receipt 
        public async Task<List<GoodsReceiptDto>> Handle(BulkValidateGoodsReceiptQuery request, CancellationToken cancellationToken)
        {
            var GoodsReceiptDtos = request.GoodsReceiptToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var GoodsReceiptNames = GoodsReceiptDtos.Select(x => x.DestinationId).Distinct().ToList();

            var existingGoodsReceipts = await _unitOfWork.Repository<GoodsReceipt>()
                .Entities
                .AsNoTracking()
                .Where(v => GoodsReceiptNames.Contains(v.DestinationId))
                .ToListAsync(cancellationToken);

            return existingGoodsReceipts.Adapt<List<GoodsReceiptDto>>();
        }

        public async Task<bool> Handle(ValidateGoodsReceiptQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GoodsReceipt>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GoodsReceiptDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGoodsReceiptQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GoodsReceipt>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GoodsReceipt>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GoodsReceipt>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.ReceiptNumber, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GoodsReceipt
                    {
                        Id = x.Id,
                        ReceiptNumber = x.ReceiptNumber,
                        NumberPurchase = x.NumberPurchase,
                        SchenduleDate = x.SchenduleDate,
                        SourceId = x.SourceId,
                        Status = x.Status,
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

                    return (pagedItems.Adapt<List<GoodsReceiptDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GoodsReceiptDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GoodsReceiptDto> Handle(GetSingleGoodsReceiptQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GoodsReceipt>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GoodsReceipt>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GoodsReceipt>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.ReceiptNumber, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GoodsReceipt
                    {
                        Id = x.Id,
                        ReceiptNumber = x.ReceiptNumber,
                        NumberPurchase = x.NumberPurchase,
                        SchenduleDate = x.SchenduleDate,
                        SourceId = x.SourceId,
                        Status=x.Status,
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

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GoodsReceiptDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }
        #endregion GET Goods Receipt

        #region GET Goods Receipt Detail

        public async Task<List<GoodsReceiptDetailDto>> Handle(GetAllGoodsReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllGoodsReceiptDetailQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GoodsReceiptDetail>? result))
                {
                    result = await _unitOfWork.Repository<GoodsReceiptDetail>().Entities
                        .Include(x => x.GoodsReceipt)
                        .Include(x=>x.Product)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GoodsReceiptDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptDetailDto>> Handle(BulkValidateGoodsReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            var GoodsReceiptDetailDtos = request.GoodsReceiptDetailToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var GoodsReceiptDetailNames = GoodsReceiptDetailDtos.Select(x => x.Batch).Distinct().ToList();
            var a = GoodsReceiptDetailDtos.Select(x => x.ProductId).Distinct().ToList();

            var existingGoodsReceiptDetails = await _unitOfWork.Repository<GoodsReceiptDetail>()
                .Entities
                .AsNoTracking()
                .Where(v => GoodsReceiptDetailNames.Contains(v.Batch)
                            && a.Contains(v.ProductId))
                .ToListAsync(cancellationToken);

            return existingGoodsReceiptDetails.Adapt<List<GoodsReceiptDetailDto>>();
        }

        public async Task<bool> Handle(ValidateGoodsReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GoodsReceiptDetail>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GoodsReceiptDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGoodsReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GoodsReceiptDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GoodsReceiptDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GoodsReceiptDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new GoodsReceiptDetail
                    {
                        Id = x.Id,
                        Batch = x.Batch,
                        Qty = x.Qty,
                        ExpiredDate = x.ExpiredDate,
                        GoodsReceiptId = x.GoodsReceiptId,
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

                    return (pagedItems.Adapt<List<GoodsReceiptDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GoodsReceiptDetailDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GoodsReceiptDetailDto> Handle(GetSingleGoodsReceiptDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GoodsReceiptDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GoodsReceiptDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GoodsReceiptDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new GoodsReceiptDetail
                    {
                        Id = x.Id,
                        Batch = x.Batch,
                        Qty = x.Qty,
                        ExpiredDate = x.ExpiredDate,
                        GoodsReceiptId = x.GoodsReceiptId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GoodsReceiptDetailDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Goods Receipt Detail

        #region GET Goods Receipt Log

        public async Task<List<GoodsReceiptLogDto>> Handle(GetAllGoodsReceiptLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllGoodsReceiptLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<GoodsReceiptLog>? result))
                {
                    result = await _unitOfWork.Repository<GoodsReceiptLog>().Entities
                        .Include(x => x.GoodsReceipt)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<GoodsReceiptLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<GoodsReceiptLogDto>> Handle(BulkValidateGoodsReceiptLogQuery request, CancellationToken cancellationToken)
        {
            var GoodsReceiptLogDtos = request.GoodsReceiptLogToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var GoodsReceiptLogNames = GoodsReceiptLogDtos.Select(x => x.GoodsReceiptId).Distinct().ToList();
            var a = GoodsReceiptLogDtos.Select(x => x.DestinationId).Distinct().ToList();

            var existingGoodsReceiptLogs = await _unitOfWork.Repository<GoodsReceiptLog>()
                .Entities
                .AsNoTracking()
                .Where(v => GoodsReceiptLogNames.Contains(v.GoodsReceiptId)
                            && a.Contains(v.DestinationId))
                .ToListAsync(cancellationToken);

            return existingGoodsReceiptLogs.Adapt<List<GoodsReceiptLogDto>>();
        }

        public async Task<bool> Handle(ValidateGoodsReceiptLogQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<GoodsReceiptLog>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<GoodsReceiptLogDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetGoodsReceiptLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GoodsReceiptLog>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GoodsReceiptLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GoodsReceiptLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.GoodsReceipt.ReceiptNumber, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GoodsReceiptLog
                    {
                        Id = x.Id,
                        GoodsReceiptId = x.GoodsReceiptId,
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

                    return (pagedItems.Adapt<List<GoodsReceiptLogDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<GoodsReceiptLogDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<GoodsReceiptLogDto> Handle(GetSingleGoodsReceiptLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<GoodsReceiptLog>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<GoodsReceiptLog>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<GoodsReceiptLog>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.GoodsReceipt.ReceiptNumber, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Destination.Name, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new GoodsReceiptLog
                    {
                        Id = x.Id,
                        GoodsReceiptId = x.GoodsReceiptId,
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

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<GoodsReceiptLogDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Goods Receipt Log

        #region CREATE Goods Receipt Detail

        public async Task<GoodsReceiptDetailDto> Handle(CreateGoodsReceiptDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptDetail>().AddAsync(request.GoodsReceiptDetailDto.Adapt<GoodsReceiptDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptDetailQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<GoodsReceiptDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptDetailDto>> Handle(CreateListGoodsReceiptDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptDetail>().AddAsync(request.GoodsReceiptDetailDtos.Adapt<List<GoodsReceiptDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GoodsReceiptDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Goods Receipt Detail

        #region CREATE Goods Receipt 

        public async Task<GoodsReceiptDto> Handle(CreateGoodsReceiptRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceipt>().AddAsync(request.GoodsReceiptDto.Adapt<GoodsReceipt>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GoodsReceiptDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptDto>> Handle(CreateListGoodsReceiptRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceipt>().AddAsync(request.GoodsReceiptDtos.Adapt<List<GoodsReceipt>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GoodsReceiptDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Goods Receipt 

        #region CREATE Goods Receipt Log

        public async Task<GoodsReceiptLogDto> Handle(CreateGoodsReceiptLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptLog>().AddAsync(request.GoodsReceiptLogDto.Adapt<GoodsReceiptLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GoodsReceiptLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptLogDto>> Handle(CreateListGoodsReceiptLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptLog>().AddAsync(request.GoodsReceiptLogDtos.Adapt<List<GoodsReceiptLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GoodsReceiptLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Goods Receipt 

        #region UPDATE Goods Receipt Detail

        public async Task<GoodsReceiptDetailDto> Handle(UpdateGoodsReceiptDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptDetail>().UpdateAsync(request.GoodsReceiptDetailDto.Adapt<GoodsReceiptDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);


                _cache.Remove("GetGoodsReceiptDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GoodsReceiptDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptDetailDto>> Handle(UpdateListGoodsReceiptDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptDetail>().UpdateAsync(request.GoodsReceiptDetailDtos.Adapt<List<GoodsReceiptDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GoodsReceiptDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Goods Receipt Detail

        #region UPDATE Goods Receipt 

        public async Task<GoodsReceiptDto> Handle(UpdateGoodsReceiptRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceipt>().UpdateAsync(request.GoodsReceiptDto.Adapt<GoodsReceipt>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GoodsReceiptDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptDto>> Handle(UpdateListGoodsReceiptRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceipt>().UpdateAsync(request.GoodsReceiptDtos.Adapt<List<GoodsReceipt>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GoodsReceiptDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Goods Receipt 

        #region UPDATE Goods Receipt Log

        public async Task<GoodsReceiptLogDto> Handle(UpdateGoodsReceiptLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptLog>().UpdateAsync(request.GoodsReceiptLogDto.Adapt<GoodsReceiptLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<GoodsReceiptLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<GoodsReceiptLogDto>> Handle(UpdateListGoodsReceiptLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<GoodsReceiptLog>().UpdateAsync(request.GoodsReceiptLogDtos.Adapt<List<GoodsReceiptLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<GoodsReceiptLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Goods Receipt 

        #region DELETE Goods Receipt Detail

        public async Task<bool> Handle(DeleteGoodsReceiptDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GoodsReceiptDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GoodsReceiptDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptDetailQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Goods Receipt Detail

        #region DELETE Goods Receipt 

        public async Task<bool> Handle(DeleteGoodsReceiptRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GoodsReceipt>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GoodsReceipt>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Goods Receipt 

        #region DELETE Goods Receipt Log

        public async Task<bool> Handle(DeleteGoodsReceiptLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<GoodsReceiptLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<GoodsReceiptLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetGoodsReceiptLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Goods Receipt 
    }
}
