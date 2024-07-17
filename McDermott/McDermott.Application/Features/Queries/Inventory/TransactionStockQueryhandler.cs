
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Inventory.TransferStockCommand;

namespace McDermott.Application.Features.Queries.Inventory
{
    public class TransactionStockQueryhandler(IUnitOfWork _unitOfWork, IMemoryCache _cache) : 
        IRequestHandler<GetTransactionStockQuery, List<TransactionStockDto>>,
        IRequestHandler<CreateTransactionStockRequest, TransactionStockDto>,
        IRequestHandler<CreateListTransactionStockRequest, List<TransactionStockDto>>,
        IRequestHandler<DeleteTransactionStockRequest, bool>
    {
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
                      .Include(x => x.Receiving)
                      .Include(x => x.Prescription)
                      .Include(x => x.ConcoctionLine)
                      .Include(x => x.Transfer)
                      .Include(x => x.Product)
                      .Include(x => x.Source)
                      .Include(x => x.Destination)
                      .Include(x => x.Uom)
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


    }
}
