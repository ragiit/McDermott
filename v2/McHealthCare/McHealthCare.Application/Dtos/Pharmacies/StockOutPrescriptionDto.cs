namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class StockOutPrescriptionDto : IMapFrom<StockOutPrescription>
    {
        public Guid Id { get; set; }
        public Guid? PrescriptionId { get; set; }
        public Guid? TransactionStockId { get; set; }

        [Required(ErrorMessage = "Input Stock Not Null!!")]
        public long CutStock { get; set; } = 0;

        public long? CurrentStock { get; set; }
        public string? Batch { get; set; }
        public Guid? uomId { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public virtual PrescriptionDto? Prescription { get; set; }

        public virtual TransactionStockDto? TransactionStock { get; set; }
    }
}