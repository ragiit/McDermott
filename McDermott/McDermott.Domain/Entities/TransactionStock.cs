namespace McDermott.Domain.Entities
{
    public class TransactionStock : BaseAuditableEntity
    {
        public string? SourceTable { get; set; }
        public long? SourcTableId { get; set; }
        public long? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public long? LocationId { get; set; }
        public long? Quantity { get; set; }
        public long? UomId { get; set; }
        public bool? Validate { get; set; }

        public Product? Product { get; set; }
        public Locations? Location { get; set; }
        public Uom? Uom { get; set; }
        public List<StockOutLines>? StockOutLines { get; set; }
        public List<StockOutPrescription>? StockOutPrescription { get; set; }
    }
}