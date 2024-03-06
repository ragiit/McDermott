namespace McDermott.Application.Dtos.Queue
{
    public class KioskQueueDto : IMapFrom<KioskQueue>
    {
         public long Id { get; set; }
        public long? KioskId { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceKId { get; set; }
        public long? NoQueue { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Queues { get; set; }
        public string? Status { get; set; }

        public virtual KioskDto? Kiosk { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual ServiceDto? ServiceK { get; set; }
    }
}