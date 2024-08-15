namespace McHealthCare.Application.Dtos.Queue
{
    public class KioskQueueDto : IMapFrom<KioskQueue>
    {
        public Guid Id { get; set; }
        public Guid? KioskId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ServiceKId { get; set; }
        public long? QueueNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Queues { get; set; }
        public string? QueueStage { get; set; }
        public string? QueueStatus { get; set; }
        public Guid? ClassTypeId { get; set; }
        public string? NameClass { get; set; } = string.Empty;

        public virtual KioskDto? Kiosk { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual ServiceDto? ServiceK { get; set; }
        public virtual ClassTypeDto? ClassType { get; set; }
    }
}