using McDermott.Application.Features.Commands;
using McDermott.Application.Features.Services;
using McDermott.Domain.Common;
using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceRecordCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintenanceRecordQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllMaintenanceRecordQuery, List<MaintenanceRecordDto>>,
        IRequestHandler<GetMaintenanceRecordQuery, (List<MaintenanceRecordDto>, int pageIndex, int pageSize, int pageCount)>, //MaintenanceRecord
        IRequestHandler<GetSingleMaintenanceRecordQuery, MaintenanceRecordDto>, IRequestHandler<ValidateMaintenanceRecordQuery, bool>,
        IRequestHandler<GetQueryMaintenanceRecord, IQueryable<MaintenanceRecord>>,
        IRequestHandler<CreateMaintenanceRecordRequest, MaintenanceRecordDto>,
        IRequestHandler<CreateListMaintenanceRecordRequest, List<MaintenanceRecordDto>>,
        IRequestHandler<UpdateMaintenanceRecordRequest, MaintenanceRecordDto>,
        IRequestHandler<UpdateListMaintenanceRecordRequest, List<MaintenanceRecordDto>>,
        IRequestHandler<DeleteMaintenanceRecordRequest, bool>
    {
        #region GET

        public async Task<List<MaintenanceRecordDto>> Handle(GetAllMaintenanceRecordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllMaintenanceRecordQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<MaintenanceRecord>? result))
                {
                    result = await _unitOfWork.Repository<MaintenanceRecord>().GetAsync(
                        null,
                        x => x
                        .Include(z => z.Maintenance)
                        .Include(x => x.Product),
                        cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintenanceRecordDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> Handle(ValidateMaintenanceRecordQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<MaintenanceRecord>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<MaintenanceRecordDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintenanceRecordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaintenanceRecord>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<MaintenanceRecord>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<MaintenanceRecord>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.DocumentName, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.SequenceProduct, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MaintenanceRecord
                    {
                        Id = x.Id,
                        DocumentName = x.DocumentName,
                        SequenceProduct = x.SequenceProduct,
                        ProductId = x.ProductId,
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

                    return (pagedItems.Adapt<List<MaintenanceRecordDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<MaintenanceRecordDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<MaintenanceRecordDto> Handle(GetSingleMaintenanceRecordQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<MaintenanceRecord>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<MaintenanceRecord>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<MaintenanceRecord>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.DocumentName, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.SequenceProduct, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new MaintenanceRecord
                    {
                        Id = x.Id,
                        DocumentName = x.DocumentName,
                        SequenceProduct = x.SequenceProduct,
                        ProductId = x.ProductId,
                        Product = new Product
                        {
                            Name = x.Product == null ? string.Empty : x.Product.Name,
                        },
                        MaintenanceId = x.MaintenanceId,
                        Maintenance = new Maintenance
                        {
                            Title = x.Maintenance == null ? string.Empty : x.Maintenance.Title
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MaintenanceRecordDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public Task<IQueryable<MaintenanceRecord>> Handle(GetQueryMaintenanceRecord request, CancellationToken cancellationToken)
        {
            return HandleQuery<MaintenanceRecord>(request, cancellationToken, request.Select is null ? x => new MaintenanceRecord
            {
                Id = x.Id,
                DocumentName=x.DocumentName, 
                SequenceProduct = x.SequenceProduct,
                ProductId = x.ProductId,
                MaintenanceId =x.MaintenanceId,
                Product = new Product
                {
                    Name= x.Product == null ? string.Empty : x.Product.Name
                },
                Maintenance= new Maintenance
                {
                    Title = x.Maintenance == null ? string.Empty : x.Maintenance.Title
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
            if (typeof(TEntity) == typeof(MaintenanceRecord))
            {
                var MaintenanceRecordQuery = query as IQueryable<MaintenanceRecord>;
                return (IQueryable<TEntity>)MaintenanceRecordQuery.Where(v =>
                    EF.Functions.Like(v.Product.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.SequenceProduct, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Maintenance.Title, $"%{searchTerm}%"));
            }
            return query; // No filtering if the type doesn't match
        }

        #endregion GET

        #region CREATE

        public async Task<MaintenanceRecordDto> Handle(CreateMaintenanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceRecord>().AddAsync(request.MaintenanceRecordDto.Adapt<MaintenanceRecord>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceRecordDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceRecordDto>> Handle(CreateListMaintenanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceRecord>().AddAsync(request.MaintenanceRecordDtos.Adapt<List<MaintenanceRecord>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceRecordDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintenanceRecordDto> Handle(UpdateMaintenanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceRecord>().UpdateAsync(request.MaintenanceRecordDto.Adapt<MaintenanceRecord>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceRecordDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceRecordDto>> Handle(UpdateListMaintenanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<MaintenanceRecord>().UpdateAsync(request.MaintenanceRecordDtos.Adapt<List<MaintenanceRecord>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceRecordQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceRecordDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintenanceRecordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<MaintenanceRecord>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<MaintenanceRecord>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceRecordQuery_"); // Ganti dengan key yang sesuai

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