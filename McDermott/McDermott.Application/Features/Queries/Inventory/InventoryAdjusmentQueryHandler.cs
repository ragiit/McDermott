namespace McDermott.Application.Features.Queries.Inventory
{
    public class InventoryAdjusmentQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInventoryAdjusmentQuery, List<InventoryAdjusmentDto>>,
        IRequestHandler<CreateInventoryAdjusmentRequest, InventoryAdjusmentDto>,
        IRequestHandler<CreateListInventoryAdjusmentRequest, List<InventoryAdjusmentDto>>,
        IRequestHandler<GetInventoryAdjusmentLogQuery, List<InventoryAdjustmentLogDto>>,
        IRequestHandler<CreateInventoryAdjusmentLogRequest, InventoryAdjustmentLogDto>,
        IRequestHandler<CreateListInventoryAdjusmentLogRequest, List<InventoryAdjustmentLogDto>>,
        IRequestHandler<UpdateInventoryAdjusmentRequest, InventoryAdjusmentDto>,
        IRequestHandler<UpdateListInventoryAdjusmentRequest, List<InventoryAdjusmentDto>>,
        IRequestHandler<DeleteInventoryAdjusmentRequest, bool>
    {
        #region GET

        public async Task<List<InventoryAdjusmentDto>> Handle(GetInventoryAdjusmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetInventoryAdjusmentQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<InventoryAdjusment>? result))
                {
                    result = await _unitOfWork.Repository<InventoryAdjusment>().Entities
                       .Include(x => x.Location)
                       .Include(x => x.Company)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<InventoryAdjusmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjustmentLogDto>> Handle(GetInventoryAdjusmentLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetInventoryAdjusmentLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<InventoryAdjusmentLog>? result))
                {
                    result = await _unitOfWork.Repository<InventoryAdjusmentLog>().Entities
                       .Include(x => x.InventoryAdjusment)
                       .Include(x => x.UserBy)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<InventoryAdjustmentLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<InventoryAdjusmentDto> Handle(CreateInventoryAdjusmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusment>().AddAsync(request.InventoryAdjusmentDto.Adapt<InventoryAdjusment>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjusmentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjusmentDto>> Handle(CreateListInventoryAdjusmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusment>().AddAsync(request.InventoryAdjusmentDtos.Adapt<List<InventoryAdjusment>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjusmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Log
        public async Task<InventoryAdjustmentLogDto> Handle(CreateInventoryAdjusmentLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentLog>().AddAsync(request.InventoryAdjusmentLogDto.Adapt<InventoryAdjusmentLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjustmentLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjustmentLogDto>> Handle(CreateListInventoryAdjusmentLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentLog>().AddAsync(request.InventoryAdjusmentLogDtos.Adapt<List<InventoryAdjusmentLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjustmentLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion CREATE

        #region UPDATE

        public async Task<InventoryAdjusmentDto> Handle(UpdateInventoryAdjusmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusment>().UpdateAsync(request.InventoryAdjusmentDto.Adapt<InventoryAdjusment>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjusmentDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjusmentDto>> Handle(UpdateListInventoryAdjusmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusment>().UpdateAsync(request.InventoryAdjusmentDtos.Adapt<List<InventoryAdjusment>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjusmentDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInventoryAdjusmentRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<InventoryAdjusment>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<InventoryAdjusment>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentQuery_"); // Ganti dengan key yang sesuai

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
