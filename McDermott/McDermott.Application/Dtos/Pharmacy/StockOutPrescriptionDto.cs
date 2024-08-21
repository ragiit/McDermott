using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class StockOutPrescriptionDto : IMapFrom<StockOutPrescription>
    {
        public long Id { get; set; }
        public long? PrescriptionId { get; set; }
        public long? TransactionStockId { get; set; }

        [Required(ErrorMessage = "Input Stock Not Null!!")]
        public long CutStock { get; set; } = 0;

        public long? CurrentStock { get; set; }
        public string? Batch { get; set; }
        public long? uomId { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [SetToNull]
        public virtual PrescriptionDto? Prescription { get; set; }

        [SetToNull]
        public virtual TransactionStockDto? TransactionStock { get; set; }
    }
}