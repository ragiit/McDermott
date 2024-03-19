namespace McDermott.Application.Dtos.Queue
{
    public partial class KioskDto : IMapFrom<Kiosk>
    {
        public long Id { get; set; }
        public string? Type { get; set; }

        [Required]
        [StringLength(150)]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NumberType { get; set; } = string.Empty;

        public long? PatientId { get; set; }
        public string? BPJS { get; set; } = string.Empty;
        public bool? StageBpjs { get; set; }
        public long? ServiceId { get; set; }
        public long? PhysicianId { get; set; }
        public long? Queue { get; set; }
        public long? CounterId { get; set; }
        public virtual CounterDto? Counter { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual UserDto? Patient { get; set; }
        public virtual UserDto? Physician { get; set; }
    }
}