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

        public long TeoriticalQty { get; set; } = 0;
        public string? Batch { get; set; }
        [NotMapped]
        public long? UomId { get; set; }

        public DateTime? ExpiredDate { get; set; } = null;

        [NotMapped]
        public string LotSerialNumber { get; set; } = "-";

        public long RealQty { get; set; }

        // Gunakan Lazy untuk properti Difference
        private readonly Lazy<long> _difference;

        public InventoryAdjusmentDetailDto()
        {
            _difference = new Lazy<long>(CalculateDifference);
        }

        [NotMapped]
        public long Difference => _difference.Value;

        private long CalculateDifference()
        {
            return RealQty - TeoriticalQty;
            //return difference switch
            //{
            //    > 0 => $"+{difference}",
            //    < 0 => $"{difference}",
            //    _ => "0"
            //};
        }

        public InventoryAdjusmentDto? InventoryAdjusment { get; set; }
        public ProductDto? Product { get; set; }
        public StockProductDto? StockProduct { get; set; }
        public TransactionStockDto? TransactionStock { get; set; }
    }
}