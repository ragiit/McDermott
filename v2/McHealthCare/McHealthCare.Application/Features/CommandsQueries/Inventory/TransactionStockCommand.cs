namespace McHealthCare.Application.Features.Commands.Inventory
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

        public class CreateTransactionStockRequest(TransactionStockDto transactionStockDto) : IRequest<TransactionStockDto>
        {
            public TransactionStockDto TransactionStockDto { get; set; } = transactionStockDto;
        }

        public class CreateListTransactionStockRequest(List<TransactionStockDto> transactionStockDto) : IRequest<List<TransactionStockDto>>
        {
            public List<TransactionStockDto> TransactionStockDtos { get; set; } = transactionStockDto;
        }

        #endregion CREATE

        #region Update

        public class UpdateTransactionStockRequest(TransactionStockDto transactionStockDto) : IRequest<TransactionStockDto>
        {
            public TransactionStockDto TransactionStockDto { get; set; } = transactionStockDto;
        }

        public class UpdateListTransactionStockRequest(List<TransactionStockDto> TransactionStockDto) : IRequest<List<TransactionStockDto>>
        {
            public List<TransactionStockDto> TransactionStockDto { get; set; } = TransactionStockDto;
        }

        #endregion Update

        #region DELETE

        public class DeleteTransactionStockRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}