namespace McDermott.Application.Dtos.Queue
{
    public class KioskConfigDto : IMapFrom<KioskConfig>
    {
         public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<long>? ServiceIds { get; set; } = [];
        public string ServiceName { get; set; } = string.Empty;
        public virtual ServiceDto? Service { get; set; }
    }
}