namespace McHealthCare.Application.Dtos.Queue
{
    public class KioskConfigDto : IMapFrom<KioskConfig>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Guid>? ServiceIds { get; set; } = [];
        public string ServiceName { get; set; } = string.Empty;
        public virtual ServiceDto? Service { get; set; }
    }
}