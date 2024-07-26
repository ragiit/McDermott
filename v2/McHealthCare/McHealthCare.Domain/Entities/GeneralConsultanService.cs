namespace McHealthCare.Domain.Entities
{
    public partial class GeneralConsultanService : BaseAuditableEntity
    {
        public Guid? KioskQueueId { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? InsurancePolicyId { get; set; }
        public Guid? ServiceId { get; set; }
        public Guid? PratitionerId { get; set; }
        public Guid? ClassTypeId { get; set; }
        public Guid? AwarenessId { get; set; }

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
        public string? RiskOfFalling { get; set; }
        public string? RiskOfFallingDetail { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public int RR { get; set; }
        public int Temp { get; set; }
        public int HR { get; set; }
        public int PainScale { get; set; }
        public int Systolic { get; set; }
        public int DiastolicBP { get; set; }
        public int SpO2 { get; set; }
        public int Sistole { get; set; }
        public int Diastole { get; set; }
        public int WaistCircumference { get; set; }

        public double BMIIndex { get; set; }
        public string BMIIndexString { get; set; } = "0";

        public string BMIState { get; set; } = "-";
        public string ClinicVisitTypes { get; set; } = "Sick";
        public string? InformationFrom { get; set; }
        //public bool IsFamilyMedicalHistory { get; set; } = false;
        //public string? FamilyMedicalHistory { get; set; }
        //public bool IsMedicationHistory { get; set; } = false;
        //public string? PastMedicalHistory { get; set; }

        public int E { get; set; } = 4;
        public int V { get; set; } = 5;
        public int M { get; set; } = 6;

        #endregion Clinical Assesment

        public virtual Awareness? AwarenessDto { get; set; }

        public virtual KioskQueue? KioskQueue { get; set; }

        public virtual Service? Service { get; set; }

        public virtual InsurancePolicy? InsurancePolicy { get; set; }

        public virtual List<GeneralConsultanCPPT>? GeneralConsultanCPPTs { get; set; }

        public virtual List<GeneralConsultationLog>? GeneralConsultationLogs { get; set; }

        public virtual List<GeneralConsultanMedicalSupport>? GeneralConsultanMedicalSupports { get; set; }

        public virtual List<Accident>? Accidents { get; set; }

        public virtual List<SickLeave>? SickLeaves { get; set; }

        public virtual ClassType? ClassType { get; set; }
    }
}