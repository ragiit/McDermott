namespace McHealthCare.Application.Features.Queries.Inventory
{
    public class InventoryAdjusmentDetailQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetInventoryAdjusmentDetailQuery, List<InventoryAdjusmentDetailDto>>,
        IRequestHandler<CreateInventoryAdjusmentDetailRequest, InventoryAdjusmentDetailDto>,
        IRequestHandler<CreateListInventoryAdjusmentDetailRequest, List<InventoryAdjusmentDetailDto>>,
        IRequestHandler<UpdateInventoryAdjusmentDetailRequest, InventoryAdjusmentDetailDto>,
        IRequestHandler<UpdateListInventoryAdjusmentDetailRequest, List<InventoryAdjusmentDetailDto>>,
        IRequestHandler<DeleteInventoryAdjusmentDetailRequest, bool>
    {
        #region GET

        public async Task<List<InventoryAdjusmentDetailDto>> Handle(GetInventoryAdjusmentDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetInventoryAdjusmentDetailQuery_"; // Gunakan nilai Predicate dalam pembuatan kunci cache &&  harus Unique

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<InventoryAdjusmentDetail>? result))
                {
                    result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().Entities
                        .Include(x => x.InventoryAdjusment)
                        .Include(x => x.Product)
                        .ThenInclude(z => z.Uom)
                        .Include(x => x.StockProduct)
                        .Include(x => x.TransactionStock)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10)); // Simpan data dalam cache selama 10 menit
                }

                result ??= [];

                // Filter result based on request.Predicate if it's not null
                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<InventoryAdjusmentDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region CREATE

        public async Task<InventoryAdjusmentDetailDto> Handle(CreateInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().AddAsync(request.InventoryAdjusmentDetailDto.Adapt<InventoryAdjusmentDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjusmentDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjusmentDetailDto>> Handle(CreateListInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().AddAsync(request.InventoryAdjusmentDetailDtos.Adapt<List<InventoryAdjusmentDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjusmentDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region UPDATE

        public async Task<InventoryAdjusmentDetailDto> Handle(UpdateInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().UpdateAsync(request.InventoryAdjusmentDetailDto.Adapt<InventoryAdjusmentDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<InventoryAdjusmentDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<InventoryAdjusmentDetailDto>> Handle(UpdateListInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<InventoryAdjusmentDetail>().UpdateAsync(request.InventoryAdjusmentDetailDtos.Adapt<List<InventoryAdjusmentDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<InventoryAdjusmentDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region DELETE

        public async Task<bool> Handle(DeleteInventoryAdjusmentDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id != Guid.Empty)
                {
                    await _unitOfWork.Repository<InventoryAdjusmentDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<InventoryAdjusmentDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetInventoryAdjusmentDetailQuery_"); // Ganti dengan key yang sesuai

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