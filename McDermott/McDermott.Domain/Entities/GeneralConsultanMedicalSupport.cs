namespace McDermott.Domain.Entities
{
    public class GeneralConsultanMedicalSupport : BaseAuditableEntity
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long? PractitionerLabEximinationId { get; set; }
        public string? LabEximinationName { get; set; }
        public long? LabResulLabExaminationtId { get; set; }
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
        public long? PractitionerECGId { get; set; }
        public bool IsNormalRestingECG { get; set; } = false;
        public bool IsSinusRhythm { get; set; } = false;
        public bool IsSinusBradycardia { get; set; } = false;
        public bool IsSinusTachycardia { get; set; } = false;
        public bool IsVentriculatExtraSystole { get; set; } = false;
        public bool IsSupraventricularExtraSystole { get; set; } = false;
        public bool IsOtherECG { get; set; } = false;
        public string? OtherDesc { get; set; }
        public string? Status { get; set; }

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
        public LabTest? LabResulLabExaminationt { get; set; }
    }
}