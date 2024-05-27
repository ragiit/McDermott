namespace McDermott.Application.Dtos.Transaction
{
    public partial class GeneralConsultanServiceDto : IMapFrom<GeneralConsultanService>
    {
        public long Id { get; set; }

        public long? KioskQueueId { get; set; }

        [Required(ErrorMessage = "The Patient field is required.")]
        public long? PatientId { get; set; }

        public long? InsuranceId { get; set; }

        public long? InsurancePolicyId { get; set; }

        [Required(ErrorMessage = "The Service field is required.")]
        public long? ServiceId { get; set; }

        public long? PratitionerId { get; set; }

        public long? ClassTypeId { get; set; }
        public string? StagingStatus { get; set; } = "Planned";
        public string? Method { get; set; }
        public string? AdmissionQueue { get; set; }

        [Required]
        public string? Payment { get; set; } = "BPJS";

        public string? NoRM { get; set; }

        public string? IdentityNumber { get; set; }
        public DateTime? BirthDay { get; set; }

        [Required]
        public string? TypeRegistration { get; set; }
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

        public DateTime? CreateDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? AppoimentDate { get; set; }

        public TimeSpan? WorkFrom { get; set; }
        public TimeSpan? WorkTo { get; set; }
        public bool Temp { get; set; } = false;
        public bool IsAlertInformationSpecialCase { get; set; } = false;
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
        public string? ReferVerticalKhususCategoryCode { get; set; }
        public string? ReferVerticalSpesialisParentSpesialisCode { get; set; }
        public string? ReferVerticalSpesialisParentSubSpesialisCode { get; set; }
        public string? ReferReason { get; set; } = "-";
        public bool? IsSarana { get; set; } = false;
        public string? ReferVerticalSpesialisSaranaCode { get; set; }
        public bool? PPKRujukanName { get; set; }
        public bool? PPKRujukanCode { get; set; }
        public DateTime? ReferDateVisit { get; set; }
        /// </BPJS Rujukan>

        public virtual KioskQueue? KioskQueue { get; set; }
        public virtual ClassTypeDto? ClassType { get; set; }
        public virtual UserDto Patient { get; set; } = new();
        public virtual UserDto? Pratitioner { get; set; }
        public virtual InsuranceDto? Insurance { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual InsurancePolicyDto? InsurancePolicy { get; set; }
    }
}