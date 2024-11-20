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
        [Required]
        public long? UomId { get; set; }

        [Required]
        public DateTime? ExpiredDate { get; set; } = null;

        [NotMapped]
        public string LotSerialNumber { get; set; } = "-";
        [Required]
        public long? RealQty { get; set; } = 0;

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
            return RealQty.GetValueOrDefault() - TeoriticalQty;
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