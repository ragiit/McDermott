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

        public class GetSingleTransactionStockQueryNew : IRequest<TransactionStockDto>
        {
            public List<Expression<Func<TransactionStock, object>>> Includes { get; set; }
            public Expression<Func<TransactionStock, bool>> Predicate { get; set; }
            public Expression<Func<TransactionStock, TransactionStock>> Select { get; set; }

            public List<(Expression<Func<TransactionStock, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetTransactionStockQueryNew : IRequest<(List<TransactionStockDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<TransactionStock, object>>> Includes { get; set; }
            public Expression<Func<TransactionStock, bool>> Predicate { get; set; }
            public Expression<Func<TransactionStock, TransactionStock>> Select { get; set; }

            public List<(Expression<Func<TransactionStock, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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

        public class DeleteTransactionStockRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}