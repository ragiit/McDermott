namespace McHealthCare.Application.Dtos.Queue
{
    public partial class KioskDto : IMapFrom<Kiosk>
    {
        public Guid Id { get; set; }
        public string? Type { get; set; }

        [Required]
        [StringLength(150)]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NumberType { get; set; } = string.Empty;

        public string? PatientId { get; set; }
        public string? BPJS { get; set; } = string.Empty;
        public bool? StageBpjs { get; set; }
        public Guid? ServiceId { get; set; }
        public string? PhysicianId { get; set; }
        public long? Queue { get; set; }
        public Guid? CounterId { get; set; }
        public Guid? ClassTypeId { get; set; }
        public virtual CounterDto? Counter { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual PatientDto? Patient { get; set; }
        public virtual DoctorDto? Physician { get; set; }
    }
}