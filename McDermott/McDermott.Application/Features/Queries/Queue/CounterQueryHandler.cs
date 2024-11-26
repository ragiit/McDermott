using McDermott.Application.Dtos.Queue;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class CounterHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
      IRequestHandler<GetCounterQuery, (List<CounterDto>, int pageIndex, int pageSize, int pageCount)>,
      IRequestHandler<GetSingleCounterQuery, CounterDto>, IRequestHandler<ValidateCounterQuery, bool>,
      IRequestHandler<CreateCounterRequest, CounterDto>,
      IRequestHandler<BulkValidateCounterQuery, List<CounterDto>>,
      IRequestHandler<CreateListCounterRequest, List<CounterDto>>,
      IRequestHandler<UpdateCounterRequest, CounterDto>,
      IRequestHandler<UpdateListCounterRequest, List<CounterDto>>,
      IRequestHandler<DeleteCounterRequest, bool>
    {
        #region GET

        public async Task<List<CounterDto>> Handle(BulkValidateCounterQuery request, CancellationToken cancellationToken)
        {
            var CounterDtos = request.CountersToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var CounterNames = CounterDtos.Select(x => x.Name).Distinct().ToList();
            //var a = CounterDtos.Select(x => x.CountryId).Distinct().ToList();

            //var existingCounters = await _unitOfWork.Repository<Counter>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => CounterNames.Contains(v.Name)
            //                && a.Contains(v.CountryId))
            //    .ToListAsync(cancellationToken);

            //return existingCounters.Adapt<List<CounterDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateCounterQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Counter>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<CounterDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetCounterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Counter>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Counter>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Counter>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new Counter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                        },
                        PhysicianId = x.PhysicianId,
                        Physician = new User
                        {
                            Name = x.Physician == null ? string.Empty : x.Physician.Name
                        },
                        IsActive = x.IsActive,
                        Status = x.Status,
                        ServiceKId = x.ServiceKId,
                        ServiceK = new Service
                        {
                            Name = x.ServiceK == null ? string.Empty : x.ServiceK.Name
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

                    return (pagedItems.Adapt<List<CounterDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<CounterDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<CounterDto> Handle(GetSingleCounterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Counter>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Counter>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Counter>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Select(x => new Counter
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                        },
                        PhysicianId = x.PhysicianId,
                        Physician = new User
                        {
                            Name = x.Physician == null ? string.Empty : x.Physician.Name
                        },
                        IsActive = x.IsActive,
                        Status = x.Status,
                        ServiceKId = x.ServiceKId,
                        ServiceK = new Service
                        {
                            Name = x.ServiceK == null ? string.Empty : x.ServiceK.Name
                        }
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<CounterDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<CounterDto> Handle(CreateCounterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Counter>().AddAsync(request.CounterDto.Adapt<CreateUpdateCounterDto>().Adapt<Counter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCounter_"); // Ganti dengan key yang sesuai

                return result.Adapt<CounterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CounterDto>> Handle(CreateListCounterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Counter>().AddAsync(request.CounterDtos.Adapt<List<Counter>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCounter_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CounterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CounterDto> Handle(UpdateCounterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Counter>().UpdateAsync(request.CounterDto.Adapt<CounterDto>().Adapt<Counter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCounter_"); // Ganti dengan key yang sesuai

                return result.Adapt<CounterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CounterDto>> Handle(UpdateListCounterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Counter>().UpdateAsync(request.CounterDtos.Adapt<List<Counter>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCounter_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<CounterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCounterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Counter>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Counter>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCounter_"); // Ganti dengan key yang sesuai

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