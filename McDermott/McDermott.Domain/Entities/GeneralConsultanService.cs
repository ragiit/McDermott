namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanService : BaseAuditableEntity
    {
        public long? KioskQueueId { get; set; }
        public long? PatientId { get; set; }
        public long? InsuranceId { get; set; }
        public long? InsurancePolicyId { get; set; }
        public long? ServiceId { get; set; }
        public long? PratitionerId { get; set; }
        public long? ClassTypeId { get; set; }
        public string? StagingStatus { get; set; }
        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }
        public string? Payment { get; set; }
        public string? NoRM { get; set; }
        public string? IdentityNumber { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? TypeRegistration { get; set; }
        public string? HomeStatus { get; set; }
        public string? TypeMedical { get; set; }
        public string? ScheduleTime { get; set; }
        public bool IsAlertInformationSpecialCase { get; set; } = false;
        public bool IsSickLeave { get; set; } = false;
        public bool IsMaternityLeave { get; set; } = false;
        public DateTime? StartDateSickLeave { get; set; }
        public DateTime? EndDateSickLeave { get; set; }
        public DateTime? StartMaternityLeave { get; set; }
        public DateTime? EndMaternityLeave { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? AppoimentDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public string? SerialNo { get; set; } // NoUrut


        /// <BPJS Rujukan>
        public string? ReferVerticalKhususCategoryName { get; set; }
        public string? ReferVerticalKhususCategoryCode { get; set; }
        public string? ReferVerticalSpesialisParentSpesialisName { get; set; }
        public string? ReferVerticalSpesialisParentSpesialisCode { get; set; }
        public string? ReferVerticalSpesialisParentSubSpesialisName { get; set; }
        public string? ReferVerticalSpesialisParentSubSpesialisCode { get; set; }
        public string? ReferReason { get; set; } = "-"; // Catatan
        public bool? IsSarana { get; set; } = false;
        public string? ReferVerticalSpesialisSaranaName { get; set; }
        public string? ReferVerticalSpesialisSaranaCode { get; set; }
        public string? PPKRujukanName { get; set; }
        public string? PPKRujukanCode { get; set; }
        public DateTime? ReferDateVisit { get; set; } // Tgl. Rencana Berkunjung
        /// </BPJS Rujukan>



        [SetToNull]
        public virtual User? Patient { get; set; }

        [SetToNull]
        public virtual User? Pratitioner { get; set; }

        [SetToNull]
        public virtual Insurance? Insurance { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }

        [SetToNull]
        public virtual InsurancePolicy? InsurancePolicy { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultanCPPT>? GeneralConsultanCPPTs { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultantClinicalAssesment>? GeneralConsultantClinicalAssesments { get; set; }

        [SetToNull]
        public virtual ClassType? ClassType { get; set; }

        [SetToNull]
        public virtual KioskQueue? KioskQueue { get; set; }
    }
}