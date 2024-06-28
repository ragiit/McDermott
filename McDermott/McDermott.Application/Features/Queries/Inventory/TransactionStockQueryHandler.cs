using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class TransactionStockQueryHandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) :
        IRequestHandler<GetTransactionStockQuery, List<TransactionStockDto>>,
        IRequestHandler<CreateTransactionStockRequest, TransactionStockDto>,
        IRequestHandler<CreateListTransactionStockRequest, List<TransactionStockDto>>,
        IRequestHandler<UpdateTransactionStockRequest, TransactionStockDto>,
        IRequestHandler<UpdateListTransactionStockRequest, List<TransactionStockDto>>,
        IRequestHandler<DeleteTransactionStockRequest, bool>,
        IRequestHandler<GetTransactionStockProductQuery, List<TransactionStockProductDto>>,
        IRequestHandler<CreateTransactionStockProductRequest, TransactionStockProductDto>,
        IRequestHandler<CreateListTransactionStockProductRequest, List<TransactionStockProductDto>>,
        IRequestHandler<UpdateTransactionStockProductRequest, TransactionStockProductDto>,
        IRequestHandler<UpdateListTransactionStockProductRequest, List<TransactionStockProductDto>>,
        IRequestHandler<DeleteTransactionStockProductRequest, bool>,
        IRequestHandler<GetTransactionStockDetailQuery, List<TransactionStockDetailDto>>,
        IRequestHandler<CreateTransactionStockDetailRequest, TransactionStockDetailDto>,
        IRequestHandler<CreateListTransactionStockDetailRequest, List<TransactionStockDetailDto>>,
        IRequestHandler<UpdateTransactionStockDetailRequest, TransactionStockDetailDto>,
        IRequestHandler<UpdateListTransactionStockDetailRequest, List<TransactionStockDetailDto>>,
        IRequestHandler<DeleteTransactionStockDetailRequest, bool>
        
    {
        #region GET

        public async Task<List<TransactionStockDto>> Handle(GetTransactionStockQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTransactionStockQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransactionStock>? result))
                {
                    result = await _unitOfWork.Repository<TransactionStock>().Entities
                      .Include(x => x.Source)
                      .Include(x => x.Destination)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransactionStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET

        #region GET Product

        public async Task<List<TransactionStockProductDto>> Handle(GetTransactionStockProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTransactionStockProductQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransactionStockProduct>? result))
                {
                    result = await _unitOfWork.Repository<TransactionStockProduct>().Entities

                      .Include(x => x.Product)
                      .Include(x => x.TransactionStock)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= new List<TransactionStockProduct>();

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransactionStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Product

        #region GET Detail

        public async Task<List<TransactionStockDetailDto>> Handle(GetTransactionStockDetailQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string cacheKey = $"GetTransactionStockDetailQuery_";

                if (request.RemoveCache)
                    _cache.Remove(cacheKey);

                if (!_cache.TryGetValue(cacheKey, out List<TransactionStockDetail>? result))
                {
                    result = await _unitOfWork.Repository<TransactionStockDetail>().Entities
                      .Include(x => x.TransactionStock)
                      .Include(x => x.Source)
                      .Include(x => x.Destination)
                      .AsNoTracking()
                      .ToListAsync(cancellationToken);

                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }

                result ??= [];

                if (request.Predicate is not null)
                    result = [.. result.AsQueryable().Where(request.Predicate)];

                return result.ToList().Adapt<List<TransactionStockDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion GET Detail

        #region CREATE

        public async Task<TransactionStockDto> Handle(CreateTransactionStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStock>().AddAsync(request.TransactionStockDto.Adapt<TransactionStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransactionStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionStockDto>> Handle(CreateListTransactionStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStock>().AddAsync(request.TransactionStockDtos.Adapt<List<TransactionStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransactionStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE

        #region CREATE Product

        public async Task<TransactionStockProductDto> Handle(CreateTransactionStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockProduct>().AddAsync(request.TransactionStockProductDto.Adapt<TransactionStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransactionStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionStockProductDto>> Handle(CreateListTransactionStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockProduct>().AddAsync(request.TransactionStockProductDtos.Adapt<List<TransactionStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransactionStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Product

        #region CREATE Detail

        public async Task<TransactionStockDetailDto> Handle(CreateTransactionStockDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockDetail>().AddAsync(request.TransactionStockDetailDto.Adapt<TransactionStockDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransactionStockDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionStockDetailDto>> Handle(CreateListTransactionStockDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockDetail>().AddAsync(request.TransactionStockDetailDtos.Adapt<List<TransactionStockDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransactionStockDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion CREATE Detail

        #region UPDATE

        public async Task<TransactionStockDto> Handle(UpdateTransactionStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStock>().UpdateAsync(request.TransactionStockDto.Adapt<TransactionStock>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransactionStockDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionStockDto>> Handle(UpdateListTransactionStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStock>().UpdateAsync(request.TransactionStockDtos.Adapt<List<TransactionStock>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransactionStockDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE

        #region UPDATE Product

        public async Task<TransactionStockProductDto> Handle(UpdateTransactionStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockProduct>().UpdateAsync(request.TransactionStockProductDto.Adapt<TransactionStockProduct>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransactionStockProductDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionStockProductDto>> Handle(UpdateListTransactionStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockProduct>().UpdateAsync(request.TransactionStockProductDtos.Adapt<List<TransactionStockProduct>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockProductQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransactionStockProductDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Product

        #region UPDATE Detail

        public async Task<TransactionStockDetailDto> Handle(UpdateTransactionStockDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockDetail>().UpdateAsync(request.TransactionStockDetailDto.Adapt<TransactionStockDetail>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<TransactionStockDetailDto>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TransactionStockDetailDto>> Handle(UpdateListTransactionStockDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.Repository<TransactionStockDetail>().UpdateAsync(request.TransactionStockDetailDtos.Adapt<List<TransactionStockDetail>>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockDetailQuery_"); // Ganti dengan key yang sesuai

                return result.Adapt<List<TransactionStockDetailDto>>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion UPDATE Detail

        #region DELETE

        public async Task<bool> Handle(DeleteTransactionStockRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransactionStock>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransactionStock>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE

        #region DELETE Product

        public async Task<bool> Handle(DeleteTransactionStockProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransactionStockProduct>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransactionStockProduct>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockProductQuery_"); // Ganti dengan key yang sesuai

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion DELETE Product

        #region DELETE Detail

        public async Task<bool> Handle(DeleteTransactionStockDetailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id > 0)
                {
                    await _unitOfWork.Repository<TransactionStockDetail>().DeleteAsync(request.Id);
                }

                if (request.Ids.Count > 0)
                {
                    await _unitOfWork.Repository<TransactionStockDetail>().DeleteAsync(x => request.Ids.Contains(x.Id));
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _cache.Remove("GetTransactionStockDetailQuery_"); // Ganti dengan key yang sesuai

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