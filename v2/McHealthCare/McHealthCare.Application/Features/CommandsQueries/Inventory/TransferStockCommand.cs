namespace McHealthCare.Application.Features.Commands.Inventory
{
    public class TransferStockCommand
    {
        #region GET

        public class GetTransferStockQuery(Expression<Func<TransferStock, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockDto>>
        {
            public Expression<Func<TransferStock, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region GET Product

        public class GetTransferStockProductQuery(Expression<Func<TransferStockProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockProductDto>>
        {
            public Expression<Func<TransferStockProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Product

        #region GET Detail

        public class GetTransferStockLogQuery(Expression<Func<TransferStockLog, bool>>? predicate = null, bool removeCache = false) : IRequest<List<TransferStockLogDto>>
        {
            public Expression<Func<TransferStockLog, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET Detail

        #region CREATE

        public class CreateTransferStockRequest(TransferStockDto TransferStockDto) : IRequest<TransferStockDto>
        {
            public TransferStockDto TransferStockDto { get; set; } = TransferStockDto;
        }

        public class CreateListTransferStockRequest(List<TransferStockDto> TransferStockDtos) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockDtos { get; set; } = TransferStockDtos;
        }

        #endregion CREATE

        #region CREATE Product

        public class CreateTransferStockProductRequest(TransferStockProductDto TransferStockProductDto) : IRequest<TransferStockProductDto>
        {
            public TransferStockProductDto TransferStockProductDto { get; set; } = TransferStockProductDto;
        }

        public class CreateListTransferStockProductRequest(List<TransferStockProductDto> TransferStockProductDtos) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductDtos { get; set; } = TransferStockProductDtos;
        }

        #endregion CREATE Product

        #region CREATE Detail

        public class CreateTransferStockLogRequest(TransferStockLogDto TransferStockLogDto) : IRequest<TransferStockLogDto>
        {
            public TransferStockLogDto TransferStockLogDto { get; set; } = TransferStockLogDto;
        }

        public class CreateListTransferStockLogRequest(List<TransferStockLogDto> TransferStockLogDtos) : IRequest<List<TransferStockLogDto>>
        {
            public List<TransferStockLogDto> TransferStockLogDtos { get; set; } = TransferStockLogDtos;
        }

        #endregion CREATE Detail

        #region Update

        public class UpdateTransferStockRequest(TransferStockDto TransferStockDto) : IRequest<TransferStockDto>
        {
            public TransferStockDto TransferStockDto { get; set; } = TransferStockDto;
        }

        public class UpdateListTransferStockRequest(List<TransferStockDto> TransferStockDtos) : IRequest<List<TransferStockDto>>
        {
            public List<TransferStockDto> TransferStockDtos { get; set; } = TransferStockDtos;
        }

        #endregion Update



        #region Update Product

        public class UpdateTransferStockProductRequest(TransferStockProductDto TransferStockProductDto) : IRequest<TransferStockProductDto>
        {
            public TransferStockProductDto TransferStockProductDto { get; set; } = TransferStockProductDto;
        }

        public class UpdateListTransferStockProductRequest(List<TransferStockProductDto> TransferStockProductDtos) : IRequest<List<TransferStockProductDto>>
        {
            public List<TransferStockProductDto> TransferStockProductDtos { get; set; } = TransferStockProductDtos;
        }

        #endregion Update Product

        #region Update Detail

        public class UpdateTransferStockLogRequest(TransferStockLogDto TransferStockLogDto) : IRequest<TransferStockLogDto>
        {
            public TransferStockLogDto TransferStockLogDto { get; set; } = TransferStockLogDto;
        }

        public class UpdateListTransferStockLogRequest(List<TransferStockLogDto> TransferStockLogDtos) : IRequest<List<TransferStockLogDto>>
        {
            public List<TransferStockLogDto> TransferStockLogDtos { get; set; } = TransferStockLogDtos;
        }

        #endregion Update Detail

        #region DELETE

        public class DeleteTransferStockRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE



        #region DELETE Product

        public class DeleteTransferStockProductRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Product

        #region DELETE Detail

        public class DeleteTransferStockLogRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE Detail
    }
}