using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Entities
{
    [Table("TelemedicineConsultation", Schema = "Telemedicine")]
    public partial class TelemedicineConsultation : BaseAuditableEntity
    {
        //[NotMapped]
        //public string StatusName => Status.GetDisplayName();
        public int Queue { get; set; }

        public string QueueString { get; set; } = string.Empty;
        public long? KioskQueueId { get; set; }
        public long? PatientId { get; set; }
        public long? InsurancePolicyId { get; set; }
        public long? ServiceId { get; set; }
        public long? PratitionerId { get; set; }
        public string? ClassType { get; set; }
        public string? Reference { get; set; }
        public string? ReferenceAnc { get; set; } = string.Empty; // For Anc Form
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

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? PatientNextVisitSchedule { get; set; }  // this field for Maternities -> Anc form

        public bool IsGC { get; set; }
        public bool IsMaternity { get; set; }

        [NotMapped]
        public string RegistrationDateString => RegistrationDate.GetValueOrDefault().ToString("dd-MM-yyyy");

        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }

        #region Rujukan

        public string? ReferralNo { get; set; } // No. Rujukan
        public string? SerialNo { get; set; }  // Dari Kiosk
        public string? VisitNumber { get; set; } // NO Kunjungan
        public string? ReferDiagnosisKd { get; set; } // Diagnosis
        public string? ReferDiagnosisNm { get; set; } // Diagnosis
        public bool? IsReferDiagnosisNonSpesialis { get; set; }  // Diagnosis
        public string? ReferVerticalKhususCategoryName { get; set; }
        public string? ReferVerticalKhususCategoryCode { get; set; }
        public DateTime? ReferSelectFaskesDate { get; set; } // Date ketika select faskes, ada di print bagian "Salam sejawat,"
        public DateTime? ReferDateVisit { get; set; } // Tgl. Rencana Berkunjung
        public string? PracticeScheduleTimeDate { get; set; } // Jadwal Praktek
        public DateTime? ReferralExpiry { get; set; } // Surat rujukan berlaku 1[satu] kali kunjungan,
        public string? ReferVerticalSpesialisParentSpesialisName { get; set; } // ANAK
        public string? ReferVerticalSpesialisParentSpesialisCode { get; set; } // ANA
        public string? ReferVerticalSpesialisParentSubSpesialisName { get; set; } // Anak
        public string? ReferVerticalSpesialisParentSubSpesialisCode { get; set; } // Ana
        public string? ReferReason { get; set; } = "-"; // Catatan

        public bool? IsSarana { get; set; } = false;
        public string? ReferVerticalSpesialisSaranaName { get; set; }
        public string? ReferVerticalSpesialisSaranaCode { get; set; }

        public string? PPKRujukanName { get; set; } = "-"; // nmppk , RS CHARIS MEDIKA
        public string? PPKRujukanCode { get; set; } // kdppk, 0070R018

        #endregion Rujukan

        /// <BPJS Rujukan>
        public string? KdPrognosa { get; set; }

        public string? Anamnesa { get; set; }
        public string? BMHP { get; set; }
        public string? ImageToBase64 { get; set; }
        public string? Description { get; set; }
        public string? Markers { get; set; }

        /// </BPJS Rujukan>
        public string? MedexType { get; set; }

        public bool IsMcu { get; set; } = false;
        public bool IsBatam { get; set; }
        public bool IsOutsideBatam { get; set; }

        //public EnumStatusTelemedicineConsultationtService Status { get; set; } = EnumStatusTelemedicineConsultationtService.Planned;
        public EnumStatusMCU StatusMCU { get; set; } = EnumStatusMCU.Draft;

        public string? McuExaminationDocs { get; set; }
        public string? McuExaminationBase64 { get; set; }
        public string? AccidentExaminationDocs { get; set; }
        public string? AccidentExaminationBase64 { get; set; }

        #region Mcdermott Internal

        public string? TypeClaim { get; set; }
        public string? Number { get; set; }
        public DateTime? DateRJMCINT { get; set; }
        public string? ReferTo { get; set; }
        public string? Hospital { get; set; }
        public string? Specialist { get; set; }
        public string? CategoryRJMCINT { get; set; }
        public string? ExamFor { get; set; }
        public long? OccupationalId { get; set; }
        public string? TempDiagnosis { get; set; }
        public string? TherapyProvide { get; set; }
        public string? InpatientClass { get; set; }

        #endregion Mcdermott Internal

        #region Clinical Assesment

        public string? ScrinningTriageScale { get; set; }
        public string? RiskOfFalling { get; set; }
        public string? RiskOfFallingDetail { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public long RR { get; set; }
        public decimal Temp { get; set; }
        public long HR { get; set; }
        public long PainScale { get; set; }
        public long Systolic { get; set; }
        public long DiastolicBP { get; set; }
        public long SpO2 { get; set; }
        public long Diastole { get; set; }
        public long WaistCircumference { get; set; }

        public double BMIIndex { get; set; }
        public string BMIIndexString { get; set; } = "0";

        public string BMIState { get; set; } = "-";
        public string ClinicVisitTypes { get; set; } = "Sick";
        public string? InformationFrom { get; set; }

        //public bool IsFamilyMedicalHistory { get; set; } = false;
        //public string? FamilyMedicalHistory { get; set; }
        //public bool IsMedicationHistory { get; set; } = false;
        //public string? PastMedicalHistory { get; set; }
        public long? AwarenessId { get; set; }

        public long E { get; set; } = 4;
        public long V { get; set; } = 5;
        public long M { get; set; } = 6;

        #endregion Clinical Assesment

        #region Vaccination

        public long? LocationId { get; set; }

        public bool IsVaccination { get; set; } = false;

        #endregion Vaccination

        #region Telemedic

        public bool IsTelemedicine { get; set; } = false;
        public string? LinkMeet { get; set; }

        #endregion Telemedic

        #region ANC

        public string? PregnancyStatusG { get; set; }
        public string? PregnancyStatusP { get; set; }
        public string? PregnancyStatusA { get; set; }
        public DateTime? HPHT { get; set; }
        public DateTime? HPL { get; set; }
        public int? LILA { get; set; }    // CM

        #endregion ANC

        #region Claim

        public bool? IsClaim { get; set; }

        #endregion Claim

        public long? ProjectId { get; set; }
        public bool IsAccident { get; set; }

        public virtual Locations? Location { get; set; }

        //[SetToNull]
        public virtual Project? Project { get; set; }

        //[SetToNull]
        public virtual Awareness? AwarenessDto { get; set; }

        //[SetToNull]
        public virtual KioskQueue? KioskQueue { get; set; }

        public virtual Occupational? Occupational { get; set; }

        //[SetToNull]
        public virtual User? Patient { get; set; }

        //[SetToNull]
        public virtual User? Pratitioner { get; set; }

        //[SetToNull]
        public virtual Service? Service { get; set; }

        //[SetToNull]
        public virtual InsurancePolicy? InsurancePolicy { get; set; }

        //[SetToNull]
        public virtual List<TelemedicineConsultationCPPT>? TelemedicineConsultationCPPTs { get; set; }

        //[SetToNull]
        public virtual List<GeneralConsultationLog>? GeneralConsultationLogs { get; set; }

        //[SetToNull]
        public virtual List<Accident>? Accidents { get; set; }

        //[SetToNull]
        public virtual List<SickLeave>? SickLeaves { get; set; }

        //[SetToNull]
    }
}