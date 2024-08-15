namespace McHealthCare.Application.Dtos.Inventory
{
    public class TransferStockProductDto : IMapFrom<TransferStockProduct>
    {
        public Guid Id { get; set; }
        public Guid? TransferStockId { get; set; }

        [Required(ErrorMessage = "Please Select Product Name...")]
        public Guid? ProductId { get; set; }

        [Required(ErrorMessage = "Please Input Stock Quantity...")]
        public long QtyStock { get; set; } = 0;

        public long CurrentStock { get; set; } = 0;
        public Guid? UomId { get; set; }
        public Guid? TransactionStockId { get; set; }

        public bool TraceAvability { get; set; } = false;

        public string? StatusStock { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public string? ProductName { get; set; }
        public string? UomName { get; set; }

        public virtual TransferStockDto? TransferStock { get; set; }

        public virtual UomDto? Uom { get; set; }

        public virtual ProductDto? Product { get; set; }

        public virtual TransactionStockDto? TransactionStock { get; set; }
    }
}