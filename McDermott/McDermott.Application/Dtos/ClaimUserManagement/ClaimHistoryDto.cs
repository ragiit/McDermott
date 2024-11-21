using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.ClaimUserManagement
{
    public class ClaimHistoryDto : IMapFrom<ClaimHistory>
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Benefit is required.")]
        public long? BenefitId { get; set; }

        [Required(ErrorMessage = "Patient is required.")]
        public long? PatientId { get; set; }

        public long? PhycisianId { get; set; }

        public DateTime ClaimDate { get; set; }

        public int ClaimedValue { get; set; } // Bisa berupa qty atau amount sesuai dengan konfigurasi benefit

        [SetToNull]
        public BenefitConfigurationDto? Benefit { get; set; } // Navigation property

        [SetToNull]
        public UserDto? Patient { get; set; } // Navigation property

        [SetToNull]
        public UserDto? Phycisian { get; set; } // Navigation property
    }
}