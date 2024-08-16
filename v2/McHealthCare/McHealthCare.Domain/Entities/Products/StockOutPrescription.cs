namespace McHealthCare.Domain.Entities.Products
{
    public class StockOutPrescription : BaseAuditableEntity
    {
        public Guid? PrescriptionId { get; set; }
        public Guid? TransactionStockId { get; set; }
        public long? CutStock { get; set; }

        public Prescription? Prescription { get; set; }

        public TransactionStock? TransactionStock { get; set; }
    }
}