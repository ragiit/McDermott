

using static McDermott.Application.Features.Commands.Pharmacy.ActiveComponentCommand;

namespace McDermott.Application.Features.Queries.Pharmacy
{
    public class ActiveComponentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetActiveComponentQuery, List<ActiveComponentDto>>,
        IRequestHandler<CreateActiveComponentRequest, ActiveComponentDto>,
        IRequestHandler<CreateListActiveComponentRequest, List<ActiveComponentDto>>,
        IRequestHandler<UpdateActiveComponentRequest, ActiveComponentDto>,
        IRequestHandler<UpdateListActiveComponentRequest, List<ActiveComponentDto>>,
        IRequestHandler<DeleteActiveComponentRequest, bool>
    {
        #region GET

        public async Task<List<ActiveComponentDto>> Handle(GetActiveComponentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetActiveComponentQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ActiveComponent>? result))
                {
                    result = await _unitOfWork.Repository<ActiveComponent>().Entities
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ActiveComponentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<ActiveComponentDto> Handle(CreateActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().AddAsync(request.ActiveComponentDto.Adapt<ActiveComponent>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ActiveComponentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ActiveComponentDto>> Handle(CreateListActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().AddAsync(request.ActiveComponentDtos.Adapt<List<ActiveComponent>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ActiveComponentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<ActiveComponentDto> Handle(UpdateActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().UpdateAsync(request.ActiveComponentDto.Adapt<ActiveComponent>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ActiveComponentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ActiveComponentDto>> Handle(UpdateListActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ActiveComponent>().UpdateAsync(request.ActiveComponentDtos.Adapt<List<ActiveComponent>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ActiveComponentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteActiveComponentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ActiveComponent>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ActiveComponent>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetActiveComponentQuery_"); // Ganti dengan key yang sesuai

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
