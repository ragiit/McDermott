using static McHealthCare.Application.Features.CommandsQueries.Inventory.UomCategoryCommand;

namespace McHealthCare.Application.Features.Queries.Inventory
{
    public class UomCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetUomCategoryQuery, List<UomCategoryDto>>,
        IRequestHandler<CreateUomCategoryRequest, UomCategoryDto>,
        IRequestHandler<CreateListUomCategoryRequest, List<UomCategoryDto>>,
        IRequestHandler<UpdateUomCategoryRequest, UomCategoryDto>,
        IRequestHandler<UpdateListUomCategoryRequest, List<UomCategoryDto>>,
        IRequestHandler<DeleteUomCategoryRequest, bool>
    {
        #region GET

        public async Task<List<UomCategoryDto>> Handle(GetUomCategoryQuery request, CancellationToken cancellationToken)
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
                if (request.Id != Guid.Empty)
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