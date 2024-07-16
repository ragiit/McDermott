using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Inventory
{
    public class TransactionStockCommand
    {
        #region GET

        public class GetTransactionStockQuery(Expression<Func<TransactionStock, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransactionStockDto>>
        {
            public Expression<Func<TransactionStock, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE
        public class CreateTransactionStockRequest(TransactionStockDto transactionStockDto):IRequest<TransactionStockDto>
        {
            public TransactionStockDto TransactionStockDto { get; set; } = transactionStockDto;
        }

        public class CreateListTransactionStockRequest(List<TransactionStockDto> transactionStockDto) : IRequest<List<TransactionStockDto>>
        {
            public List<TransactionStockDto> TransactionStockDtos { get; set; } = transactionStockDto;
        }
        #endregion

        #region DELETE
        public class DeleteTransactionStockRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}
