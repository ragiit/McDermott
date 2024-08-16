namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class StockOutLinesDto : IMapFrom<StockOutLines>
    {
        public Guid Id { get; set; }
        public Guid? LinesId { get; set; }
        public Guid? TransactionStockId { get; set; }

        [Required(ErrorMessage = "Input Stock Not Null!!")]
        public long CutStock { get; set; } = 0;

        public long? CurrentStock { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        public virtual ConcoctionLineDto? Lines { get; set; }

        public virtual TransactionStockDto? TransactionStock { get; set; }
    }
}