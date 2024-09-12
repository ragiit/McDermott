

namespace McDermott.Application.Features.Queries.Medical
{
    public class HealthCenterQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetHealthCenterQuery, List<HealthCenterDto>>,
        IRequestHandler<CreateHealthCenterRequest, HealthCenterDto>,
        IRequestHandler<CreateListHealthCenterRequest, List<HealthCenterDto>>,
        IRequestHandler<UpdateHealthCenterRequest, HealthCenterDto>,
        IRequestHandler<UpdateListHealthCenterRequest, List<HealthCenterDto>>,
        IRequestHandler<DeleteHealthCenterRequest, bool>
    {
        #region GET

        public async Task<List<HealthCenterDto>> Handle(GetHealthCenterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetHealthCenterQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<HealthCenter>? result))
                {
                    result = await _unitOfWork.Repository<HealthCenter>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<HealthCenterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<HealthCenterDto> Handle(CreateHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().AddAsync(request.HealthCenterDto.Adapt<HealthCenter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<HealthCenterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<HealthCenterDto>> Handle(CreateListHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().AddAsync(request.HealthCenterDtos.Adapt<List<HealthCenter>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<List<HealthCenterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<HealthCenterDto> Handle(UpdateHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().UpdateAsync(request.HealthCenterDto.Adapt<HealthCenter>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<HealthCenterDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<HealthCenterDto>> Handle(UpdateListHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<HealthCenter>().UpdateAsync(request.HealthCenterDtos.Adapt<List<HealthCenter>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

                return result.Adapt<List<HealthCenterDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteHealthCenterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<HealthCenter>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<HealthCenter>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetHealthCenterQuery_");

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