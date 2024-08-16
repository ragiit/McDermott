namespace McHealthCare.Domain.Entities.Products
{
    public class StockProduct : BaseAuditableEntity
    {
        public Guid? ProductId { get; set; }
        public long? Qty { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? DestinanceId { get; set; }
        public Guid? UomId { get; set; }
        public string? StatusTransaction { get; set; }
        public string? Batch { get; set; }
        public string? Referency { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? Expired { get; set; }

        public Product? Product { get; set; }

        public Location? Source { get; set; }

        public Location? Destinance { get; set; }

        public Uom? Uom { get; set; }

        public List<TransferStock>? TransactionStocks { get; set; }

        public List<ReceivingStockProduct>? ReceivingStockProduct { get; set; }

        public List<StockOutPrescription>? StockOutPrescriptions { get; set; }
    }
}