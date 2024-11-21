using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.ClaimUserManagement
{
    public class ClaimRequestDto : IMapFrom<ClaimRequest>
    {
        public long Id { get; set; }

        public long? PatientId { get; set; }

        public long? PhycisianId { get; set; }

        public DateTime ClaimDate { get; set; } = DateTime.Now;

        public long? BenefitId { get; set; }

        public string? Remark { get; set; } // Field catatan atau detail klaim

        public EnumClaimRequestStatus? Status { get; set; } // Enum untuk 'Draft' dan 'Done'

        public bool? IsNotificationSent { get; set; } // Status notifikasi otomatis

        [SetToNull]
        public BenefitConfigurationDto? Benefit { get; set; } // Navigation property untuk data benefit

        [SetToNull]
        public UserDto? Patient { get; set; } // Navigation property untuk data pasien

        [SetToNull]
        public UserDto? Phycisian { get; set; } // Navigation property untuk data dokter
    }
}