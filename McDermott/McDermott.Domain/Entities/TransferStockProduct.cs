namespace McDermott.Domain.Entities
{
    public class TransferStockProduct : BaseAuditableEntity
    {
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? TransferStockId { get; set; }
        public long? ProductId { get; set; }
        public long? QtyStock { get; set; }

        [SetToNull]
        public TransferStock? TransferStock { get; set; }

        [SetToNull]
        public Product? Product { get; set; }
    }
}