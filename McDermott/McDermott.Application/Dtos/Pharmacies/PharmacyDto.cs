using McDermott.Domain.Common;
using McDermott.Domain.Entities;

namespace McDermott.Application.Dtos.Pharmacies
{
    public class PharmacyDto : IMapFrom<Pharmacy>
    {
        public long Id { get; set; }

        [Required]
        public long? PatientId { get; set; }

        [Required]
        public long? PractitionerId { get; set; }

        [Required]
        public long? LocationId { get; set; }

        public long? MedicamentGroupId { get; set; }
        public long? ServiceId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime? ReceiptDate { get; set; } = DateTime.Now;
        public bool IsWeather { get; set; } = false;
        public bool IsFarmacologi { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public EnumStatusPharmacy? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        [SetToNull]
        public LocationDto? Location { get; set; }
        [SetToNull]
        public MedicamentGroupDto? MedicamentGroup { get; set; }
        [SetToNull]
        public ServiceDto? Service { get; set; }
        [SetToNull]
        public UserDto? Patient { get; set; }
        [SetToNull]
        public UserDto? Practitioner { get; set; }
    }
}