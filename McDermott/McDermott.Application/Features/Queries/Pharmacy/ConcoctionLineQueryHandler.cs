using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class ConcoctionLineQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllConcoctionLineQuery, List<ConcoctionLineDto>>,//ConcoctionLine
        IRequestHandler<GetConcoctionLineQuery, (List<ConcoctionLineDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleConcoctionLineQuery, ConcoctionLineDto>, IRequestHandler<ValidateConcoctionLineQuery, bool>,
        IRequestHandler<BulkValidateConcoctionLineQuery, List<ConcoctionLineDto>>,
        IRequestHandler<CreateConcoctionLineRequest, ConcoctionLineDto>,
        IRequestHandler<CreateListConcoctionLineRequest, List<ConcoctionLineDto>>,
        IRequestHandler<UpdateConcoctionLineRequest, ConcoctionLineDto>,
        IRequestHandler<UpdateListConcoctionLineRequest, List<ConcoctionLineDto>>,
        IRequestHandler<DeleteConcoctionLineRequest, bool>,
        IRequestHandler<GetAllStockOutLinesQuery, List<StockOutLinesDto>>,//StockOutLines
        IRequestHandler<GetStockOutLinesQuery, (List<StockOutLinesDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleStockOutLinesQuery, StockOutLinesDto>, IRequestHandler<ValidateStockOutLinesQuery, bool>,
        IRequestHandler<BulkValidateStockOutLinesQuery, List<StockOutLinesDto>>, IRequestHandler<CreateStockOutLinesRequest, StockOutLinesDto>,
        IRequestHandler<CreateListStockOutLinesRequest, List<StockOutLinesDto>>,
        IRequestHandler<UpdateStockOutLinesRequest, StockOutLinesDto>,
        IRequestHandler<UpdateListStockOutLinesRequest, List<StockOutLinesDto>>,
        IRequestHandler<DeleteStockOutLinesRequest, bool>
    {
        #region Concoction Line

        #region GET CoctionLine

        public async Task<List<ConcoctionLineDto>> Handle(GetAllConcoctionLineQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllConcoctionLineQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ConcoctionLine>? result))
                {
                    result = await _unitOfWork.Repository<ConcoctionLine>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ConcoctionLineDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionLineDto>> Handle(BulkValidateConcoctionLineQuery request, CancellationToken cancellationToken)
        {
            var ConcoctionLineDtos = request.ConcoctionLineToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var ConcoctionLineNames = ConcoctionLineDtos.Select(x => x.Product.Name).Distinct().ToList();

            var existingConcoctionLines = await _unitOfWork.Repository<ConcoctionLine>()
                .Entities
                .AsNoTracking()
                .Where(v => ConcoctionLineNames.Contains(v.Product.Name))
                .ToListAsync(cancellationToken);

            return existingConcoctionLines.Adapt<List<ConcoctionLineDto>>();
        }

        public async Task<bool> Handle(ValidateConcoctionLineQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<ConcoctionLine>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<ConcoctionLineDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetConcoctionLineQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ConcoctionLine>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ConcoctionLine>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ConcoctionLine>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TotalQty.ToString(), $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ConcoctionLine
                    {
                        Id = x.Id,
                        ConcoctionId = x.ConcoctionId,
                        ProductId = x.ProductId,
                        TotalQty = x.TotalQty,
                        UomId = x.UomId,
                        MedicamentDosage = x.MedicamentDosage,
                        MedicamentUnitOfDosage = x.MedicamentUnitOfDosage,
                        Dosage = x.Dosage,
                        AvaliableQty = x.AvaliableQty,
                        ActiveComponentId = x.ActiveComponentId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        Uom = new Uom
                        {
                            Name = x.Uom == null ? string.Empty : x.Uom.Name,
                        },
                        ActiveComponent = x.ActiveComponent == null ? null : x.ActiveComponent.Select(ac => new ActiveComponent
                        {
                            Name = ac.Name
                        }).ToList(),
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<ConcoctionLineDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<ConcoctionLineDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<ConcoctionLineDto> Handle(GetSingleConcoctionLineQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<ConcoctionLine>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<ConcoctionLine>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<ConcoctionLine>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TotalQty.ToString(), $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new ConcoctionLine
                    {
                        Id = x.Id,
                        ConcoctionId = x.ConcoctionId,
                        ProductId = x.ProductId,
                        TotalQty = x.TotalQty,
                        UomId = x.UomId,
                        MedicamentDosage = x.MedicamentDosage,
                        MedicamentUnitOfDosage = x.MedicamentUnitOfDosage,
                        Dosage = x.Dosage,
                        AvaliableQty = x.AvaliableQty,
                        ActiveComponentId = x.ActiveComponentId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        Uom = new Uom
                        {
                            Name = x.Uom == null ? string.Empty : x.Uom.Name,
                        },
                        ActiveComponent = x.ActiveComponent == null ? null : x.ActiveComponent.Select(ac => new ActiveComponent
                        {
                            Name = ac.Name
                        }).ToList(),
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<ConcoctionLineDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET CoctionLine

        #region CREATE

        public async Task<ConcoctionLineDto> Handle(CreateConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().AddAsync(request.ConcoctionLineDto.Adapt<ConcoctionLine>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionLineDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionLineDto>> Handle(CreateListConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().AddAsync(request.ConcoctionLineDtos.Adapt<List<ConcoctionLine>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionLineDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ConcoctionLineDto> Handle(UpdateConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().UpdateAsync(request.ConcoctionLineDto.Adapt<ConcoctionLine>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ConcoctionLineDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ConcoctionLineDto>> Handle(UpdateListConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ConcoctionLine>().UpdateAsync(request.ConcoctionLineDtos.Adapt<List<ConcoctionLine>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ConcoctionLineDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteConcoctionLineRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ConcoctionLine>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ConcoctionLine>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetConcoctionLineQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Concoction Line

        #region Stock Out ConcoctionLine

        #region GET Concoction

        public async Task<List<StockOutLinesDto>> Handle(GetAllStockOutLinesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllStockOutLinesQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<StockOutLines>? result))
                {
                    result = await _unitOfWork.Repository<StockOutLines>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<StockOutLinesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutLinesDto>> Handle(BulkValidateStockOutLinesQuery request, CancellationToken cancellationToken)
        {
            var StockOutLinesDtos = request.StockOutLinesToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var StockOutLinesNames = StockOutLinesDtos.Select(x => x.CutStock).Distinct().ToList();

            var existingStockOutLiness = await _unitOfWork.Repository<StockOutLines>()
                .Entities
                .AsNoTracking()
                .Where(v => StockOutLinesNames.Contains((long)v.CutStock))
                .ToListAsync(cancellationToken);

            return existingStockOutLiness.Adapt<List<StockOutLinesDto>>();
        }

        public async Task<bool> Handle(ValidateStockOutLinesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<StockOutLines>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<StockOutLinesDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetStockOutLinesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<StockOutLines>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<StockOutLines>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<StockOutLines>)query).ThenBy(additionalOrderBy.OrderBy);
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

                //if (!string.IsNullOrEmpty(request.SearchTerm))
                //{
                //    query = query.Where(v =>
                //            EF.Functions.Like(v.EventName, $"%{request.SearchTerm}%") ||
                //            EF.Functions.Like(v.EventCategory.Name, $"%{request.SearchTerm}%")
                //            );
                //}

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new StockOutLines
                    {
                        Id = x.Id,
                        LinesId = x.LinesId,
                        CutStock = x.CutStock,
                        TransactionStockId = x.TransactionStockId,
                        Lines = new ConcoctionLine
                        {
                            ProductId = x.Lines.ProductId,
                            Product = new Product
                            {
                                Name = x.Lines.Product == null ? string.Empty : x.Lines.Product.Name
                            }
                        },
                        TransactionStock = new TransactionStock
                        {
                            Batch = x.TransactionStock == null ? string.Empty : x.TransactionStock.Batch
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

                    return (pagedItems.Adapt<List<StockOutLinesDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<StockOutLinesDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<StockOutLinesDto> Handle(GetSingleStockOutLinesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<StockOutLines>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<StockOutLines>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<StockOutLines>)query).ThenBy(additionalOrderBy.OrderBy);
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

                //if (!string.IsNullOrEmpty(request.SearchTerm))
                //{
                //    query = query.Where(v =>
                //            EF.Functions.Like(v.EventName, $"%{request.SearchTerm}%") ||
                //            EF.Functions.Like(v.EventCategory.Name, $"%{request.SearchTerm}%")
                //            );
                //}

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new StockOutLines
                    {
                        Id = x.Id,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<StockOutLinesDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Concoction

        #region CREATE

        public async Task<StockOutLinesDto> Handle(CreateStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().AddAsync(request.StockOutLinesDto.Adapt<StockOutLines>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutLinesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutLinesDto>> Handle(CreateListStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().AddAsync(request.StockOutLinesDtos.Adapt<List<StockOutLines>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutLinesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<StockOutLinesDto> Handle(UpdateStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().UpdateAsync(request.StockOutLinesDto.Adapt<StockOutLines>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutLinesDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutLinesDto>> Handle(UpdateListStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutLines>().UpdateAsync(request.StockOutLinesDto.Adapt<List<StockOutLines>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutLinesDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteStockOutLinesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<StockOutLines>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<StockOutLines>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutLinesQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #endregion Stock Out ConcoctionLine
    }
}