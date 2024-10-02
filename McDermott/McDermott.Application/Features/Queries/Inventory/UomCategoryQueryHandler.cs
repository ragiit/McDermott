using McDermott.Application.Features.Services;
using static McDermott.Application.Features.Commands.Inventory.UomCategoryCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class UomCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetUomCategoryQuery, (List<UomCategoryDto>, int pageIndex, int pageSize, int pageCount)>,
        IRequestHandler<GetAllUomCategoryQuery, List<UomCategoryDto>>,
        IRequestHandler<BulkValidateUomCategoryQuery, List<UomCategoryDto>>,
        IRequestHandler<ValidateUomCategoryQuery, bool>,
        IRequestHandler<CreateUomCategoryRequest, UomCategoryDto>,
        IRequestHandler<CreateListUomCategoryRequest, List<UomCategoryDto>>,
        IRequestHandler<UpdateUomCategoryRequest, UomCategoryDto>,
        IRequestHandler<UpdateListUomCategoryRequest, List<UomCategoryDto>>,
        IRequestHandler<DeleteUomCategoryRequest, bool>
    {
        #region GET

        public async Task<List<UomCategoryDto>> Handle(GetAllUomCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetUomCategoryQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<UomCategory>? result))
                {
                    result = await _unitOfWork.Repository<UomCategory>().Entities
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<UomCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<(List<UomCategoryDto>, int pageIndex, int pageSize, int pageCount)> Handle(GetUomCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _unitOfWork.Repository<UomCategory>().Entities.AsNoTracking();

                // Apply dynamic includes
                if (request.Includes is not null)
                {
                    foreach (var includeExpression in request.Includes)
                    {
                        query = query.Include(includeExpression);
                    }
                }

                if (request.Predicate is not null)
                    query = query.Where(request.Predicate);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    query = query.Where(v =>
                        EF.Functions.Like(v.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.Like(v.Type, $"%{request.SearchTerm}%")
                        );
                }

                // Apply dynamic select if provided
                if (request.Select is not null)
                {
                    query = query.Select(request.Select);
                }

                var (totalCount, pagedItems, totalPages) = await PaginateAsyncClass.PaginateAndSortAsync(
                                  query,
                                  request.PageSize,
                                  request.PageIndex,
                                  q => q.OrderBy(x => x.Name), // Custom order by bisa diterapkan di sini
                                  cancellationToken);

                return (pagedItems.Adapt<List<UomCategoryDto>>(), request.PageIndex, request.PageSize, totalPages);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomCategoryDto>> Handle(BulkValidateUomCategoryQuery request, CancellationToken cancellationToken)
        {
            var UomsCategory = request.UomCategoryToValidate;

            // Ekstrak semua kombinasi yang akan dicari di database
            var A = UomsCategory.Select(x => x.Name).Distinct().ToList();
            var B = UomsCategory.Select(x => x.Type).Distinct().ToList();
            var existingLabTests = await _unitOfWork.Repository<UomCategory>()
                .Entities
                .AsNoTracking()
                .Where(v => A.Contains(v.Name)
                            && B.Contains(v.Type)
                            )
                .ToListAsync(cancellationToken);

            return existingLabTests.Adapt<List<UomCategoryDto>>();
        }

        public async Task<bool> Handle(ValidateUomCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<UomCategory>()
                .Entities
                .AsNoTracking()
                .Where(request.Predicate)  // Apply the Predicate for filtering
                .AnyAsync(cancellationToken);  // Check if any record matches the condition
        }

        #endregion GET

        #region CREATE

        public async Task<UomCategoryDto> Handle(CreateUomCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<UomCategory>().AddAsync(request.UomCategoryDto.Adapt<UomCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<UomCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomCategoryDto>> Handle(CreateListUomCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<UomCategory>().AddAsync(request.UomCategoryDtos.Adapt<List<UomCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UomCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<UomCategoryDto> Handle(UpdateUomCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<UomCategory>().UpdateAsync(request.UomCategoryDto.Adapt<UomCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<UomCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UomCategoryDto>> Handle(UpdateListUomCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<UomCategory>().UpdateAsync(request.UomCategoryDtos.Adapt<List<UomCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<UomCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteUomCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<UomCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<UomCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetUomCategoryQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetUomQuery_");

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