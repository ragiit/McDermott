namespace McDermott.Domain.Entities
{
    public class ClaimRequest : BaseAuditableEntity
    {
        public long? PatientId { get; set; }

        public long? PhycisianId { get; set; }

        public DateTime ClaimDate { get; set; }

        public long? BenefitId { get; set; }

        public string? Remark { get; set; } // Field catatan atau detail klaim

        public EnumClaimRequestStatus? Status { get; set; } // Enum untuk 'Draft' dan 'Done'

        public bool? IsNotificationSent { get; set; } // Status notifikasi otomatis

        [SetToNull]
        public BenefitConfiguration? Benefit { get; set; } // Navigation property untuk data benefit

        [SetToNull]
        public User? Patient { get; set; } // Navigation property untuk data pasien

        [SetToNull]
        public User? Phycisian { get; set; } // Navigation property untuk data dokter
    }
}