namespace McDermott.Application.Dtos.Pharmacy
{
    public class PharmacyDto : IMapFrom<McDermott.Domain.Entities.Pharmacy>
    {
        public long Id { get; set; }

        [Required]
        public long? PatientId { get; set; }

        [Required]
        public long? PractitionerId { get; set; }

        [Required]
        public long? PrescriptionLocationId { get; set; }

        public long? MedicamentGroupId { get; set; }
        public long? ServiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; } = DateTime.Now;
        public bool IsWeather { get; set; } = false;
        public bool IsFarmacologi { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public string? Status { get; set; }

        public LocationDto? Location { get; set; }
        public MedicamentGroupDto? MedicamentGroup { get; set; }
        public ServiceDto? Service { get; set; }
        public UserDto? Patient { get; set; }
        public UserDto? Practitioner { get; set; }
    }
}