using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class TransferStockQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetTransferStockQuery, List<TransferStockDto>>,
        IRequestHandler<CreateTransferStockRequest, TransferStockDto>,
        IRequestHandler<CreateListTransferStockRequest, List<TransferStockDto>>,
        IRequestHandler<UpdateTransferStockRequest, TransferStockDto>,
        IRequestHandler<UpdateListTransferStockRequest, List<TransferStockDto>>,
        IRequestHandler<DeleteTransferStockRequest, bool>,
        IRequestHandler<GetTransferStockProductQuery, List<TransferStockProductDto>>,
        IRequestHandler<CreateTransferStockProductRequest, TransferStockProductDto>,
        IRequestHandler<CreateListTransferStockProductRequest, List<TransferStockProductDto>>,
        IRequestHandler<UpdateTransferStockProductRequest, TransferStockProductDto>,
        IRequestHandler<UpdateListTransferStockProductRequest, List<TransferStockProductDto>>,
        IRequestHandler<DeleteTransferStockProductRequest, bool>,
        IRequestHandler<GetTransferStockLogQuery, List<TransferStockLogDto>>,
        IRequestHandler<CreateTransferStockLogRequest, TransferStockLogDto>,
        IRequestHandler<CreateListTransferStockLogRequest, List<TransferStockLogDto>>,
        IRequestHandler<UpdateTransferStockLogRequest, TransferStockLogDto>,
        IRequestHandler<UpdateListTransferStockLogRequest, List<TransferStockLogDto>>,
        IRequestHandler<DeleteTransferStockLogRequest, bool>
        
    {
        #region GET

        public async Task<List<TransferStockDto>> Handle(GetTransferStockQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTransferStockQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransferStock>? result))
                {
                    result = await _unitOfWork.Repository<TransferStock>().Entities
                      .Include(x => x.Source)
                      .Include(x => x.Destination)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransferStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region GET Product

        public async Task<List<TransferStockProductDto>> Handle(GetTransferStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTransferStockProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransferStockProduct>? result))
                {
                    result = await _unitOfWork.Repository<TransferStockProduct>().Entities

                      .Include(x => x.Product)
                      .Include(x => x.TransferStock)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= new List<TransferStockProduct>();

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransferStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Product

        #region GET Detail

        public async Task<List<TransferStockLogDto>> Handle(GetTransferStockLogQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTransferStockLogQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransferStockLog>? result))
                {
                    result = await _unitOfWork.Repository<TransferStockLog>().Entities
                      .Include(x => x.TransferStock)
                      .Include(x => x.Source)
                      .Include(x => x.Destination)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransferStockLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Detail

        #region CREATE

        public async Task<TransferStockDto> Handle(CreateTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().AddAsync(request.TransferStockDto.Adapt<TransferStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockDto>> Handle(CreateListTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().AddAsync(request.TransferStockDtos.Adapt<List<TransferStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region CREATE Product

        public async Task<TransferStockProductDto> Handle(CreateTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().AddAsync(request.TransferStockProductDto.Adapt<TransferStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockProductDto>> Handle(CreateListTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().AddAsync(request.TransferStockProductDtos.Adapt<List<TransferStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Product

        #region CREATE Detail

        public async Task<TransferStockLogDto> Handle(CreateTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().AddAsync(request.TransferStockLogDto.Adapt<TransferStockLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockLogDto>> Handle(CreateListTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().AddAsync(request.TransferStockLogDtos.Adapt<List<TransferStockLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Detail

        #region UPDATE

        public async Task<TransferStockDto> Handle(UpdateTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().UpdateAsync(request.TransferStockDto.Adapt<TransferStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockDto>> Handle(UpdateListTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStock>().UpdateAsync(request.TransferStockDtos.Adapt<List<TransferStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region UPDATE Product

        public async Task<TransferStockProductDto> Handle(UpdateTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().UpdateAsync(request.TransferStockProductDto.Adapt<TransferStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockProductDto>> Handle(UpdateListTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockProduct>().UpdateAsync(request.TransferStockProductDtos.Adapt<List<TransferStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Product

        #region UPDATE Detail

        public async Task<TransferStockLogDto> Handle(UpdateTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().UpdateAsync(request.TransferStockLogDto.Adapt<TransferStockLog>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransferStockLogDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransferStockLogDto>> Handle(UpdateListTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransferStockLog>().UpdateAsync(request.TransferStockLogDtos.Adapt<List<TransferStockLog>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransferStockLogDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Detail

        #region DELETE

        public async Task<bool> Handle(DeleteTransferStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransferStock>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransferStock>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #region DELETE Product

        public async Task<bool> Handle(DeleteTransferStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransferStockProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransferStockProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockProductQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Product

        #region DELETE Detail

        public async Task<bool> Handle(DeleteTransferStockLogRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransferStockLog>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransferStockLog>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransferStockLogQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Detail
    }
}