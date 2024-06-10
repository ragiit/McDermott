using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class InventoryAdjusmentDetailDto : IMapFrom<InventoryAdjusmentDetail>
    {
        public long Id { get; set; }
        public long InventoryAdjusmentId { get; set; }
        public long? StockProductId { get; set; }

        [Required]
        public long? ProductId { get; set; }

        [NotMapped]
        public long TeoriticalQty { get; set; }

        [NotMapped]
        public long? UomId { get; set; }

        public DateTime? ExpiredDate { get; set; }

        [NotMapped]
        public string LotSerialNumber { get; set; } = "-";

        public long RealQty { get; set; }

        public InventoryAdjusmentDto? InventoryAdjusment { get; set; }
        public ProductDto? Product { get; set; }
        public StockProductDto? StockProduct { get; set; }
    }
}
