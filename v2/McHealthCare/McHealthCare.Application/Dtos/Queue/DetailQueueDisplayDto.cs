namespace McHealthCare.Application.Dtos.Queue
{
    public class DetailQueueDisplayDto : IMapFrom<DetailQueueDisplay>
    {
        public Guid? KioskQueueId { get; set; }
        public Guid? ServicekId { get; set; }
        public Guid? ServiceId { get; set; }
        public long? NumberQueue { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}