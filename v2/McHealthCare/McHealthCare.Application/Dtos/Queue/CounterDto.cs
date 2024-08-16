namespace McHealthCare.Application.Dtos.Queue
{
    public class CounterDto : IMapFrom<Counter>
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? ServiceKId { get; set; }
        public Guid? PhysicianId { get; set; }

        public string? Status { get; set; } = string.Empty;

        public virtual ServiceDto? Service { get; set; }
    }
}