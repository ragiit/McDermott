

namespace McHealthCare.Domain.Entities.Products
{
    public class StockOutPrescription:BaseAuditableEntity
    {
        public Guid? PrescriptionId {get; set; }
        public Guid? TransactionStockId {  get; set; }
        public long? CutStock { get; set; }

        [SetToNull]
        public Prescription? Prescription { get; set; } 
        [SetToNull]
        public TransactionStock? TransactionStock { get; set; }
    }
}
