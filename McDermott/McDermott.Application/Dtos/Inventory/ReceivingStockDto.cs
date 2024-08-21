using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class ReceivingStockDto : IMapFrom<ReceivingStock>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please Selected Destination!!..")]
        public long? DestinationId { get; set; }

        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? NumberPurchase { get; set; }
        public string? KodeReceiving { get; set; }
        public string? Reference { get; set; }

        public EnumStatusReceiving? Status { get; set; }

        [SetToNull]
        public virtual LocationDto? Destination { get; set; }
    }
}