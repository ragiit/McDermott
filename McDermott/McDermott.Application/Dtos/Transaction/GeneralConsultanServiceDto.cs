using McDermott.Application.Dtos.Queue;

using FluentValidation;

namespace McDermott.Application.Dtos.Transaction
{
    public partial class GeneralConsultanServiceDto : IMapFrom<GeneralConsultanService>
    {
        public long Id { get; set; }

        public string Reference { get; set; } = string.Empty;
        public string? ReferenceAnc { get; set; } = string.Empty; // For Anc Form

        public long? KioskQueueId { get; set; }

        //[Required(ErrorMessage = "The Patient field is required.")]
        public long? PatientId { get; set; }

        public long? InsurancePolicyId { get; set; }

        //[Required(ErrorMessage = "The Service field is required.")]
        public long? ServiceId { get; set; }

        public long? PratitionerId { get; set; }

        public string? ClassType { get; set; }
        public EnumStatusGeneralConsultantService Status { get; set; } = EnumStatusGeneralConsultantService.Planned;

        [NotMapped]
        public string StatusName => Status.GetDisplayName();

        public EnumStatusMCU StatusMCU { get; set; } = EnumStatusMCU.Draft;

        [NotMapped]
        public string StatusMcuName => StatusMCU.GetDisplayName();

        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }

        [Required]
        public string Payment { get; set; } = "BPJS";

        public string TypeRegistration { get; set; } = "General Consultation";

        public string? MedexType { get; set; }
        public string? HomeStatus { get; set; }

        public string? TypeMedical { get; set; }
        public string? Anamnesa { get; set; }
        public string? BMHP { get; set; }
        public string? KdPrognosa { get; set; } 
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
        public DateTime? PatientNextVisitSchedule { get; set; }  // this field for Maternities -> Anc form

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
        public DateTime? StartMaternityLeave { get; set; }
        public DateTime? EndMaternityLeave { get; set; }
        public bool IsWeather { get; set; } = false;
        public bool IsPharmacology { get; set; } = false;
        public bool IsFood { get; set; } = false;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsGC { get; set; }
        public bool IsMaternity { get; set; }
        public string? SerialNo { get; set; } // NoUrut
        public string? VisitNumber { get; set; } // NoUrut

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
        public string? PPKRujukanName { get; set; } = "-";
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
        public decimal Temp { get; set; }

        [Required]
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
        public long? ProjectId { get; set; }
        public long? AwarenessId { get; set; }

        public long E { get; set; } = 4;
        public long V { get; set; } = 5;
        public long M { get; set; } = 6;

        #endregion Clinical Assesment

        #region Vaccination

        public bool IsVaccination { get; set; } = false;
        public long? LocationId { get; set; }

        #endregion Vaccination

        #region ANC

        public string? PregnancyStatusG { get; set; }
        public string? PregnancyStatusP { get; set; }
        public string? PregnancyStatusA { get; set; }
        public DateTime? HPHT { get; set; }
        public DateTime? HPL { get; set; }
        public int? LILA { get; set; }    // CM

        #endregion ANC

        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string Kepada { get; set; }
        public string Hospital { get; set; }
        public string ExaminationPurpose { get; set; }
        public string Category { get; set; }
        public string ExamFor { get; set; }
        public string Name { get; set; }
        public string Job { get; set; }
        public string TemporaryDiagnosis { get; set; }
        public string TherapyProvided { get; set; }
        public string InpatientClass { get; set; }
        public string? ImageToBase64 { get; set; }
        public string? Description { get; set; }

        public string? Markers { get; set; }
        public string? LinkMeet { get; set; }
        public bool IsTelemedicine { get; set; } = false;
        public bool IsAccident { get; set; }

        public virtual LocationDto? Location { get; set; }
        public virtual AwarenessDto? Awareness { get; set; }
        public virtual KioskQueueDto? KioskQueue { get; set; }
        public virtual UserDto? Patient { get; set; } = new();
        public virtual UserDto? Pratitioner { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual InsurancePolicyDto? InsurancePolicy { get; set; }
        public virtual ProjectDto? Project { get; set; }

        // Add this property
        [NotMapped]
        public virtual AccidentDto? Accident { get; set; }
    }

