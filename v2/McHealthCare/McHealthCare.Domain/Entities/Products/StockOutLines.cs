namespace McHealthCare.Domain.Entities.Products
{
    public class StockOutLines : BaseAuditableEntity
    {
        public Guid? LinesId { get; set; }
        public Guid? TransactionStockId { get; set; }
        public long? CutStock { get; set; }

        public ConcoctionLine? Lines { get; set; }

        public TransactionStock? TransactionStock { get; set; }
    }
}