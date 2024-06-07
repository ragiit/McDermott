using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransactionStockProductDto : IMapFrom<TransactionStockProduct>
    {
        public long Id { get; set; }
        public long? TransactionStockId { get; set; }
        public long? StockId { get; set; }

        [Required(ErrorMessage = "Please Select Product Name...")]
        public long? ProductId { get; set; }
        public long? StockProductId { get; set; }

        [Required(ErrorMessage = "Please Input Stock Quantity...")]
        public long? QtyStock { get; set; }

        public bool TraceAvability { get; set; } = false;

        public string? StatusStock { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public string? ProductName { get; set; }
        public string? UomName { get; set; }

        [SetToNull]
        public virtual TransactionStockDto? TransactionStock { get; set; }

        [SetToNull]
        public virtual StockProductDto? Stock { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; }

        [SetToNull]
        public StockProduct? StockProduct { get; set; }
    }
}