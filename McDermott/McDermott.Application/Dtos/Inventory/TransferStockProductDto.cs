using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransferStockProductDto : IMapFrom<TransferStockProduct>
    {
        public long Id { get; set; }
        public long? TransferStockId { get; set; }       

        [Required(ErrorMessage = "Please Select Product Name...")]
        public long? ProductId { get; set; }

        [Required(ErrorMessage = "Please Input Stock Quantity...")]
        public long QtyStock { get; set; } = 0;
        public long CurrentStock { get; set; } = 0;
        public long? UomId { get; set; }
        public long? TransactionStockId { get; set; }

        public bool TraceAvability { get; set; } = false;

        public string? StatusStock { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public string? ProductName { get; set; }
        public string? UomName { get; set; }

        [SetToNull]
        public virtual TransferStockDto? TransferStock { get; set; }
        [SetToNull]
        public virtual UomDto? Uom { get; set; }

        [SetToNull]
        public virtual ProductDto? Product { get; set; } 
        [SetToNull]
        public virtual TransactionStockDto? TransactionStock { get; set; }

       
    }
}