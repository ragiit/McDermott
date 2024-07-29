


using static McDermott.Application.Features.Commands.Inventory.ReceivingCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class ReceivingQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache):
        IRequestHandler<GetReceivingStockQuery, List<ReceivingStockDto>>,
        IRequestHandler<CreateReceivingStockRequest, ReceivingStockDto>,
        IRequestHandler<CreateListReceivingStockRequest, List<ReceivingStockDto>>,
        IRequestHandler<UpdateReceivingStockRequest, ReceivingStockDto>,
        IRequestHandler<UpdateListReceivingStockRequest, List<ReceivingStockDto>>,
        IRequestHandler<DeleteReceivingStockRequest, bool>,
        IRequestHandler<GetReceivingStockProductQuery, List<ReceivingStockProductDto>>,
        IRequestHandler<CreateReceivingStockProductRequest, ReceivingStockProductDto>,
        IRequestHandler<CreateListReceivingStockProductRequest, List<ReceivingStockProductDto>>,
        IRequestHandler<UpdateReceivingStockProductRequest, ReceivingStockProductDto>,
        IRequestHandler<UpdateListReceivingStockProductRequest, List<ReceivingStockProductDto>>,
        IRequestHandler<DeleteReceivingStockPoductRequest, bool>,
        IRequestHandler<GetReceivingLogQuery, List<ReceivingLogDto>>,
        IRequestHandler<CreateReceivingLogRequest, ReceivingLogDto>,
        IRequestHandler<CreateListReceivingLogRequest, List<ReceivingLogDto>>,
        IRequestHandler<UpdateReceivingLogRequest, ReceivingLogDto>,
        IRequestHandler<UpdateListReceivingLogRequest, List<ReceivingLogDto>>,
        IRequestHandler<DeleteReceivingLogRequest, bool>
    {
        #region GET Receiving Stock Product

        public async Task<List<ReceivingStockProductDto>> Handle(GetReceivingStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetReceivingStockProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ReceivingStockProduct>? result))
                {
                    result = await _unitOfWork.Repository<ReceivingStockProduct>().Entities
                      .Include(x => x.Product)
                      .Include(x=>x.ReceivingStock)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= new List<ReceivingStockProduct>(); ;

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ReceivingStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Receiving Stock Product

        #region GET Receiving Stock

        public async Task<List<ReceivingStockDto>> Handle(GetReceivingStockQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetReceivingStockQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ReceivingStock>? result))
                {
                    result = await _unitOfWork.Repository<ReceivingStock>().Entities
                      .Include(x => x.Destination)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ReceivingStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Receiving Stock

        #region GET Receiving Log

        public async Task<List<ReceivingLogDto>> Handle(GetReceivingLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetReceivingLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<ReceivingLog>? result))
                {
                    result = await _unitOfWork.Repository<ReceivingLog>().Entities
                      .Include(x => x.Source)
                      .Include(x => x.UserBy)
                      .Include(x => x.Receiving)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<ReceivingLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Receiving Stock

        #region CREATE Receiving Stock Product

        public async Task<ReceivingStockProductDto> Handle(CreateReceivingStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStockProduct>().AddAsync(request.ReceivingStockProductDto.Adapt<ReceivingStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockProductQuery_"); // Ganti dengan key yang sesuai 

                return result.Adapt<ReceivingStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReceivingStockProductDto>> Handle(CreateListReceivingStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStockProduct>().AddAsync(request.ReceivingStockProductDtos.Adapt<List<ReceivingStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReceivingStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Receiving Stock Product

        #region CREATE Receiving Stock

        public async Task<ReceivingStockDto> Handle(CreateReceivingStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStock>().AddAsync(request.ReceivingStockDto.Adapt<ReceivingStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReceivingStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReceivingStockDto>> Handle(CreateListReceivingStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStock>().AddAsync(request.ReceivingStockDtos.Adapt<List<ReceivingStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReceivingStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Receiving Stock

        #region CREATE Receiving Log

        public async Task<ReceivingLogDto> Handle(CreateReceivingLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingLog>().AddAsync(request.ReceivingLogDto.Adapt<ReceivingLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReceivingLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReceivingLogDto>> Handle(CreateListReceivingLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingLog>().AddAsync(request.ReceivingLogDtos.Adapt<List<ReceivingLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReceivingLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Receiving Stock

        #region UPDATE Receiving Stock Detail

        public async Task<ReceivingStockProductDto> Handle(UpdateReceivingStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStockProduct>().UpdateAsync(request.ReceivingStockProductDto.Adapt<ReceivingStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockDetailQuery_"); // Ganti dengan key yang sesuai
                _cache.Remove("GetReceivingStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReceivingStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReceivingStockProductDto>> Handle(UpdateListReceivingStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStockProduct>().UpdateAsync(request.ReceivingStockProductDtos.Adapt<List<ReceivingStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReceivingStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Receiving Stock Detail

        #region UPDATE Receiving Stock

        public async Task<ReceivingStockDto> Handle(UpdateReceivingStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStock>().UpdateAsync(request.ReceivingStockDto.Adapt<ReceivingStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReceivingStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReceivingStockDto>> Handle(UpdateListReceivingStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingStock>().UpdateAsync(request.ReceivingStockDtos.Adapt<List<ReceivingStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReceivingStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Receiving Stock

        #region UPDATE Receiving Log

        public async Task<ReceivingLogDto> Handle(UpdateReceivingLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingLog>().UpdateAsync(request.ReceivingLogDto.Adapt<ReceivingLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<ReceivingLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ReceivingLogDto>> Handle(UpdateListReceivingLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<ReceivingLog>().UpdateAsync(request.ReceivingLogDtos.Adapt<List<ReceivingLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<ReceivingLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Receiving Stock

        #region DELETE Receiving Stock Detail

        public async Task<bool> Handle(DeleteReceivingStockPoductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ReceivingStockProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ReceivingStockProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockDetailQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Receiving Stock Detail

        #region DELETE Receiving Stock

        public async Task<bool> Handle(DeleteReceivingStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ReceivingStock>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ReceivingStock>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingStockQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Receiving Stock

        #region DELETE Receiving Log

        public async Task<bool> Handle(DeleteReceivingLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<ReceivingLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<ReceivingLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetReceivingLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Receiving Stock
    }
}
