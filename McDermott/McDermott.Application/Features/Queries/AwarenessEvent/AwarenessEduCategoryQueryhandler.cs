using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Features.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;

namespace McDermott.Application.Features.Queries.AwarenessEvent
{
    public class AwarenessEduCategoryQueryhandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAllAwarenessEduCategoryQuery, List<AwarenessEduCategoryDto>>,//AwarenessEduCategory
        IRequestHandler<GetAwarenessEduCategoryQuery, (List<AwarenessEduCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetSingleAwarenessEduCategoryQuery, AwarenessEduCategoryDto>, IRequestHandler<ValidateAwarenessEduCategoryQuery, bool>,
        IRequestHandler<BulkValidateAwarenessEduCategoryQuery, List<AwarenessEduCategoryDto>>,
        IRequestHandler<CreateAwarenessEduCategoryRequest, AwarenessEduCategoryDto>,
        IRequestHandler<CreateListAwarenessEduCategoryRequest, List<AwarenessEduCategoryDto>>,
        IRequestHandler<UpdateAwarenessEduCategoryRequest, AwarenessEduCategoryDto>,
        IRequestHandler<UpdateListAwarenessEduCategoryRequest, List<AwarenessEduCategoryDto>>,
        IRequestHandler<DeleteAwarenessEduCategoryRequest, bool>
    {
        #region GET Education Program

        public async Task<List<AwarenessEduCategoryDto>> Handle(GetAllAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAllAwarenessEduCategoryQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<AwarenessEduCategory>? result))
                {
                    result = await _unitOfWork.Repository<AwarenessEduCategory>().Entities

                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<AwarenessEduCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AwarenessEduCategoryDto>> Handle(BulkValidateAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
        {
            var AwarenessEduCategoryDtos = request.AwarenessEduCategoryToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var AwarenessEduCategoryNames = AwarenessEduCategoryDtos.Select(x => x.Name).Distinct().ToList();

            var existingAwarenessEduCategorys = await _unitOfWork.Repository<AwarenessEduCategory>()
                .Entities
                .AsNoTracking()
                .Where(v => AwarenessEduCategoryNames.Contains(v.Name))
                .ToListAsync(cancellationToken);

            return existingAwarenessEduCategorys.Adapt<List<AwarenessEduCategoryDto>>();
        }

        public async Task<bool> Handle(ValidateAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<AwarenessEduCategory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        public async Task<(List<AwarenessEduCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<AwarenessEduCategory>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<AwarenessEduCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<AwarenessEduCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new AwarenessEduCategory
                    {
                        Id = x.Id,
                        Name = x.Name
                    });

                if (!request.IsGetAll)
                { // Paginate and sort
                    var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                        query,
                        request.PageSize,
                        request.PageIndex,
                        cancellationToken
                    );

                    return (pagedItems.Adapt<List<AwarenessEduCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
                }
                else
                {
                    return ((await query.ToListAsync(cancellationToken)).Adapt<List<AwarenessEduCategoryDto>>(), 0, 1, 1);
                }
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        public async Task<AwarenessEduCategoryDto> Handle(GetSingleAwarenessEduCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<AwarenessEduCategory>().Entities.AsNoTracking();

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
                            ? ((IOrderedQueryable<AwarenessEduCategory>)query).ThenByDescending(additionalOrderBy.OrderBy)
                            : ((IOrderedQueryable<AwarenessEduCategory>)query).ThenBy(additionalOrderBy.OrderBy);
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
                            EF.Functions.Like(v.Name, $"%{request.SearchTerm}%"));
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                    query = query.Select(request.Select);
                else
                    query = query.Select(x => new AwarenessEduCategory
                    {
                        Id = x.Id,
                        Name = x.Name,
                    });

                return (await query.FirstOrDefaultAsync(cancellationToken)).Adapt<AwarenessEduCategoryDto>();
            }
            catch (Exception ex)
            {
                // Consider logging the exception
                throw;
            }
        }

        #endregion GET Education Program

        #region CREATE Education Program

        public async Task<AwarenessEduCategoryDto> Handle(CreateAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<AwarenessEduCategory>().AddAsync(request.AwarenessEduCategoryDto.Adapt<AwarenessEduCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<AwarenessEduCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AwarenessEduCategoryDto>> Handle(CreateListAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<AwarenessEduCategory>().AddAsync(request.AwarenessEduCategoryDtos.Adapt<List<AwarenessEduCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<AwarenessEduCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Education Program

        #region UPDATE Education Program

        public async Task<AwarenessEduCategoryDto> Handle(UpdateAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<AwarenessEduCategory>().UpdateAsync(request.AwarenessEduCategoryDto.Adapt<AwarenessEduCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<AwarenessEduCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AwarenessEduCategoryDto>> Handle(UpdateListAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<AwarenessEduCategory>().UpdateAsync(request.AwarenessEduCategoryDtos.Adapt<List<AwarenessEduCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<AwarenessEduCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Education Program

        #region DELETE Education Program

        public async Task<bool> Handle(DeleteAwarenessEduCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<AwarenessEduCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<AwarenessEduCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessEduCategoryQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Education Program
    }
}