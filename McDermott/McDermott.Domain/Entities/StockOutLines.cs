namespace McDermott.Domain.Entities
{
    public class StockOutLines : BaseAuditableEntity
    {
        public long? LinesId { get; set; }
        public long? TransactionStockId { get; set; }
        public long? CutStock { get; set; }

        [SetToNull]
        public ConcoctionLine? Lines { get; set; }

        [SetToNull]
        public TransactionStock? TransactionStock { get; set; }
    }
}