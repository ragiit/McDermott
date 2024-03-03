namespace McDermott.Application.Dtos.Queue
{
    public class CounterDto : IMapFrom<Counter>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceKId { get; set; }
        public int? PhysicianId { get; set; }

        public string? Status { get; set; } = string.Empty;

        public virtual ServiceDto? Service { get; set; }
    }
}