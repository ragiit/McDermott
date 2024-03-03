namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanService : BaseAuditableEntity
    {
        public int? PatientId { get; set; }
        public int? InsuranceId { get; set; }
        public int? InsurancePolicyId { get; set; }
        public int? ServiceId { get; set; }
        public int? PratitionerId { get; set; }
        public int? ClassType { get; set; }
        public string? StagingStatus { get; set; }
        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }
        public string? Payment { get; set; }
        public string? NoRM { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? TypeRegistration { get; set; }
        public string? TypeMedical { get; set; }
        public string? ScheduleTime { get; set; }
        public bool IsAlertInformationSpecialCase { get; set; } = false;
        public bool IsSickLeave { get; set; } = false;
        public DateTime? StartDateSickLeave { get; set; }
        public DateTime? EndDateSickLeave { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual User? Patient { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual User? Pratitioner { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual Insurance? Insurance { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual Service? Service { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual InsurancePolicy? InsurancePolicy { get; set; }

        public virtual List<GeneralConsultanCPPT>? GeneralConsultanCPPTs { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual List<GeneralConsultantClinicalAssesment>? GeneralConsultantClinicalAssesments { get; set; }
    }
}