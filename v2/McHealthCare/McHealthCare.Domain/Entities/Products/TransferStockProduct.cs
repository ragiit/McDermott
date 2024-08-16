namespace McHealthCare.Domain.Entities.Products
{
    public class TransferStockProduct : BaseAuditableEntity
    {
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public Guid? TransferStockId { get; set; }
        public Guid? ProductId { get; set; }
        public long? QtyStock { get; set; }

        public TransferStock? TransferStock { get; set; }

        public Product? Product { get; set; }
    }
}