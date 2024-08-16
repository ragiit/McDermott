namespace McHealthCare.Application.Dtos.Inventory
{
    public class ReceivingLogDto : IMapFrom<ReceivingLog>
    {
        public Guid Id { get; set; }
        public Guid? ReceivingId { get; set; }
        public Guid? SourceId { get; set; }
        public string? UserById { get; set; }
        public EnumStatusReceiving? Status { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ReceivingStockDto? Receiving { get; set; }
        public ApplicationUser? UserBy { get; set; }
        public LocationDto? Source { get; set; }
    }
}