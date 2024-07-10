using System.ComponentModel.DataAnnotations.Schema;

namespace McDermott.Domain.Entities
{
    public class GeneralConsultanMedicalSupport : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long? PractitionerLabEximinationId { get; set; }
        public string? LabEximinationName { get; set; }
        public long? LabResulLabExaminationtId { get; set; }
        public List<long>? LabResulLabExaminationtIds { get; set; }
        public string? LabEximinationAttachment { get; set; }
        public long? PractitionerRadiologyEximinationId { get; set; }
        public string? RadiologyEximinationName { get; set; }
        public string? RadiologyEximinationAttachment { get; set; }
        public long? PractitionerAlcoholEximinationId { get; set; }
        public string? AlcoholEximinationName { get; set; }
        public string? AlcoholEximinationAttachment { get; set; }
        public bool? AlcoholNegative { get; set; }
        public bool? AlcoholPositive { get; set; }
        public long? PractitionerDrugEximinationId { get; set; }
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
        public long? PractitionerECGId { get; set; }
        public bool IsNormalRestingECG { get; set; } = false;
        public bool IsSinusRhythm { get; set; } = false;
        public bool IsSinusBradycardia { get; set; } = false;
        public bool IsSinusTachycardia { get; set; } = false;

        #region Confined Space
        //public long? EmployeeId { get; set; }
        //public bool IsDefectiveSenseOfSmell { get; set; } = false;
        //public bool IsAsthmaOrLungAilment { get; set; } = false;
        //public bool IsBackPainOrLimitationOfMobility { get; set; } = false;
        //public bool IsClaustrophobia { get; set; } = false;
        //public bool IsDiabetesOrHypoglycemia { get; set; } = false;
        //public bool IsEyesightProblem { get; set; } = false;
        //public bool IsFaintingSpellOrSeizureOrEpilepsy { get; set; } = false;
        //public bool IsHearingDisorder { get; set; } = false;
        //public bool IsHeartDiseaseOrDisorder { get; set; } = false;
        //public bool IsHighBloodPressure { get; set; } = false;
        //public bool IsLowerLimbsDeformity { get; set; } = false;
        //public bool IsMeniereDiseaseOrVertigo { get; set; } = false;
        //public string? RemarksMedicalHistory { get; set; }
        //public DateTime? DateMedialHistory { get; set; }
        //public byte[]? SignatureEmployeeImagesMedicalHistory { get; set; }
        //public double? Weight { get; set; }
        //public double? Height { get; set; }
        //public double? ChestCircumference { get; set; }
        //public string? BloodPressure { get; set; }
        //public int? Pulse { get; set; }
        //public double? AbdomenCircumference { get; set; }
        //public double? RespiratoryRate { get; set; }
        //public double? Temperature { get; set; }

        //public string? Eye { get; set; }
        //public string? EarNoseThroat { get; set; }
        //public string? Cardiovascular { get; set; }
        //public string? Respiratory { get; set; }
        //public string? Abdomen { get; set; }
        //public string? Extremities { get; set; }
        //public string? Musculoskeletal { get; set; }
        //public string? Neurologic { get; set; }
        //public string? SpirometryTest { get; set; }
        //public string? RespiratoryFitTest { get; set; }
        //public long? Size { get; set; }
        //public string? Comment { get; set; }
        //public List<string> Recommendeds { get; set; } = [];
        //public DateTime? DateEximinedbyDoctor { get; set; }
        //public byte[]? SignatureEximinedDoctor { get; set; }


        #endregion

        public long HR { get; set; }
        //public bool IsVentriculatExtraSystole { get; set; } = false;
        //public bool IsSupraventricularExtraSystole { get; set; } = false;
        public bool IsOtherECG { get; set; } = false;
        public string? OtherDesc { get; set; }

        public EnumStatusGeneralConsultantServiceProcedureRoom? Status { get; set; } = EnumStatusGeneralConsultantServiceProcedureRoom.Draft;
        [NotMapped]
        public string StatusName => Status.GetDisplayName();
        public long? LabTestId { get; set; }

        //[SetToNull]
        //public User? Employee { get; set; }
        [SetToNull]
        public LabTest? LabTest { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public GeneralConsultanService? GeneralConsultanService { get; set; }

        [SetToNull]
        public User? PractitionerLabEximination { get; set; }

        [SetToNull]
        public User? PractitionerRadiologyEximination { get; set; }

        [SetToNull]
        public User? PractitionerAlcoholEximination { get; set; }

        [SetToNull]
        public User? PractitionerDrugEximination { get; set; }

        [SetToNull]
        public User? PractitionerECG { get; set; }

        [SetToNull]
        public LabTestDetail? LabResulLabExaminationt { get; set; }

        [SetToNull]
        public List<LabResultDetail>? LabResultDetails { get; set; }
    }
}