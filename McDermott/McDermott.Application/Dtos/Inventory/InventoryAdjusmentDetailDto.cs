namespace McDermott.Application.Dtos.Inventory
{
    public class InventoryAdjusmentDetailDto : IMapFrom<InventoryAdjusmentDetail>
    {
        public long Id { get; set; }
        public long InventoryAdjusmentId { get; set; }

        [Required]
        public long? ProductId { get; set; }

        [NotMapped]
        public long TeoriticalQty { get; set; }

        [NotMapped]
        public long? UomId { get; set; }

        [Required]
        public DateTime? ExpiredDate { get; set; }

        [NotMapped]
        public string LotSerialNumber { get; set; } = "-";

        [Required]
        public long RealQty { get; set; } = 0;

        public InventoryAdjusmentDto? InventoryAdjusment { get; set; }
        public ProductDto? Product { get; set; }
    }
}
