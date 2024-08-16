namespace McHealthCare.Domain.Entities.Products
{
    public class TransactionStock : BaseAuditableEntity
    {
        public string? SourceTable { get; set; }
        public Guid? SourcTableId { get; set; }
        public Guid? ProductId { get; set; }
        public string? Reference { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public Guid? LocationId { get; set; }
        public long? Quantity { get; set; }
        public Guid? UomId { get; set; }
        public bool? Validate { get; set; }

        public Product? Product { get; set; }
        public Location? Location { get; set; }
        public Uom? Uom { get; set; }
        public InventoryAdjusment? InventoryAdjusment { get; set; }
        public List<StockOutLines>? StockOutLines { get; set; }
        public List<StockOutPrescription>? StockOutPrescription { get; set; }
    }
}