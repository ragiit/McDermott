using McDermott.Application.Dtos.Queue;
using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Application.Features.Queries.Queue
{
    public class QueueDisplayQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetQueueDisplayQuery, (List<QueueDisplayDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleQueueDisplayQuery, QueueDisplayDto>, IRequestHandler<ValidateQueueDisplayQuery, bool>,
     IRequestHandler<CreateQueueDisplayRequest, QueueDisplayDto>,
     IRequestHandler<BulkValidateQueueDisplayQuery, List<QueueDisplayDto>>,
     IRequestHandler<CreateListQueueDisplayRequest, List<QueueDisplayDto>>,
     IRequestHandler<UpdateQueueDisplayRequest, QueueDisplayDto>,
     IRequestHandler<UpdateListQueueDisplayRequest, List<QueueDisplayDto>>,
     IRequestHandler<DeleteQueueDisplayRequest, bool>
    {
        #region GET

        public async Task<List<QueueDisplayDto>> Handle(BulkValidateQueueDisplayQuery request, CancellationToken cancellationToken)
        {
            var QueueDisplayDtos = request.QueueDisplaysToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var QueueDisplayNames = QueueDisplayDtos.Select(x => x.Name).Distinct().ToList();
            //var a = QueueDisplayDtos.Select(x => x.CountryId).Distinct().ToList();

            //var existingQueueDisplays = await _unitOfWork.Repository<QueueDisplay>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => QueueDisplayNames.Contains(v.Name)
            //                && a.Contains(v.CountryId))
            //    .ToListAsync(cancellationToken);

            return [];

            //return existingQueueDisplays.Adapt<List<QueueDisplayDto>>();
        }

        public async Task<bool> Handle(ValidateQueueDisplayQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<QueueDisplay>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<QueueDisplayDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetQueueDisplayQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<QueueDisplay>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<QueueDisplay>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<QueueDisplay>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Where(v => EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new QueueDisplay
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CounterIds = x.CounterIds
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<QueueDisplayDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<QueueDisplayDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<QueueDisplayDto> Handle(GetSingleQueueDisplayQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<QueueDisplay>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<QueueDisplay>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<QueueDisplay>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    query = query.Where(v => EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new QueueDisplay
                    {
                        Id = x.Id,
                        Name = x.Name,
                        CounterIds = x.CounterIds
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<QueueDisplayDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<QueueDisplayDto> Handle(CreateQueueDisplayRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<QueueDisplay>().AddAsync(request.QueueDisplayDto.Adapt<CreateUpdateQueueDisplayDto>().Adapt<QueueDisplay>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetQueueDisplayQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<QueueDisplayDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<QueueDisplayDto>> Handle(CreateListQueueDisplayRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<QueueDisplay>().AddAsync(request.QueueDisplayDtos.Adapt<List<QueueDisplay>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetQueueDisplayQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<QueueDisplayDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<QueueDisplayDto> Handle(UpdateQueueDisplayRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<QueueDisplay>().UpdateAsync(request.QueueDisplayDto.Adapt<QueueDisplayDto>().Adapt<QueueDisplay>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetQueueDisplayQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<QueueDisplayDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<QueueDisplayDto>> Handle(UpdateListQueueDisplayRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<QueueDisplay>().UpdateAsync(request.QueueDisplayDtos.Adapt<List<QueueDisplay>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetQueueDisplayQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<QueueDisplayDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteQueueDisplayRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<QueueDisplay>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<QueueDisplay>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetQueueDisplayQuery_"); // Ganti dengan key yang sesuai

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