using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramSessionCommand;

namespace McDermott.Application.Features.Queries.Transaction
{
    public class WellnessProgramSessionHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
     IRequestHandler<GetWellnessProgramSessionQuery, (List<WellnessProgramSessionDto>, int pageIndex, int pageSize, int pageCount)>,
     IRequestHandler<GetSingleWellnessProgramSessionQuery, WellnessProgramSessionDto>, IRequestHandler<ValidateWellnessProgramSession, bool>,
     IRequestHandler<CreateWellnessProgramSessionRequest, WellnessProgramSessionDto>,
     IRequestHandler<BulkValidateWellnessProgramSession, List<WellnessProgramSessionDto>>,
     IRequestHandler<CreateListWellnessProgramSessionRequest, List<WellnessProgramSessionDto>>,
     IRequestHandler<UpdateWellnessProgramSessionRequest, WellnessProgramSessionDto>,
     IRequestHandler<UpdateListWellnessProgramSessionRequest, List<WellnessProgramSessionDto>>,
     IRequestHandler<DeleteWellnessProgramSessionRequest, bool>
    {
        #region GET

        public async Task<List<WellnessProgramSessionDto>> Handle(BulkValidateWellnessProgramSession request, CancellationToken cancellationToken)
        {
            var CountryDtos = request.WellnessProgramSessionsToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            //var CountryNames = CountryDtos.Select(x => x.Name).Distinct().ToList();
            //var Codes = CountryDtos.Select(x => x.Code).Distinct().ToList();

            //var existingCountrys = await _unitOfWork.Repository<Country>()
            //    .Entities
            //    .AsNoTracking()
            //    .Where(v => CountryNames.Contains(v.Name) && Codes.Contains(v.Code))
            //    .ToListAsync(cancellationToken);

            //return existingCountrys.Adapt<List<CountryDto>>();

            return [];
        }

        public async Task<bool> Handle(ValidateWellnessProgramSession request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<WellnessProgramSession>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<WellnessProgramSessionDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetWellnessProgramSessionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramSession>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramSession>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramSession>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //        EF.Functions.Like(v.WellnessProgramSession.Name, $"%{request.SearchTerm}%")
                    //        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramSession
                    {
                        Id = x.Id,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<WellnessProgramSessionDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<WellnessProgramSessionDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<WellnessProgramSessionDto> Handle(GetSingleWellnessProgramSessionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<WellnessProgramSession>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<WellnessProgramSession>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<WellnessProgramSession>)query).ThenBy(additionalOrderBy.OrderBy);
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
                    //    EF.Functions.Like(v.WellnessProgramSession.Name, $"%{request.SearchTerm}%")
                    //    );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new WellnessProgramSession
                    {
                        Id = x.Id,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<WellnessProgramSessionDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<WellnessProgramSessionDto> Handle(CreateWellnessProgramSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramSession>().AddAsync(request.WellnessProgramSessionDto.Adapt<CreateUpdateWellnessProgramSessionDto>().Adapt<WellnessProgramSession>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramSessionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramSessionDto>> Handle(CreateListWellnessProgramSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramSession>().AddAsync(request.WellnessProgramSessionDtos.Adapt<List<WellnessProgramSession>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramSessionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<WellnessProgramSessionDto> Handle(UpdateWellnessProgramSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramSession>().UpdateAsync(request.WellnessProgramSessionDto.Adapt<WellnessProgramSessionDto>().Adapt<WellnessProgramSession>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<WellnessProgramSessionDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WellnessProgramSessionDto>> Handle(UpdateListWellnessProgramSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<WellnessProgramSession>().UpdateAsync(request.WellnessProgramSessionDtos.Adapt<List<WellnessProgramSession>>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<WellnessProgramSessionDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteWellnessProgramSessionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramSession>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<WellnessProgramSession>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetWellnessProgramSessionQuery_"); // Ganti dengan key yang sesuai

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