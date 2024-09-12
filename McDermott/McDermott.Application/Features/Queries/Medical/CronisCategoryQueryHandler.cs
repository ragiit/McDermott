namespace McDermott.Application.Features.Queries.Medical
{
    public class CronisCategoryQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetCronisCategoryQuery, List<CronisCategoryDto>>,
        IRequestHandler<CreateCronisCategoryRequest, CronisCategoryDto>,
        IRequestHandler<CreateListCronisCategoryRequest, List<CronisCategoryDto>>,
        IRequestHandler<UpdateCronisCategoryRequest, CronisCategoryDto>,
        IRequestHandler<UpdateListCronisCategoryRequest, List<CronisCategoryDto>>,
        IRequestHandler<DeleteCronisCategoryRequest, bool>
    {
        #region GET

        public async Task<List<CronisCategoryDto>> Handle(GetCronisCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetCronisCategoryQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<CronisCategory>? result))
                {
                    result = await _unitOfWork.Repository<CronisCategory>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<CronisCategoryDto> Handle(CreateCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<CronisCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CronisCategoryDto>> Handle(CreateListCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().AddAsync(request.CronisCategoryDtos.Adapt<List<CronisCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<CronisCategoryDto> Handle(UpdateCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDto.Adapt<CronisCategory>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<CronisCategoryDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CronisCategoryDto>> Handle(UpdateListCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<CronisCategory>().UpdateAsync(request.CronisCategoryDtos.Adapt<List<CronisCategory>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

                return result.Adapt<List<CronisCategoryDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteCronisCategoryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<CronisCategory>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<CronisCategory>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetCronisCategoryQuery_");

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