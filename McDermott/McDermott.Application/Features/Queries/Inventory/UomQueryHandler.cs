using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.UomCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class UomQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
      IRequestHandler<GetUomQuery, (List<UomDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleUomQuery, UomDto>,
        IRequestHandler<GetAllUomQuery, List<UomDto>>,
        IRequestHandler<BulkValidateUomQuery, List<UomDto>>,
        IRequestHandler<ValidateUomQuery, bool>,
        IRequestHandler<CreateUomRequest, UomDto>,
        IRequestHandler<CreateListUomRequest, List<UomDto>>,
        IRequestHandler<UpdateUomRequest, UomDto>,
        IRequestHandler<UpdateListUomRequest, List<UomDto>>,
        IRequestHandler<DeleteUomRequest, bool>
    {
        #region GET

        public async Task<List<UomDto>> Handle(GetAllUomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetUomQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Uom>? result))
                {
                    result = await _unitOfWork.Repository<Uom>().Entities
                        .Include(x => x.UomCategory)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<UomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<(List<UomDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetUomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Uom>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Uom>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Uom>)query).ThenBy(additionalOrderBy.OrderBy);
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
                       EF.Functions.Like(v.UomCategory.Name, $"%{request.SearchTerm}%") ||
                       EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                       v.BiggerRatio.Equals(request.SearchTerm) ||
                       v.RoundingPrecision.Equals(request.SearchTerm)
                       );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Uom
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UomCategoryId = x.UomCategoryId,
                        UomCategory = new UomCategory
                        {
                            Name = x.UomCategory == null ? "" : x.UomCategory.Name,
                        }, 
                        Active = x.Active,
                        BiggerRatio = x.BiggerRatio,
                        RoundingPrecision = x.RoundingPrecision,
                        Type = x.Type,
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<UomDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<UomDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<UomDto> Handle(GetSingleUomQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<Uom>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<Uom>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<Uom>)query).ThenBy(additionalOrderBy.OrderBy);
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
                       EF.Functions.Like(v.UomCategory.Name, $"%{request.SearchTerm}%") ||
                       EF.Functions.Like(v.Type, $"%{request.SearchTerm}%") ||
                       v.BiggerRatio.Equals(request.SearchTerm) ||
                       v.RoundingPrecision.Equals(request.SearchTerm)
                       );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new Uom
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UomCategoryId = x.UomCategoryId,
                        UomCategory = new UomCategory
                        {
                            Name = x.UomCategory == null ? "" : x.UomCategory.Name,
                        }, 
                        Active = x.Active,
                        BiggerRatio = x.BiggerRatio,
                        RoundingPrecision = x.RoundingPrecision,
                        Type = x.Type,

                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<UomDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<List<UomDto>> Handle(BulkValidateUomQuery request, CancellationToken cancellationToken)
        {
            var Uoms = request.UomToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var A = Uoms.Select(x => x.Name).Distinct().ToList();
            var B = Uoms.Select(x => x.Type).Distinct().ToList();
            var C = Uoms.Select(x => x.UomCategoryId).Distinct().ToList();

            var existingLabTests = await _unitOfWork.Repository<Uom>()
                .Entities
                .AsNoTracking()
                .Where(v => A.Contains(v.Name)
                            && B.Contains(v.Type)
                            && C.Contains(v.UomCategoryId)
                            )
                .ToListAsync(cancellationToken);

            return existingLabTests.Adapt<List<UomDto>>();
        }

        public async Task<bool> Handle(ValidateUomQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Uom>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<UomDto> Handle(CreateUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDto.Adapt<Uom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<UomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomDto>> Handle(CreateListUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().AddAsync(request.UomDtos.Adapt<List<Uom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<UomDto> Handle(UpdateUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDto.Adapt<Uom>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<UomDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomDto>> Handle(UpdateListUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Uom>().UpdateAsync(request.UomDtos.Adapt<List<Uom>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UomDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteUomRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<Uom>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Uom>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomQuery_"); // Ganti dengan key yang sesuai

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