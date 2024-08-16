namespace McHealthCare.Domain.Entities.Products
{
    public class ReceivingStock : BaseAuditableEntity
    {
        public Guid? DestinationId { get; set; }
        public DateTime? SchenduleDate { get; set; }

        public string? KodeReceiving { get; set; }
        public string? NumberPurchase { get; set; }
        public string? Reference { get; set; }

        public EnumStatusReceiving? Status { get; set; }

        public Location? Destination { get; set; }

        public List<ReceivingStockProduct>? receivingStockProduct { get; set; }
    }
}