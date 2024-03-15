namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanMedicalSupportDto : IMapFrom<GeneralConsultanMedicalSupport>
    {
        public long Id { get; set; }
        public long? GeneralConsultanServiceId { get; set; }
        public long? PractitionerLabEximinationId { get; set; }
        public string? LabEximinationName { get; set; }
        public string? LabEximinationAttachment { get; set; }
        public long? PractitionerRadiologyEximinationId { get; set; }
        public string? RadiologyEximinationName { get; set; }
        public string? RadiologyEximinationAttachment { get; set; }

        public long? PractitionerAlcoholEximinationId { get; set; }
        public string? AlcoholEximinationName { get; set; }
        public string? AlcoholEximinationAttachment { get; set; }

        private bool _AlcoholNegative = false;
        private bool _AlcoholPositive = false;

        public bool AlcoholNegative
        {
            get => _AlcoholNegative;
            set
            {
                _AlcoholNegative = value;
                if (value)
                {
                    _AlcoholPositive = false;
                }
            }
        }

        public bool AlcoholPositive
        {
            get => _AlcoholPositive;
            set
            {
                _AlcoholPositive = value;
                if (value)
                {
                    _AlcoholNegative = false;
                }
            }
        }

        public long? PractitionerDrugEximinationId { get; set; }
        public string? DrugEximinationName { get; set; }
        public string? DrugEximinationAttachment { get; set; }

        private bool _DrugNegative = false;
        private bool _DrugPositive = false;

        public bool DrugNegative
        {
            get => _DrugNegative;
            set
            {
                _DrugNegative = value;
                if (value)
                {
                    _DrugPositive = false;
                }
            }
        }

        public bool DrugPositive
        {
            get => _DrugPositive;
            set
            {
                _DrugPositive = value;
                if (value)
                {
                    _DrugNegative = false;
                }
            }
        }
        public long? PractitionerECGId { get; set; }
        public bool IsNormalRestingECG { get; set; } = false;
        public bool IsSinusRhythm { get; set; } = false;
        public bool IsSinusBradycardia { get; set; } = false;
        public bool IsSinusTachycardia { get; set; } = false;
        public bool IsVentriculatExtraSystole { get; set; } = false;
        public bool IsSupraventricularExtraSystole { get; set; } = false;
        public bool IsOtherECG { get; set; } = false;
        public string? OtherDesc { get; set; }
        public string? Status { get; set; } = "Draft";

        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public UserDto? PractitionerLabEximination { get; set; }
        public UserDto? PractitionerRadiologyEximination { get; set; }
        public UserDto? PractitionerAlcoholEximination { get; set; }
        public UserDto? PractitionerDrugEximination { get; set; }
        public UserDto? PractitionerECG { get; set; }
    }
}