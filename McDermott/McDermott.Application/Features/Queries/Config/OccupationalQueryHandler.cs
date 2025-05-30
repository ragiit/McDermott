using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class OccupationalQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetOccupationalQuery, (List<OccupationalDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleOccupationalQuery, OccupationalDto>,
        IRequestHandler<ValidateOccupationalQuery, bool>,
        IRequestHandler<BulkValidateOccupationalQuery, List<OccupationalDto>>,
        IRequestHandler<CreateOccupationalRequest, OccupationalDto>,
        IRequestHandler<CreateListOccupationalRequest, List<OccupationalDto>>,
        IRequestHandler<UpdateOccupationalRequest, OccupationalDto>,
        IRequestHandler<UpdateListOccupationalRequest, List<OccupationalDto>>,
        IRequestHandler<DeleteOccupationalRequest, bool>
    {
        #region GET

        public async Task<List<OccupationalDto>> Handle(BulkValidateOccupationalQuery request, CancellationToken cancellationToken)
        {
            var OccupationalDtos = request.OccupationalsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var OccupationalNames = OccupationalDtos.Select(x => x.Name).Distinct().ToList();

            var existingOccupationals = await _unitOfWork.Repository<Occupational>()
                .Entities
                .AsNoTracking()
                .Where(v => OccupationalNames.Contains(v.Name))
                .ToListAsync(cancellationToken);

            return existingOccupationals.Adapt<List<OccupationalDto>>();
        }

        public async Task<(List<OccupationalDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetOccupationalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Occupational>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Occupational>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Occupational>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                            EF.Functions.Like(v.Description, $"%{request.SearchTerm}%")
                            );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Occupational
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<OccupationalDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<OccupationalDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<OccupationalDto> Handle(GetSingleOccupationalQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Occupational>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Occupational>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Occupational>)query).ThenBy(additionalOrderBy.OrderBy);
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
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Description, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Occupational
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<OccupationalDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<bool> Handle(ValidateOccupationalQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Occupational>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<OccupationalDto> Handle(CreateOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().AddAsync(request.OccupationalDto.Adapt<Occupational>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<OccupationalDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OccupationalDto>> Handle(CreateListOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().AddAsync(request.OccupationalDtos.Adapt<List<Occupational>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<OccupationalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<OccupationalDto> Handle(UpdateOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().UpdateAsync(request.OccupationalDto.Adapt<Occupational>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<OccupationalDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OccupationalDto>> Handle(UpdateListOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Occupational>().UpdateAsync(request.OccupationalDtos.Adapt<List<Occupational>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<OccupationalDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteOccupationalRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Occupational>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Occupational>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetOccupationalQuery_"); // Ganti dengan key yang sesuai

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