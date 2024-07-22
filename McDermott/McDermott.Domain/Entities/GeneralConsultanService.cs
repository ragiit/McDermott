namespace McDermott.Domain.Entities
{
    public partial class GeneralConsultanService : BaseAuditableEntity
    {
        public long? KioskQueueId { get; set; }
        public long? PatientId { get; set; }
        public long? InsurancePolicyId { get; set; }
        public long? ServiceId { get; set; }
        public long? PratitionerId { get; set; }
        public long? ClassTypeId { get; set; }
        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }
        public string? Payment { get; set; }
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
        public DateTime? AppointmentDate { get; set; }
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
        public string? MedexType { get; set; }
        public bool IsMcu { get; set; } = false;
        public bool IsBatam { get; set; }
        public bool IsOutsideBatam { get; set; }
        public EnumStatusGeneralConsultantService Status { get; set; } = EnumStatusGeneralConsultantService.Planned;
        public EnumStatusMCU StatusMCU { get; set; } = EnumStatusMCU.Draft;
        public string? McuExaminationDocs { get; set; }
        public string? McuExaminationBase64 { get; set; }
        public string? AccidentExaminationDocs { get; set; }
        public string? AccidentExaminationBase64 { get; set; }

        #region Clinical Assesment 
        public string? ScrinningTriageScale { get; set; }
        public string? RiskOfFallingDetail { get; set; }
        public double Weight { get; set; } 
        public double Height { get; set; }
        public long RR { get; set; }
        public long Temp { get; set; }
        public long HR { get; set; }
        public long PainScale { get; set; }
        public long Systolic { get; set; }
        public long DiastolicBP { get; set; }
        public long SpO2 { get; set; }
        public long Sistole { get; set; }
        public long Diastole { get; set; }
        public long WaistCircumference { get; set; }

        public double BMIIndex { get; set; }
        public string BMIIndexString { get; set; } = "0";

        public string BMIState { get; set; } = "-";
        public string ClinicVisitTypes { get; set; } = "Sick";
        //public string? InformationFrom { get; set; }
        //public bool IsFamilyMedicalHistory { get; set; } = false;
        //public string? FamilyMedicalHistory { get; set; }
        //public bool IsMedicationHistory { get; set; } = false;
        //public string? PastMedicalHistory { get; set; }
        public long? AwarenessId { get; set; }

        public long E { get; set; } = 4;
        public long V { get; set; } = 5;
        public long M { get; set; } = 6;

        #endregion

        [SetToNull]
        public virtual Awareness? AwarenessDto { get; set; }
        [SetToNull]
        public virtual KioskQueue? KioskQueue { get; set; }

        [SetToNull]
        public virtual User? Patient { get; set; }

        [SetToNull]
        public virtual User? Pratitioner { get; set; }


        [SetToNull]
        public virtual Service? Service { get; set; }

        [SetToNull]
        public virtual InsurancePolicy? InsurancePolicy { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultanCPPT>? GeneralConsultanCPPTs { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultationLog>? GeneralConsultationLogs { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }

        [SetToNull]
        public virtual List<GeneralConsultantClinicalAssesment>? GeneralConsultantClinicalAssesments { get; set; }

        [SetToNull]
        public virtual List<Accident>? Accidents { get; set; }

        [SetToNull]
        public virtual List<SickLeave>? SickLeaves { get; set; }

        [SetToNull]
        public virtual ClassType? ClassType { get; set; }
    }
}