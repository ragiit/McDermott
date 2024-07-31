using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Dtos.Transaction
{
    public partial class GeneralConsultanServiceDto : IMapFrom<GeneralConsultanService>
    {
        public long Id { get; set; }

        public long? KioskQueueId { get; set; }

        //[Required(ErrorMessage = "The Patient field is required.")]
        public long? PatientId { get; set; }

        public long? InsurancePolicyId { get; set; }

        //[Required(ErrorMessage = "The Service field is required.")]
        public long? ServiceId { get; set; }

        public long? PratitionerId { get; set; }

        public long? ClassTypeId { get; set; }
        public EnumStatusGeneralConsultantService Status { get; set; } = EnumStatusGeneralConsultantService.Planned;
        [NotMapped]
        public string StatusName => Status.GetDisplayName();
        public EnumStatusMCU StatusMCU { get; set; } = EnumStatusMCU.Draft;
        [NotMapped]
        public string StatusMcuName => StatusMCU.GetDisplayName();
        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }

        [Required]
        public string? Payment { get; set; } = "BPJS";
        [Required]
        public string? TypeRegistration { get; set; } = "General Consultation";
        public string? MedexType { get; set; }
        public string? HomeStatus { get; set; }

        public string? TypeMedical { get; set; }

        public string? ScheduleTime { get; set; }

        [NotMapped]
        public long? Age
        {
            get
            {
                if (Patient is null || Patient.DateOfBirth is null)
                    return null;

                if (Patient.DateOfBirth is null)
                    return null;

                return DateTime.Now.Year - Patient.DateOfBirth.GetValueOrDefault().Year;
            }
            set
            { }
        }

        [Required]
        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public bool IsAlertInformationSpecialCase { get; set; } = false;

        [NotMapped]
        public string IsBatamString => IsBatam ? "Batam" : "Outside Batam";
        private bool _isBatam = true;
        public bool IsBatam
        {
            get => _isBatam;
            set
            {
                if (_isBatam != value)
                {
                    _isBatam = value;
                    IsOutsideBatam = !_isBatam;
                }
            }
        }

        public string? McuExaminationDocs { get; set; }
        public string? McuExaminationBase64 { get; set; }
        public string? AccidentExaminationDocs { get; set; }
        public string? AccidentExaminationBase64 { get; set; }

        public bool IsMcu { get; set; } = false;

        private bool _isOutsideBatam;
        public bool IsOutsideBatam
        {
            get => _isOutsideBatam;
            set
            {
                if (_isOutsideBatam != value)
                {
                    _isOutsideBatam = value;
                    _isBatam = !_isOutsideBatam;
                }
            }
        }

        public bool IsSickLeave { get; set; } = false;
        public bool IsMaternityLeave { get; set; } = false;
        public DateTime? StartDateSickLeave { get; set; }
        public DateTime? EndDateSickLeave { get; set; }
        public DateTime StartMaternityLeave { get; set; }
        public DateTime? EndMaternityLeave { get; set; }
        public bool IsWeather { get; set; } = false;
        public bool IsPharmacology { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
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


        #region Clinical Assesment
        public string? ScrinningTriageScale { get; set; }
        public string? RiskOfFalling { get; set; }
        public string? RiskOfFallingDetail { get; set; }
        public double _Weight { get; set; }

        public double Weight
        {
            get => _Weight;
            set
            {
                _Weight = value;
                CalculateBMI(Height, value);
            }
        }

        public double _Height { get; set; }

        public double Height
        {
            get => _Height;
            set
            {
                _Height = value;
                CalculateBMI(value, Weight);
            }
        }

        public void CalculateBMI(double height, double weight)
        {
            BMIIndex = 0;
            BMIState = "-";

            if (height > 0 && weight > 0)
            {
                weight = weight / 100;
                height = height / 1000;

                BMIIndex = weight / (height * height);
                BMIIndexString = Math.Round(BMIIndex, 2).ToString();

                if (BMIIndex < 18.5)
                {
                    BMIState = "Low Weight";
                }
                else if (BMIIndex >= 18.5 && BMIIndex <= 24.9)
                {
                    BMIState = "Normal";
                }
                else if (BMIIndex == 25.0 && BMIIndex <= 29.9)
                {
                    BMIState = "Overweight";
                }
                else
                {
                    BMIState = "Obesity";
                }
                return;
            }
        }

        public long RR { get; set; }
        public long Temp { get; set; }

        [Required]
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
        public string? InformationFrom { get; set; }  
        public long? ProjectId { get; set; }    
        public long? AwarenessId { get; set; }
        public string? IsFamilyMedicalHistory { get; set; } 
        public string? FamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistoryOther { get; set; }
        public string? IsMedicationHistory { get; set; } 
        public string? MedicationHistory { get; set; } 
        public string? PastMedicalHistory { get; set; }  

        public long E { get; set; } = 4;
        public long V { get; set; } = 5;
        public long M { get; set; } = 6;

        #endregion

        public virtual AwarenessDto? AwarenessDto { get; set; }
        public virtual KioskQueueDto? KioskQueue { get; set; }
        public virtual ClassTypeDto? ClassType { get; set; }
        public virtual UserDto? Patient { get; set; } = new();
        public virtual UserDto? Pratitioner { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual InsurancePolicyDto? InsurancePolicy { get; set; }
        public virtual ProjectDto? Project { get; set; }    

        // Add this property
        [NotMapped]
        public virtual AccidentDto? Accident { get; set; }
    }
}