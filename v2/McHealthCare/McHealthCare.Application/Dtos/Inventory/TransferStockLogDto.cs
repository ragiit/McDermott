namespace McHealthCare.Application.Dtos.Inventory
{
    public class TransferStockLogDto : IMapFrom<TransferStockLog>
    {
        public Guid Id { get; set; }
        public Guid? TransferStockId { get; set; }
        public Guid? SourceId { get; set; }
        public Guid? DestinationId { get; set; }
        public EnumStatusInternalTransfer? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual TransferStockDto? TransferStock { get; set; }

        public virtual LocationDto? Source { get; set; }

        public virtual LocationDto? Destination { get; set; }
    }
}