    public class GeneralConsultanServiceValidator : AbstractValidator<GeneralConsultanServiceDto>
    {
        public GeneralConsultanServiceValidator()
        {
            // Validation for IsMaternity field
            //RuleFor(x => x.PatientNextVisitSchedule)
            //    .NotEmpty().WithMessage("Patient's next visit schedule is required for maternity cases")
            //    .When(x => x.IsMaternity == true);

            // Validation for Patient field
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("Patient is required")
                .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned));

            RuleFor(x => x.ServiceId)
              .NotEmpty().WithMessage("Service is required")
              .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned));

            //RuleFor(x => x.RegistrationDate)
            //     .NotEmpty().WithMessage("Registration Date is required")
            //     .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned))
            //     .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Registration Date cannot be in the past")
            //     .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned));

            // Validation for Current Mobile field
            //RuleFor(x => x.CurrentMobile)
            //    .NotEmpty().WithMessage("Current Mobile is required")
            //    .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned) ||
            //              x.Status.Equals(EnumStatusGeneralConsultantService.NurseStation) ||
            //              x.Status.Equals(EnumStatusGeneralConsultantService.Physician));

            // Validation for Type Registration field
            RuleFor(x => x.TypeRegistration)
                .NotEmpty().WithMessage("Registration Type is required")
                .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned));

            // Validation for Return Status field
            RuleFor(x => x.HomeStatus)
                .NotEmpty().WithMessage("Return Status is required")
                .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Physician));

            // Optional: If IsAlertInformationSpecialCase needs to be validated
            RuleFor(x => x.IsAlertInformationSpecialCase)
                .NotNull().WithMessage("Alert Information Special Case is required")
                .When(x => x.Status.Equals(EnumStatusGeneralConsultantService.Planned));

            RuleFor(x => x.Payment)
          .NotEmpty().WithMessage("Payment Method is required.");

            When(x => x.Payment == "Insurance" || x.Payment == "BPJS", () =>
            {
                RuleFor(x => x.InsurancePolicyId)
                    .NotNull().WithMessage("Insurance Policy is required when Payment Method is Insurance or BPJS.");
            });
        }
    }

    public class CreateUpdateGeneralConsultanServiceDto
    {
        public long Id { get; set; }
        public string? Reference { get; set; }
        public string? ReferenceAnc { get; set; } = string.Empty; // For Anc Form

        public DateTime? HPHT { get; set; }
        public DateTime? HPL { get; set; }
        public bool IsGC { get; set; }
        public bool IsMaternity { get; set; }
        public long? KioskQueueId { get; set; }

        //[Required(ErrorMessage = "The Patient field is required.")]
        public long? PatientId { get; set; }

        public long? InsurancePolicyId { get; set; }

        //[Required(ErrorMessage = "The Service field is required.")]
        public long? ServiceId { get; set; }

        public bool IsVaccination { get; set; } = false;
        public long? PratitionerId { get; set; }
        public string? ClassType { get; set; }
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

        public string? KdPrognosa { get; set; }
        public string? Anamnesa { get; set; }
        public string? BMHP { get; set; }
        public string? ScheduleTime { get; set; }
        public string? ImageToBase64 { get; set; }
        public string? Description { get; set; }
        public string? Markers { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? PatientNextVisitSchedule { get; set; }  // this field for Maternities -> Anc form

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
        public string? VisitNumber { get; set; } // NO Kunjungan

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
        ///
        public string? LinkMeet { get; set; } //telemedic

        public bool IsTelemedicine { get; set; } = false;

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
        public decimal Temp { get; set; }

        [Required]
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
        public long? ProjectId { get; set; }
        public long? AwarenessId { get; set; }
        public bool IsAccident { get; set; }

        public long E { get; set; } = 4;
        public long V { get; set; } = 5;
        public long M { get; set; } = 6;

        #endregion Clinical Assesment

        #region Vaccination

        public long? LocationId { get; set; }

        #endregion Vaccination
    }
}