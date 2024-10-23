using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class TransferStockLogDto : IMapFrom<TransferStockLog>
    {
        public long Id { get; set; }
        public long? TransferStockId { get; set; }
        public long? SourceId { get; set; }
        public long? DestinationId { get; set; }
        public EnumStatusInternalTransfer? Status { get; set; }
        public long? UserById { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [SetToNull]
        public virtual TransferStockDto? TransferStock { get; set; }

        [SetToNull]
        public virtual LocationDto? Source { get; set; }

        [SetToNull]
        public virtual LocationDto? Destination { get; set; }
        
        [SetToNull]
        public virtual UserDto? UserBy { get; set; }
    }
}