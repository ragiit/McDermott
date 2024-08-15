namespace McHealthCare.Application.Dtos.Inventory
{
    public record InventoryAdjusmentDetailDto : IMapFrom<InventoryAdjusmentDetail>
    {
        public Guid Id { get; set; }
        public Guid InventoryAdjusmentId { get; set; }
        public Guid? StockProductId { get; set; }
        public Guid? TransactionStockId { get; set; }

        [Required]
        public Guid? ProductId { get; set; }

        public long TeoriticalQty { get; set; } = 0;
        public string? Batch { get; set; }
        [NotMapped]
        public Guid? UomId { get; set; }

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