using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class StockOutLinesDto : IMapFrom<StockOutLines>
    {
        public long Id { get; set; }
        public long? LinesId { get; set; }
        public long? TransactionStockId { get; set; }

        [Required(ErrorMessage = "Input Stock Not Null!!")]
        public long CutStock { get; set; } = 0;

        public long? CurrentStock { get; set; }
        public string? Batch { get; set; }
        public DateTime? ExpiredDate { get; set; }

        [SetToNull]
        public virtual ConcoctionLineDto? Lines { get; set; }

        [SetToNull]
        public virtual TransactionStockDto? TransactionStock { get; set; }
    }
}