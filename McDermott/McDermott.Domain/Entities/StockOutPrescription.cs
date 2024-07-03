

namespace McDermott.Domain.Entities
{
    public class StockOutPrescription:BaseAuditableEntity
    {
        public long? PrescriptionId {get; set; }
        public long? StockId { get; set; }
        public long? CutStock { get; set; }

        [SetToNull]
        public Prescription? Prescription { get; set; }
        [SetToNull]
        public StockProduct? Stock {  get; set; }
    }
}
