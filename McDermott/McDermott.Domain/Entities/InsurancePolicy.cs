namespace McDermott.Domain.Entities
{
    public class InsurancePolicy : BaseAuditableEntity
    {
        public long UserId { get; set; } // Patient
        public long InsuranceId { get; set; }
        public string? PolicyNumber { get; set; }
        public bool Active { get; set; }

        [StringLength(200)]
        public string? Prolanis { get; set; }

        [StringLength(200)]
        public string? ParticipantName { get; set; }

        [StringLength(200)]
        public string? NoCard { get; set; }

        [StringLength(200)]
        public string? NoId { get; set; } // NIK

        [StringLength(5)]
        public string? Sex { get; set; }

        [StringLength(200)]
        public string? Class { get; set; }

        [StringLength(200)]
        public string? MedicalRecordNo { get; set; }

        [StringLength(200)]
        public string? ServicePPKName { get; set; }

        [StringLength(200)]
        public string? ServicePPKCode { get; set; }

        [StringLength(50)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? NursingClass { get; set; }

        [StringLength(200)]
        public string? Diagnosa { get; set; }

        [StringLength(200)]
        public string? Poly { get; set; }

        [StringLength(200)]
        public string? Doctor { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime? CardPrintDate { get; set; }
        public DateTime? TmtDate { get; set; }
        public DateTime? TatDate { get; set; }

        [StringLength(200)]
        public string? ParticipantStatus { get; set; }

        [StringLength(200)]
        public string? ServiceType { get; set; }

        [StringLength(200)]
        public string? ServiceParticipant { get; set; }

        public DateTime? CurrentAge { get; set; }
        public DateTime? AgeAtTimeOfService { get; set; }

        [StringLength(200)]
        public string? DinSos { get; set; }

        [StringLength(200)]
        public string? PronalisPBR { get; set; }

        [StringLength(200)]
        public string? NoSKTM { get; set; }

        [StringLength(200)]
        public string? InsuranceNo { get; set; }

        [StringLength(200)]
        public string? InsuranceName { get; set; }

        [StringLength(200)]
        public string? ProviderName { get; set; }

        public virtual Insurance? Insurance { get; set; }
        public virtual User? User { get; set; }
    }
}