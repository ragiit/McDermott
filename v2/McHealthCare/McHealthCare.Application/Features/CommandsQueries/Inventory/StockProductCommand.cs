namespace McHealthCare.Application.Features.Commands.Inventory
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

        public class DeleteStockProductRequest(Guid? id = null, List<Guid>? ids = null) : IRequest<bool>
        {
            public Guid Id { get; set; } = id ?? Guid.Empty;
            public List<Guid> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}