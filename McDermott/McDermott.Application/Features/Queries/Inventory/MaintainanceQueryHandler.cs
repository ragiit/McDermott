using DocumentFormat.OpenXml.Wordprocessing;
using McDermott.Application.Features.Commands;
using McDermott.Application.Features.Services;
using McDermott.Domain.Common;
using static McDermott.Application.Features.Commands.GetDataCommand;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class MaintenanceQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetMaintenanceQuery, (List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)>, //Maintenance
        IRequestHandler<GetQueryMaintenance, IQueryable<Maintenance>>,
        IRequestHandler<GetSingleMaintenanceQuery, MaintenanceDto>,
        IRequestHandler<GetAllMaintenanceQuery, List<MaintenanceDto>>,
        IRequestHandler<ValidateMaintenanceQuery, bool>,
        IRequestHandler<CreateMaintenanceRequest, MaintenanceDto>,
        IRequestHandler<CreateListMaintenanceRequest, List<MaintenanceDto>>,
        IRequestHandler<UpdateMaintenanceRequest, MaintenanceDto>,
        IRequestHandler<UpdateListMaintenanceRequest, List<MaintenanceDto>>,
        IRequestHandler<DeleteMaintenanceRequest, bool>
    {
        #region GET

        public async Task<List<MaintenanceDto>> Handle(GetAllMaintenanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetMaintenanceQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Maintenance>? result))
                {
                    result = await _unitOfWork.Repository<Maintenance>().Entities
                    .Include(x => x.RequestBy)
                    .Include(x => x.Location)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<MaintenanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<MaintenanceDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetMaintenanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Maintenance>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Maintenance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Maintenance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Sequence, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Title, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Maintenance
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Sequence = x.Sequence,
                        RequestDate = x.RequestDate,
                        ScheduleDate = x.ScheduleDate,
                        isCorrective = x.isCorrective,
                        isPreventive = x.isPreventive,
                        isInternal = x.isInternal,
                        isExternal = x.isExternal,
                        VendorBy = x.VendorBy,
                        Recurrent = x.Recurrent,
                        RepeatNumber = x.RepeatNumber,
                        RepeatWork = x.RepeatWork,
                        Status = x.Status,
                        ResponsibleById = x.ResponsibleById,
                        RequestById = x.RequestById,
                        LocationId = x.LocationId,
                        CreatedBy = x.CreatedBy,
                        RequestBy = new User
                        {
                            Name = x.RequestBy == null ? string.Empty : x.RequestBy.Name,
                        },
                        ResponsibleBy = new User
                        {
                            Name = x.ResponsibleBy == null ? string.Empty : x.ResponsibleBy.Name,
                        },
                        Location = new Locations
                        {
                            Name = x.Location == null ? string.Empty : x.Location.Name,
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

                    return (pagedItems.Adapt<List<MaintenanceDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<MaintenanceDto>>(), 0, 1, 1);
                }
            }
            catch (Exception)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<MaintenanceDto> Handle(GetSingleMaintenanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Maintenance>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Maintenance>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Maintenance>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Sequence, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Title, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Maintenance
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Sequence = x.Sequence,
                        RequestDate = x.RequestDate,
                        ScheduleDate = x.ScheduleDate,
                        isCorrective = x.isCorrective,
                        isPreventive = x.isPreventive,
                        isInternal = x.isInternal,
                        isExternal = x.isExternal,
                        VendorBy = x.VendorBy,
                        Recurrent = x.Recurrent,
                        RepeatNumber = x.RepeatNumber,
                        RepeatWork = x.RepeatWork,
                        Status = x.Status,
                        ResponsibleById = x.ResponsibleById,
                        RequestById = x.RequestById,
                        LocationId = x.LocationId,
                        CreatedBy = x.CreatedBy,
                        RequestBy = new User
                        {
                            Name = x.RequestBy == null ? string.Empty : x.RequestBy.Name,
                        },
                        ResponsibleBy = new User
                        {
                            Name = x.ResponsibleBy == null ? string.Empty : x.ResponsibleBy.Name,
                        },
                        Location = new Locations
                        {
                            Name = x.Location == null ? string.Empty : x.Location.Name,
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<MaintenanceDto>();
            }
            catch (Exception)
            {
                // Consider logging the exception
                throw;
            }
        }

        public Task<IQueryable<Maintenance>> Handle(GetQueryMaintenance request, CancellationToken cancellationToken)
        {
            return HandleQuery<Maintenance>(request, cancellationToken, request.Select is null ? x => new Maintenance
            {
                Id = x.Id,
                Title = x.Title,
                Sequence = x.Sequence,
                RequestDate = x.RequestDate,
                ScheduleDate = x.ScheduleDate,
                isCorrective = x.isCorrective,
                isPreventive = x.isPreventive,
                isInternal = x.isInternal,
                isExternal = x.isExternal,
                VendorBy = x.VendorBy,
                Recurrent = x.Recurrent,
                RepeatNumber = x.RepeatNumber,
                RepeatWork = x.RepeatWork,
                Status = x.Status,
                ResponsibleById = x.ResponsibleById,
                RequestById = x.RequestById,
                LocationId = x.LocationId,
                CreatedBy = x.CreatedBy,
                RequestBy = new User
                {
                    Name = x.RequestBy == null ? string.Empty : x.RequestBy.Name,
                },
                ResponsibleBy = new User
                {
                    Name = x.ResponsibleBy == null ? string.Empty : x.ResponsibleBy.Name,
                },
                Location = new Locations
                {
                    Name = x.Location == null ? string.Empty : x.Location.Name,
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
            if (typeof(TEntity) == typeof(Maintenance))
            {
                var MaintenanceQuery = query as IQueryable<Maintenance>;
                return (IQueryable<TEntity>)MaintenanceQuery.Where(v =>
                    EF.Functions.Like(v.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.ResponsibleBy.Name, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.Sequence, $"%{searchTerm}%") ||
                    EF.Functions.Like(v.RequestBy.Name, $"%{searchTerm}%"));
            }
            return query; // No filtering if the type doesn't match
        }

        public async Task<bool> Handle(ValidateMaintenanceQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Maintenance>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<MaintenanceDto> Handle(CreateMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDto.Adapt<Maintenance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceDto>> Handle(CreateListMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().AddAsync(request.MaintenanceDtos.Adapt<List<Maintenance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<MaintenanceDto> Handle(UpdateMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDto.Adapt<Maintenance>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<MaintenanceDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<MaintenanceDto>> Handle(UpdateListMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Maintenance>().UpdateAsync(request.MaintenanceDtos.Adapt<List<Maintenance>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<MaintenanceDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteMaintenanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Maintenance>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Maintenance>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetMaintenanceQuery_"); // Ganti dengan key yang sesuai

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