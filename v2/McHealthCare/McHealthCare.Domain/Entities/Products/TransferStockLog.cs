namespace McHealthCare.Domain.Entities.Products
{
    public class TransferStockLog : BaseAuditableEntity
    {
        public Guid? TransferStockId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
        public string? Status { get; set; }

        public TransferStock? TransferStock { get; set; }

        public Location? Source { get; set; }

        public Location? Destination { get; set; }
    }
}