namespace McHealthCare.Application.Dtos.Transaction
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

        public string ConfinedSpaceString => IsConfinedSpace ? "Confined Space" : "Referral from GC";
        public bool IsConfinedSpace { get; set; } = false;
        public long? PractitionerECGId { get; set; }
        public bool IsNormalRestingECG { get; set; } = false;
        public bool IsSinusRhythm { get; set; } = false;
        public bool IsSinusBradycardia { get; set; } = false;
        public long HR { get; set; }
        public bool IsSinusTachycardia { get; set; } = false;
        public string? ECGAttachment { get; set; }

        //public bool IsVentriculatExtraSystole { get; set; } = false;
        //public bool IsSupraventricularExtraSystole { get; set; } = false;
        public bool IsOtherECG { get; set; } = false;

        public string? OtherDesc { get; set; }
        public bool IsOtherExaminationECG { get; set; } = false;
        public string? OtherExaminationTypeECG { get; set; }
        public string? OtherExaminationRemarkECG { get; set; }
        public EnumStatusGeneralConsultantServiceProcedureRoom? Status { get; set; } = EnumStatusGeneralConsultantServiceProcedureRoom.Draft;

        [NotMapped]
        public string StatusName => Status.GetDisplayName();

        public long? LabTestId { get; set; }

        #region Confined Space

        public long? EmployeeId { get; set; }
        public bool IsFirstTimeEnteringConfinedSpace { get; set; } = false;
        public long EnteringConfinedSpaceCount { get; set; } = 0;
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
        public long? SignatureEmployeeId { get; set; }
        public byte[]? SignatureEmployeeImagesMedicalHistory { get; set; } = [];
        public string? SignatureEmployeeImagesMedicalHistoryBase64 => System.Text.Encoding.UTF8.GetString(SignatureEmployeeImagesMedicalHistory ?? []);

        public long? Wt { get; set; }
        public long? Bp { get; set; }
        public long? Height { get; set; }
        public long? Pulse { get; set; }
        public long? ChestCircumference { get; set; }
        public long? AbdomenCircumference { get; set; }
        public long? RespiratoryRate { get; set; }
        public long? Temperature { get; set; }

        public string? Eye { get; set; }
        public string? EarNoseThroat { get; set; }
        public string? Cardiovascular { get; set; }
        public string? Respiratory { get; set; }
        public string? Abdomen { get; set; }
        public string? Extremities { get; set; }
        public string? Musculoskeletal { get; set; }
        public string? Neurologic { get; set; }
        public string? SpirometryTest { get; set; }
        public string? RespiratoryFitTest { get; set; }
        public long? Size { get; set; }
        public string? Comment { get; set; }
        public IEnumerable<string> Recommendeds { get; set; } = [];
        public string? Recommended { get; set; }
        public DateTime? DateEximinedbyDoctor { get; set; }
        public byte[]? SignatureEximinedDoctor { get; set; }
        public string? SignatureEximinedDoctorBase64 => System.Text.Encoding.UTF8.GetString(SignatureEximinedDoctor ?? []);
        public long? ExaminedPhysicianId { get; set; }

        #endregion Confined Space

        public EmployeeDto? Employee { get; set; }
        public DoctorDto? ExaminedPhysician { get; set; }
        public DoctorDto? SignatureEmployee { get; set; }
        public LabTestDto? LabTest { get; set; }
        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public DoctorDto? PractitionerLabEximination { get; set; }
        public DoctorDto? PractitionerRadiologyEximination { get; set; }
        public DoctorDto? PractitionerAlcoholEximination { get; set; }
        public DoctorDto? PractitionerDrugEximination { get; set; }
        public DoctorDto? PractitionerECG { get; set; }
        public LabTestDetailDto? LabResulLabExaminationt { get; set; }
    }
}