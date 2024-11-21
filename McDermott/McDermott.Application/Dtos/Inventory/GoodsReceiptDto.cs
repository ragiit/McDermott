using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class GoodsReceiptDto : IMapFrom<GoodsReceipt>
    {
        public long Id { get; set; }

        public long? SourceId { get; set; }

        [Required(ErrorMessage = "Please Selected Destination!!..")]
        public long? DestinationId { get; set; }

        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? NumberPurchase { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? Reference { get; set; }

        public EnumStatusGoodsReceipt? Status { get; set; }

        [SetToNull]
        public virtual LocationDto? Destination { get; set; }

        [SetToNull]
        public virtual LocationDto? Source { get; set; }
    }
}