namespace McDermott.Application.Dtos.Queue
{
    public class DetailQueueDisplayDto : IMapFrom<DetailQueueDisplay>
    {
        public long Id { get; set; }
        public long? QueueDisplayId { get; set; }
        public long? CounterId { get; set; }
        public string? CounterName { get; set; } = string.Empty;
        public string? DisplayName { get; set; } = string.Empty;

        public virtual QueueDisplay? QueueDisplay { get; set; }
        public virtual Counter? Counter { get; set; }
    }
}