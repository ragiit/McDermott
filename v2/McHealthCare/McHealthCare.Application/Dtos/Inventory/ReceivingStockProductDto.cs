namespace McHealthCare.Application.Dtos.Inventory
{
    public class ReceivingStockProductDto : IMapFrom<ReceivingStockProduct>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please Select Product Name...")]
        public Guid? ProductId { get; set; }

        public Guid? ReceivingStockId { get; set; }

        [Required(ErrorMessage = "Please Input Stock Quantity...")]
        public long Qty { get; set; } = 0;

        public bool TraceAbility { get; set; } = false;
        public DateTime? ExpiredDate { get; set; }

        public string? UomName { get; set; }
        public string? ProductName { get; set; }
        public string? PurchaseName { get; set; }
        public string? Batch { get; set; }

        public virtual ProductDto? Product { get; set; }

        public virtual StockProductDto? Stock { get; set; }

        public virtual ReceivingStockDto? ReceivingStock { get; set; }
    }
}