namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class PharmacyDto : IMapFrom<Pharmacy>
    {
        public Guid Id { get; set; }

        [Required]
        public string? PatientId { get; set; }

        [Required]
        public string? PractitionerId { get; set; }

        [Required]
        public Guid? PrescriptionLocationId { get; set; }

        public Guid? MedicamentGroupId { get; set; }
        public Guid? ServiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; } = DateTime.Now;
        public bool IsWeather { get; set; } = false;
        public bool IsFarmacologi { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public EnumStatusPharmacy? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        public LocationDto? Location { get; set; }
        public MedicamentGroupDto? MedicamentGroup { get; set; }
        public ServiceDto? Service { get; set; }
        public PatientDto? Patient { get; set; }
        public DoctorDto? Practitioner { get; set; }
    }
}