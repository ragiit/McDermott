namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanMedicalSupportDto : IMapFrom<GeneralConsultanMedicalSupport>
    {
        public long Id { get; set; }
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
        public string? OtherExaminationAttachment { get; set; }

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

        private bool _AmphetaminesNegative = true;
        private bool _AmphetaminesPositive = false;

        public bool AmphetaminesNegative
        {
            get => _AmphetaminesNegative;
            set
            {
                _AmphetaminesNegative = value;
                if (value)
                {
                    _AmphetaminesPositive = false;
                }
            }
        }

        public bool AmphetaminesPositive
        {
            get => _AmphetaminesPositive;
            set
            {
                _AmphetaminesPositive = value;
                if (value)
                {
                    _AmphetaminesNegative = false;
                }
            }
        }


        private bool _BenzodiazepinesNegative { get; set; } = true;
        private bool _BenzodiazepinesPositive = false;

        public bool BenzodiazepinesNegative
        {
            get => _BenzodiazepinesNegative;
            set
            {
                _BenzodiazepinesNegative = value;
                if (value)
                {
                    _BenzodiazepinesPositive = false;
                }
            }
        }

        public bool BenzodiazepinesPositive
        {
            get => _BenzodiazepinesPositive;
            set
            {
                _BenzodiazepinesPositive = value;
                if (value)
                {
                    _BenzodiazepinesNegative = false;
                }
            }
        }

        // Lanjutkan dengan pola yang sama untuk properti-properti lainnya

        private bool _CocaineMetabolitesNegative { get; set; } = true;
        private bool _CocaineMetabolitesPositive = false;

        public bool CocaineMetabolitesNegative
        {
            get => _CocaineMetabolitesNegative;
            set
            {
                _CocaineMetabolitesNegative = value;
                if (value)
                {
                    _CocaineMetabolitesPositive = false;
                }
            }
        }

        public bool CocaineMetabolitesPositive
        {
            get => _CocaineMetabolitesPositive;
            set
            {
                _CocaineMetabolitesPositive = value;
                if (value)
                {
                    _CocaineMetabolitesNegative = false;
                }
            }
        }

        private bool _OpiatesNegative = true;
        private bool _OpiatesPositive = false;

        public bool OpiatesNegative
        {
            get => _OpiatesNegative;
            set
            {
                _OpiatesNegative = value;
                if (value)
                {
                    _OpiatesPositive = false;
                }
            }
        }

        public bool OpiatesPositive
        {
            get => _OpiatesPositive;
            set
            {
                _OpiatesPositive = value;
                if (value)
                {
                    _OpiatesNegative = false;
                }
            }
        }

        private bool _MethamphetaminesNegative = true;
        private bool _MethamphetaminesPositive = false;

        public bool MethamphetaminesNegative
        {
            get => _MethamphetaminesNegative;
            set
            {
                _MethamphetaminesNegative = value;
                if (value)
                {
                    _MethamphetaminesPositive = false;
                }
            }
        }

        public bool MethamphetaminesPositive
        {
            get => _MethamphetaminesPositive;
            set
            {
                _MethamphetaminesPositive = value;
                if (value)
                {
                    _MethamphetaminesNegative = false;
                }
            }
        }

        private bool _THCCannabinoidMarijuanaNegative = true;
        private bool _THCCannabinoidMarijuanaPositive = false;

        public bool THCCannabinoidMarijuanaNegative
        {
            get => _THCCannabinoidMarijuanaNegative;
            set
            {
                _THCCannabinoidMarijuanaNegative = value;
                if (value)
                {
                    _THCCannabinoidMarijuanaPositive = false;
                }
            }
        }

        public bool THCCannabinoidMarijuanaPositive
        {
            get => _THCCannabinoidMarijuanaPositive;
            set
            {
                _THCCannabinoidMarijuanaPositive = value;
                if (value)
                {
                    _THCCannabinoidMarijuanaNegative = false;
                }
            }
        }

        public long? PractitionerECGId { get; set; }
        public bool IsNormalRestingECG { get; set; } = false;
        public bool IsSinusRhythm { get; set; } = false;
        public bool IsSinusBradycardia { get; set; } = false;
        public long HR { get; set; }
        public bool IsSinusTachycardia { get; set; } = false;
        //public bool IsVentriculatExtraSystole { get; set; } = false;
        //public bool IsSupraventricularExtraSystole { get; set; } = false;
        public bool IsOtherECG { get; set; } = false;
        public string? OtherDesc { get; set; }
        public bool IsOtherExaminationECG { get; set; } = false;
        public string? OtherExaminationTypeECG { get; set; }
        public string? OtherExaminationRemarkECG { get; set; }
        public string? Status { get; set; } = "Draft";
        public long? LabTestId { get; set; }

        public LabTestDto? LabTest { get; set; }
        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public UserDto? PractitionerLabEximination { get; set; }
        public UserDto? PractitionerRadiologyEximination { get; set; }
        public UserDto? PractitionerAlcoholEximination { get; set; }
        public UserDto? PractitionerDrugEximination { get; set; }
        public UserDto? PractitionerECG { get; set; }
        public LabTestDetailDto? LabResulLabExaminationt { get; set; }
    }
}