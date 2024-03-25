namespace McDermott.Application.Dtos.Queue
{
    public class DetailQueueDisplayDto : IMapFrom<DetailQueueDisplay>
    {
        public long? KioskQueueId { get; set; }
        public long? ServicekId { get; set; }
        public long? ServiceId { get; set; }
        public long? NumberQueue { get; set; }
    }
}