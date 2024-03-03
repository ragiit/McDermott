namespace McDermott.Application.Dtos.Transaction
{
    public partial class GeneralConsultanServiceDto : IMapFrom<GeneralConsultanService>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Patient field is required.")]
        public int? PatientId { get; set; }

        public int? InsuranceId { get; set; }

        //[NoRMRequired(ErrorMessage = "The Insurance Policy field is required.")]
        public int? InsurancePolicyId { get; set; }

        public class NoRMRequiredAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var model = (GeneralConsultanServiceDto)validationContext.ObjectInstance;

                if (!string.IsNullOrEmpty(model.Payment) && string.IsNullOrEmpty((string)value))
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }
        }

        [Required(ErrorMessage = "The Service field is required.")]
        public int? ServiceId { get; set; }

        [Required(ErrorMessage = "The Physicion field is required.")]
        public int? PratitionerId { get; set; }

        public int? ClassType { get; set; }
        public string? StagingStatus { get; set; } = "Planned";
        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }

        [Required]
        public string? Payment { get; set; }

        public string? NoRM { get; set; }

        public string? IdentityNumber { get; set; }
        public DateTime? BirthDay { get; set; }

        [Required]
        public string? TypeRegistration { get; set; } = string.Empty;

        public string? TypeMedical { get; set; }

        [Required]
        public string? ScheduleTime { get; set; }

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public int? Age { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? RegistrationDate { get; set; } = DateTime.Now;

        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public bool Temp { get; set; } = false;
        public bool IsAlertInformationSpecialCase { get; set; } = false;
        public bool IsSickLeave { get; set; } = false;
        public DateTime? StartDateSickLeave { get; set; }
        public DateTime? EndDateSickLeave { get; set; }
        public bool IsWeather { get; set; } = false;
        public bool IsPharmacology { get; set; } = false;
        public bool IsFood { get; set; } = false;

        public virtual UserDto? Patient { get; set; }
        public virtual UserDto? Pratitioner { get; set; }
        public virtual InsuranceDto? Insurance { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual InsurancePolicyDto? InsurancePolicy { get; set; }
    }
}