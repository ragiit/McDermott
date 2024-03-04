namespace McDermott.Application.Dtos.Queue
{
    public class KioskQueueDto : IMapFrom<KioskQueue>
    {
        public int Id { get; set; }
        public int? KioskId { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceKId { get; set; }
        public int? NoQueue { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Queues { get; set; }
        public string? Status { get; set; }

        public virtual KioskDto? Kiosk { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual ServiceDto? ServiceK { get; set; }
    }
}