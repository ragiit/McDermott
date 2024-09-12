using static McDermott.Application.Features.Commands.Medical.DiseaseCategoryCommand;

namespace McDermott.Application.Features.Queries.Medical
{
    public class DiseaseCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetDiseaseCategoryQuery, List<DiseaseCategoryDto>>,
        IRequestHandler<CreateDiseaseCategoryRequest, DiseaseCategoryDto>,
        IRequestHandler<CreateListDiseaseCategoryRequest, List<DiseaseCategoryDto>>,
        IRequestHandler<UpdateDiseaseCategoryRequest, DiseaseCategoryDto>,
        IRequestHandler<UpdateListDiseaseCategoryRequest, List<DiseaseCategoryDto>>,
        IRequestHandler<DeleteDiseaseCategoryRequest, bool>
    {
        #region GET

        public async Task<List<DiseaseCategoryDto>> Handle(GetDiseaseCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetDiseaseCategoryQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<DiseaseCategory>? result))
                {
                    result = await _unitOfWork.Repository<DiseaseCategory>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<DiseaseCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<DiseaseCategoryDto> Handle(CreateDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().AddAsync(request.DiseaseCategoryDto.Adapt<DiseaseCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiseaseCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiseaseCategoryDto>> Handle(CreateListDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().AddAsync(request.DiseaseCategoryDtos.Adapt<List<DiseaseCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiseaseCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<DiseaseCategoryDto> Handle(UpdateDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().UpdateAsync(request.DiseaseCategoryDto.Adapt<DiseaseCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<DiseaseCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DiseaseCategoryDto>> Handle(UpdateListDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<DiseaseCategory>().UpdateAsync(request.DiseaseCategoryDtos.Adapt<List<DiseaseCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<DiseaseCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteDiseaseCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<DiseaseCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<DiseaseCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetDiseaseCategoryQuery_"); // Ganti dengan key yang sesuai

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