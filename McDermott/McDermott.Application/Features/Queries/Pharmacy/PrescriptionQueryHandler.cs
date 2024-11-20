using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Dtos.Pharmacies;
using static McDermott.Application.Features.Commands.Pharmacies.PrescriptionCommand;
using McDermott.Application.Features.Services;

namespace McDermott.Application.Features.Queries.Pharmacies
{
    public class PrescriptionQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
       IRequestHandler<GetAllPrescriptionQuery, List<PrescriptionDto>>,//Prescription
        IRequestHandler<GetPrescriptionQuery, (List<PrescriptionDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSinglePrescriptionQuery, PrescriptionDto>, IRequestHandler<ValidatePrescriptionQuery, bool>,
        IRequestHandler<BulkValidatePrescriptionQuery, List<PrescriptionDto>>,
        IRequestHandler<CreatePrescriptionRequest, PrescriptionDto>,
        IRequestHandler<CreateListPrescriptionRequest, List<PrescriptionDto>>,
        IRequestHandler<UpdatePrescriptionRequest, PrescriptionDto>,
        IRequestHandler<UpdateListPrescriptionRequest, List<PrescriptionDto>>,
        IRequestHandler<DeletePrescriptionRequest, bool>,
        IRequestHandler<GetAllStockOutPrescriptionQuery, List<StockOutPrescriptionDto>>,//StockOutPrescription
        IRequestHandler<GetStockOutPrescriptionQuery, (List<StockOutPrescriptionDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleStockOutPrescriptionQuery, StockOutPrescriptionDto>, IRequestHandler<ValidateStockOutPrescriptionQuery, bool>,
        IRequestHandler<BulkValidateStockOutPrescriptionQuery, List<StockOutPrescriptionDto>>, IRequestHandler<CreateStockOutPrescriptionRequest, StockOutPrescriptionDto>,
        IRequestHandler<CreateListStockOutPrescriptionRequest, List<StockOutPrescriptionDto>>,
        IRequestHandler<UpdateStockOutPrescriptionRequest, StockOutPrescriptionDto>,
        IRequestHandler<UpdateListStockOutPrescriptionRequest, List<StockOutPrescriptionDto>>,
        IRequestHandler<DeleteStockOutPrescriptionRequest, bool>
    {
        #region Prescription
        #region GET Education Program

        public async Task<List<PrescriptionDto>> Handle(GetAllPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllPrescriptionQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Prescription>? result))
                {
                    result = await _unitOfWork.Repository<Prescription>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<PrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PrescriptionDto>> Handle(BulkValidatePrescriptionQuery request, CancellationToken cancellationToken)
        {
            var PrescriptionDtos = request.PrescriptionToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var PrescriptionNames = PrescriptionDtos.Select(x => x.Product.Name).Distinct().ToList();

            var existingPrescriptions = await _unitOfWork.Repository<Prescription>()
                .Entities
                .AsNoTracking()
                .Where(v => PrescriptionNames.Contains(v.Product.Name))
                .ToListAsync(cancellationToken);

            return existingPrescriptions.Adapt<List<PrescriptionDto>>();
        }

        public async Task<bool> Handle(ValidatePrescriptionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Prescription>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<PrescriptionDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Prescription>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Prescription>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Prescription>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Stock.ToString(), $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Prescription
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        SignaId = x.SignaId,
                        DrugDosageId = x.DrugDosageId,
                        DrugFromId = x.DrugFromId,
                        DrugRouteId = x.DrugRouteId,
                        ActiveComponentId = x.ActiveComponentId,
                        DosageFrequency = x.DosageFrequency,
                        Stock = x.Stock,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        Signa = new Signa
                        {
                            Name = x.Signa == null ? string.Empty : x.Signa.Name,
                        },
                        DrugDosage = new DrugDosage
                        {
                            Frequency = x.DrugDosage == null ? string.Empty : x.DrugDosage.Frequency,
                        },
                        DrugForm = new DrugForm
                        {
                            Name = x.DrugForm == null ? string.Empty : x.DrugForm.Name,
                        },
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty : x.DrugRoute.Route,
                        },
                        ActiveComponent = x.ActiveComponent == null ? null : x.ActiveComponent.Select(ac => new ActiveComponent
                        {
                            Name = ac.Name
                        }).ToList()

                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<PrescriptionDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<PrescriptionDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<PrescriptionDto> Handle(GetSinglePrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Prescription>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Prescription>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Prescription>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Stock.ToString(), $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Prescription
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        SignaId = x.SignaId,
                        DrugDosageId = x.DrugDosageId,
                        DrugFromId = x.DrugFromId,
                        DrugRouteId = x.DrugRouteId,
                        ActiveComponentId = x.ActiveComponentId,
                        DosageFrequency = x.DosageFrequency,
                        Stock = x.Stock,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        Signa = new Signa
                        {
                            Name = x.Signa == null ? string.Empty : x.Signa.Name,
                        },
                        DrugDosage = new DrugDosage
                        {
                            Frequency = x.DrugDosage == null ? string.Empty : x.DrugDosage.Frequency,
                        },
                        DrugForm = new DrugForm
                        {
                            Name = x.DrugForm == null ? string.Empty : x.DrugForm.Name,
                        },
                        DrugRoute = new DrugRoute
                        {
                            Route = x.DrugRoute == null ? string.Empty : x.DrugRoute.Route,
                        },
                        ActiveComponent = x.ActiveComponent == null ? null : x.ActiveComponent.Select(ac => new ActiveComponent
                        {
                            Name = ac.Name
                        }).ToList(),

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<PrescriptionDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Education Program

        #region CREATE

        public async Task<PrescriptionDto> Handle(CreatePrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().AddAsync(request.PrescriptionDto.Adapt<Prescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PrescriptionDto>> Handle(CreateListPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().AddAsync(request.PrescriptionDtos.Adapt<List<Prescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<PrescriptionDto> Handle(UpdatePrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().UpdateAsync(request.PrescriptionDto.Adapt<Prescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<PrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PrescriptionDto>> Handle(UpdateListPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Prescription>().UpdateAsync(request.PrescriptionDtos.Adapt<List<Prescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<PrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeletePrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Prescription>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Prescription>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
        #endregion

        #region Stock Out Prescription
        #region GET Education Program

        public async Task<List<StockOutPrescriptionDto>> Handle(GetAllStockOutPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllStockOutPrescriptionQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<StockOutPrescription>? result))
                {
                    result = await _unitOfWork.Repository<StockOutPrescription>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<StockOutPrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutPrescriptionDto>> Handle(BulkValidateStockOutPrescriptionQuery request, CancellationToken cancellationToken)
        {
            var StockOutPrescriptionDtos = request.StockOutPrescriptionToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var StockOutPrescriptionNames = StockOutPrescriptionDtos.Select(x => x.CutStock).Distinct().ToList();

            var existingStockOutPrescriptions = await _unitOfWork.Repository<StockOutPrescription>()
                .Entities
                .AsNoTracking()
                .Where(v => StockOutPrescriptionNames.Contains((long)v.CutStock))
                .ToListAsync(cancellationToken);

            return existingStockOutPrescriptions.Adapt<List<StockOutPrescriptionDto>>();
        }

        public async Task<bool> Handle(ValidateStockOutPrescriptionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<StockOutPrescription>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<StockOutPrescriptionDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetStockOutPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<StockOutPrescription>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<StockOutPrescription>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<StockOutPrescription>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.CutStock.ToString(), $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.TransactionStock.Batch, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new StockOutPrescription
                    {
                        Id = x.Id,
                       
                        PrescriptionId = x.PrescriptionId,
                        TransactionStockId = x.TransactionStockId,
                        CutStock = x.CutStock,

                        TransactionStock = new TransactionStock
                        {
                            Batch = x.TransactionStock == null ? string.Empty : x.TransactionStock.Batch,
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

                    return (pagedItems.Adapt<List<StockOutPrescriptionDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<StockOutPrescriptionDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<StockOutPrescriptionDto> Handle(GetSingleStockOutPrescriptionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<StockOutPrescription>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<StockOutPrescription>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<StockOutPrescription>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.TransactionStock.Batch, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.CutStock.ToString(), $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new StockOutPrescription
                    {
                        Id = x.Id,
                        PrescriptionId = x.PrescriptionId,
                        TransactionStockId = x.TransactionStockId,
                        CutStock = x.CutStock,
                        
                        TransactionStock = new TransactionStock
                        {
                            Batch = x.TransactionStock == null ? string.Empty : x.TransactionStock.Batch,
                        },

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<StockOutPrescriptionDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }



        #endregion GET Education Program

        #region CREATE

        public async Task<StockOutPrescriptionDto> Handle(CreateStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().AddAsync(request.StockOutPrescriptionDto.Adapt<StockOutPrescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutPrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutPrescriptionDto>> Handle(CreateListStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().AddAsync(request.StockOutPrescriptionDtos.Adapt<List<StockOutPrescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutPrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<StockOutPrescriptionDto> Handle(UpdateStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().UpdateAsync(request.StockOutPrescriptionDto.Adapt<StockOutPrescription>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<StockOutPrescriptionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StockOutPrescriptionDto>> Handle(UpdateListStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<StockOutPrescription>().UpdateAsync(request.StockOutPrescriptionDtos.Adapt<List<StockOutPrescription>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<StockOutPrescriptionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteStockOutPrescriptionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<StockOutPrescription>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<StockOutPrescription>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetStockOutPrescriptionQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE
        #endregion
    }
}
