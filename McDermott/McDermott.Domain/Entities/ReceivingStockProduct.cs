namespace McDermott.Domain.Entities
{
    public class ReceivingStockProduct : BaseAuditableEntity
    {
        public long? ReceivingStockId { get; set; }
        public long? ProductId { get; set; }
        public long? Qty { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [SetToNull]
        public ReceivingStock? ReceivingStock { get; set; }

        [SetToNull]
        public Product? Product { get; set; }

        [SetToNull]
        public StockProduct? Stock { get; set; }
    }
}