using static McDermott.Application.Features.Commands.BpjsIntegration.AwarenessCommand;

namespace McDermott.Application.Features.Queries.BpjsIntegration
{
    public class AwarenessQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetAwarenessQuery, List<AwarenessDto>>,
        IRequestHandler<CreateAwarenessRequest, AwarenessDto>,
        IRequestHandler<CreateListAwarenessRequest, List<AwarenessDto>>,
        IRequestHandler<UpdateAwarenessRequest, AwarenessDto>,
        IRequestHandler<UpdateListAwarenessRequest, List<AwarenessDto>>,
        IRequestHandler<UpdateToDbAwarenessRequest, List<AwarenessDto>>,
        IRequestHandler<DeleteAwarenessRequest, bool>
    {
        #region GET

        public async Task<List<AwarenessDto>> Handle(GetAwarenessQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetAwarenessQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<Awareness>? result))
                {
                    result = await _unitOfWork.Repository<Awareness>().Entities
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<AwarenessDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<AwarenessDto> Handle(CreateAwarenessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Awareness>().AddAsync(request.AwarenessDto.Adapt<Awareness>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessQuery_");

                return result.Adapt<AwarenessDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AwarenessDto>> Handle(CreateListAwarenessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Awareness>().AddAsync(request.AwarenessDtos.Adapt<List<Awareness>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessQuery_");

                return result.Adapt<List<AwarenessDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<AwarenessDto> Handle(UpdateAwarenessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Awareness>().UpdateAsync(request.AwarenessDto.Adapt<Awareness>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessQuery_");

                return result.Adapt<AwarenessDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AwarenessDto>> Handle(UpdateListAwarenessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<Awareness>().UpdateAsync(request.AwarenessDtos.Adapt<List<Awareness>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessQuery_");

                return result.Adapt<List<AwarenessDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AwarenessDto>> Handle(UpdateToDbAwarenessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.Repository<Awareness>().DeleteAsync(true);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var result = await _unitOfWork.Repository<Awareness>().AddAsync(request.AwarenessDtos.Adapt<List<Awareness>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessQuery_");

                return result.Adapt<List<AwarenessDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteAwarenessRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<Awareness>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<Awareness>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetAwarenessQuery_"); // Ganti dengan key yang sesuai

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