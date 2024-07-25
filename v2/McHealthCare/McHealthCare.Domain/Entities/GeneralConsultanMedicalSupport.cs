using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Domain.Entities
{
    public class GeneralConsultanMedicalSupport : BaseAuditableEntity
    {
        public Guid? GeneralConsultanServiceId { get; set; }
        public Guid? PractitionerLabEximinationId { get; set; }
        public string? LabEximinationName { get; set; }
        public Guid? LabResulLabExaminationtId { get; set; }
        public List<long>? LabResulLabExaminationtIds { get; set; }
        public string? LabEximinationAttachment { get; set; }
        public Guid? PractitionerRadiologyEximinationId { get; set; }
        public string? RadiologyEximinationName { get; set; }
        public string? RadiologyEximinationAttachment { get; set; }
        public Guid? PractitionerAlcoholEximinationId { get; set; }
        public string? AlcoholEximinationName { get; set; }
        public string? AlcoholEximinationAttachment { get; set; }
        public bool? AlcoholNegative { get; set; }
        public bool? AlcoholPositive { get; set; }
        public Guid? PractitionerDrugEximinationId { get; set; }
        public string? DrugEximinationName { get; set; }
        public string? DrugEximinationAttachment { get; set; }
        public bool? DrugNegative { get; set; }
        public bool? DrugPositive { get; set; }
        public bool? AmphetaminesNegative { get; set; }
        public bool? AmphetaminesPositive { get; set; }
        public bool? BenzodiazepinesNegative { get; set; }
        public bool? BenzodiazepinesPositive { get; set; }
        public bool? CocaineMetabolitesNegative { get; set; }
        public bool? CocaineMetabolitesPositive { get; set; }
        public bool? OpiatesNegative { get; set; }
        public bool? OpiatesPositive { get; set; }
        public bool? MethamphetaminesNegative { get; set; }
        public bool? MethamphetaminesPositive { get; set; }
        public bool? THCCannabinoidMarijuanaNegative { get; set; }
        public bool? THCCannabinoidMarijuanaPositive { get; set; }
        public string? OtherExaminationAttachment { get; set; }
        public string? ECGAttachment { get; set; }
        public bool IsOtherExaminationECG { get; set; } = false;
        public string? OtherExaminationTypeECG { get; set; }
        public string? OtherExaminationRemarkECG { get; set; }
        public Guid? PractitionerECGId { get; set; }
        public bool IsNormalRestingECG { get; set; } = false;
        public bool IsSinusRhythm { get; set; } = false;
        public bool IsSinusBradycardia { get; set; } = false;
        public bool IsSinusTachycardia { get; set; } = false;

        #region Confined Space

        public Guid? EmployeeId { get; set; }
        public bool IsFirstTimeEnteringConfinedSpace { get; set; } = false;
        public int EnteringConfinedSpaceCount { get; set; } = 0;
        public bool IsDefectiveSenseOfSmell { get; set; } = false;
        public bool IsAsthmaOrLungAilment { get; set; } = false;
        public bool IsBackPainOrLimitationOfMobility { get; set; } = false;
        public bool IsClaustrophobia { get; set; } = false;
        public bool IsDiabetesOrHypoglycemia { get; set; } = false;
        public bool IsEyesightProblem { get; set; } = false;
        public bool IsFaintingSpellOrSeizureOrEpilepsy { get; set; } = false;
        public bool IsHearingDisorder { get; set; } = false;
        public bool IsHeartDiseaseOrDisorder { get; set; } = false;
        public bool IsHighBloodPressure { get; set; } = false;
        public bool IsLowerLimbsDeformity { get; set; } = false;
        public bool IsMeniereDiseaseOrVertigo { get; set; } = false;
        public string? RemarksMedicalHistory { get; set; }
        public DateTime? DateMedialHistory { get; set; }
        public Guid? SignatureEmployeeId { get; set; }
        public byte[]? SignatureEmployeeImagesMedicalHistory { get; set; } = [];
        public string? SignatureEmployeeImagesMedicalHistoryBase64 { get; set; }

        public Guid? Wt { get; set; }
        public Guid? Bp { get; set; }
        public Guid? Height { get; set; }
        public Guid? Pulse { get; set; }
        public Guid? ChestCircumference { get; set; }
        public Guid? AbdomenCircumference { get; set; }
        public Guid? RespiratoryRate { get; set; }
        public Guid? Temperature { get; set; }
        public bool IsConfinedSpace { get; set; } = false;

        public string? Eye { get; set; }
        public string? EarNoseThroat { get; set; }
        public string? Cardiovascular { get; set; }
        public string? Respiratory { get; set; }
        public string? Abdomen { get; set; }
        public string? Extremities { get; set; }
        public string? Musculoskeletal { get; set; }
        public string? NeuroLogic { get; set; }
        public string? SpirometryTest { get; set; }
        public string? RespiratoryFitTest { get; set; }
        public Guid? Size { get; set; }
        public string? Comment { get; set; }
        public List<string> Recommendeds { get; set; } = [];
        public DateTime? DateEximinedbyDoctor { get; set; }
        public byte[]? SignatureEximinedDoctor { get; set; }
        public string? SignatureEximinedDoctorBase64 { get; set; }
        public string? Recommended { get; set; }
        public Guid? ExaminedPhysicianId { get; set; }

        #endregion Confined Space

        public Guid HR { get; set; }

        //public bool IsVentriculatExtraSystole { get; set; } = false;
        //public bool IsSupraventricularExtraSystole { get; set; } = false;
        public bool IsOtherECG { get; set; } = false;

        public string? OtherDesc { get; set; }

        public EnumStatusGeneralConsultantServiceProcedureRoom? Status { get; set; } = EnumStatusGeneralConsultantServiceProcedureRoom.Draft;

        [NotMapped]
        public string StatusName => Status.GetDisplayName();

        public Guid? LabTestId { get; set; }

        //public User? Employee { get; set; }

        public LabTest? LabTest { get; set; }

        // Tandai properti yang harus diatur ke null
        public GeneralConsultanService? GeneralConsultanService { get; set; }

        //public User? PractitionerLabEximination { get; set; }

        //public User? PractitionerRadiologyEximination { get; set; }

        //public User? PractitionerAlcoholEximination { get; set; }

        //public User? PractitionerDrugEximination { get; set; }

        //public User? PractitionerECG { get; set; }

        public LabTestDetail? LabResulLabExaminationt { get; set; }

        public List<LabResultDetail>? LabResultDetails { get; set; }
    }
}