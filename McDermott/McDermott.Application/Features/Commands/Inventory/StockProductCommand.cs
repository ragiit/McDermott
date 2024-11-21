namespace McDermott.Application.Features.Commands.Inventory
{
    public class StockProductCommand
    {
        #region GET

        public class GetStockProductQuery(Expression<Func<StockProduct, bool>>? predicate = null, bool removeCache = false) : IRequest<List<StockProductDto>>
        {
            public Expression<Func<StockProduct, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreateStockProductRequest(StockProductDto StockProductDto) : IRequest<StockProductDto>
        {
            public StockProductDto StockProductDto { get; set; } = StockProductDto;
        }

        public class CreateListStockProductRequest(List<StockProductDto> StockProductDtos) : IRequest<List<StockProductDto>>
        {
            public List<StockProductDto> StockProductDtos { get; set; } = StockProductDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateStockProductRequest(StockProductDto StockProductDto) : IRequest<StockProductDto>
        {
            public StockProductDto StockProductDto { get; set; } = StockProductDto;
        }

        public class UpdateListStockProductRequest(List<StockProductDto> StockProductDtos) : IRequest<List<StockProductDto>>
        {
            public List<StockProductDto> StockProductDtos { get; set; } = StockProductDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteStockProductRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}