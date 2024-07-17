using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public record InventoryAdjusmentDetailDto : IMapFrom<InventoryAdjusmentDetail>
    {
        public long Id { get; set; }
        public long InventoryAdjusmentId { get; set; }
        public long? StockProductId { get; set; }
        public long? TransactionStockId { get; set; }

        [Required]
        public long? ProductId { get; set; }

        [NotMapped]
        public long TeoriticalQty { get; set; }

        [NotMapped]
        public long? UomId { get; set; }

        public DateTime? ExpiredDate { get; set; } = null;

        [NotMapped]
        public string LotSerialNumber { get; set; } = "-";

        public long RealQty { get; set; }

        // Gunakan Lazy untuk properti Difference
        private readonly Lazy<string> _difference;

        public InventoryAdjusmentDetailDto()
        {
            _difference = new Lazy<string>(CalculateDifference);
        }

        [NotMapped]
        public string Difference => _difference.Value;

        private string CalculateDifference()
        {
            var difference = RealQty - (TransactionStock?.Quantity ?? 0);
            return difference switch
            {
                > 0 => $"+{difference}",
                < 0 => $"{difference}",
                _ => "0"
            };
        }


        public InventoryAdjusmentDto? InventoryAdjusment { get; set; }
        public ProductDto? Product { get; set; }
        public StockProductDto? StockProduct { get; set; }
        public TransactionStockDto? TransactionStock { get; set; }
    }
}
