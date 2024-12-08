using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class InventoryAdjusmentDetailQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInventoryAdjusmentDetailQuery, (List<InventoryAdjusmentDetailDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleInventoryAdjusmentDetailQuery, InventoryAdjusmentDetailDto>,
        IRequestHandler<CreateInventoryAdjusmentDetailRequest, InventoryAdjusmentDetailDto>,
        IRequestHandler<ValidateInventoryAdjusmentDetail, bool>,
        IRequestHandler<CreateListInventoryAdjusmentDetailRequest, List<InventoryAdjusmentDetailDto>>,
        IRequestHandler<UpdateInventoryAdjusmentDetailRequest, InventoryAdjusmentDetailDto>,
        IRequestHandler<UpdateListInventoryAdjusmentDetailRequest, List<InventoryAdjusmentDetailDto>>,
        IRequestHandler<DeleteInventoryAdjusmentDetailRequest, bool>
    {
        #region GET

        public async Task<bool> Handle(ValidateInventoryAdjusmentDetail request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<InventoryAdjusmentDetail>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<InventoryAdjusmentDetailDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetInventoryAdjusmentDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<InventoryAdjusmentDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<InventoryAdjusmentDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<InventoryAdjusmentDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //        EF.Functions.Like(v.InventoryAdjusmentDetail.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new InventoryAdjusmentDetail
                    {
                        Id = x.Id,
                        StockProductId = x.StockProductId,
                        Batch = x.Batch,
                        InventoryAdjusmentId = x.InventoryAdjusmentId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? "" : x.Product.Name,
                            UomId = x.Product == null ? null : x.Product.UomId,
                            Uom = new Uom
                            {
                                Name = x.Product.Uom == null ? "" : x.Product.Uom.Name
                            }
                        },
                        ExpiredDate = x.ExpiredDate,
                        RealQty = x.RealQty,
                        StockProduct = x.StockProduct,
                        TransactionStockId = x.TransactionStockId,
                        TeoriticalQty = x.TeoriticalQty,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<InventoryAdjusmentDetailDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<InventoryAdjusmentDetailDto>>(), 0, 1, 1);
                }
            }
            catch (Exception )
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<InventoryAdjusmentDetailDto> Handle(GetSingleInventoryAdjusmentDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<InventoryAdjusmentDetail>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<InventoryAdjusmentDetail>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<InventoryAdjusmentDetail>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //query = query.Where(v =>
                    //    EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                    //    EF.Functions.Like(v.InventoryAdjusmentDetail.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new InventoryAdjusmentDetail
                    {
                        Id = x.Id,
                        StockProductId = x.StockProductId,
                        Batch = x.Batch,
                        InventoryAdjusmentId = x.InventoryAdjusmentId,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? "" : x.Product.Name,
                            UomId = x.Product == null ? null : x.Product.UomId,
                            Uom = new Uom
                            {
                                Name = x.Product.Uom == null ? "" : x.Product.Uom.Name
                            }
                        },
                        ExpiredDate = x.ExpiredDate,
                        RealQty = x.RealQty,
                        StockProduct = x.StockProduct,
                        TransactionStockId = x.TransactionStockId,
                        TeoriticalQty = x.TeoriticalQty,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<InventoryAdjusmentDetailDto>();
            }
            catch (Exception )
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<InventoryAdjusmentDetailDto> Handle(CreateInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().AddAsync(request.InventoryAdjusmentDetailDto.Adapt<InventoryAdjusmentDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjusmentDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjusmentDetailDto>> Handle(CreateListInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().AddAsync(request.InventoryAdjusmentDetailDtos.Adapt<List<InventoryAdjusmentDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjusmentDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InventoryAdjusmentDetailDto> Handle(UpdateInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().UpdateAsync(request.InventoryAdjusmentDetailDto.Adapt<InventoryAdjusmentDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjusmentDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjusmentDetailDto>> Handle(UpdateListInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().UpdateAsync(request.InventoryAdjusmentDetailDtos.Adapt<List<InventoryAdjusmentDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjusmentDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<InventoryAdjusmentDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<InventoryAdjusmentDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

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