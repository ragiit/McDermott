namespace McDermott.Domain.Entities
{
    public class ReceivingStockDetail : BaseAuditableEntity
    {
        public long? ProductId { get; set; }
        public long? StockId { get; set; }
        public DateTime? SchenduleDate { get; set; }
        public string? Reference { get; set; }

        public Product? Product { get; set; }
        public StockProduct? Stock { get; set; }
    }
}