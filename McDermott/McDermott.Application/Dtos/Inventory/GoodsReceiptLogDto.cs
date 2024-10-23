using McDermott.Domain.Common;
namespace McDermott.Application.Dtos.Inventory
{
    public class GoodsReceiptLogDto : IMapFrom<GoodsReceiptLog>
    {
        public long Id { get; set; }
        public long? GoodsReceiptId { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusGoodsReceipt? Status { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public GoodsReceiptDto? GoodsReceipt { get; set; }
        public UserDto? UserBy { get; set; }
        public LocationDto? Destination { get; set; }
        public LocationDto? Source { get; set; }
    }
}