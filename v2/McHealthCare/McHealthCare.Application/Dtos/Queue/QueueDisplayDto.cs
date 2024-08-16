namespace McHealthCare.Application.Dtos.Queue
{
    public class QueueDisplayDto : IMapFrom<QueueDisplay>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? NameCounter { get; set; } = string.Empty;
        public List<Guid>? CounterIds { get; set; } = [];
        public virtual List<CounterDto>? Counter { get; set; }
    }
}