namespace McHealthCare.Application.Dtos.Inventory
{
    public class ReceivingStockDto : IMapFrom<ReceivingStock>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please Selected Destination!!..")]
        public Guid? DestinationId { get; set; }

        public DateTime? SchenduleDate { get; set; } = DateTime.Now;
        public string? NumberPurchase { get; set; }
        public string? KodeReceiving { get; set; }
        public string? Reference { get; set; }

        public EnumStatusReceiving? Status { get; set; }

        public virtual LocationDto? Destination { get; set; }
    }
}