using FluentValidation;

namespace McDermott.Application.Dtos.Config
{
    public class UserDto : IMapFrom<User>
    {
        public long Id { get; set; }
        public long? GroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string PhysicanCode { get; set; } = string.Empty;

        //[Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? MartialStatus { get; set; }
        public string? PlaceOfBirth { get; set; }

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? DateOfBirth { get; set; }

        public string? TypeId { get; set; } = "KTP"; // Jenis Identitas

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NoId { get; set; } // No Identitas // KTP

        public DateTime? ExpiredId { get; set; }
        public string? IdCardAddress1 { get; set; } // KTP Alamat 1
        public string? IdCardAddress2 { get; set; } // KTP Alamat 1
        public long? IdCardCountryId { get; set; } // KTP Negara
        public long? IdCardProvinceId { get; set; } // KTP Province
        public long? IdCardCityId { get; set; } // KTP City
        public long? IdCardDistrictId { get; set; } // KTP District
        public long? IdCardVillageId { get; set; } // KTP Village
        public string? IdCardRtRw { get; set; } // KTP  RTRW
        public long? IdCardZip { get; set; } // KTP Zip
        public string? DomicileAddress1 { get; set; }   // Domisili Alamat 1
        public string? DomicileAddress2 { get; set; }   // Domisili Alamat 2
        public long? DomicileCountryId { get; set; } // Domisili Negara
        public long? DomicileProvinceId { get; set; } // Domisili Province
        public long? DomicileCityId { get; set; } // Domisili City
        public long? DomicileDistrictId { get; set; } // Domisili District
        public long? DomicileVillageId { get; set; } // Domisili Village
        public string? DomicileRtRw { get; set; } // Domisili RtRw
        public long? DomicileZip { get; set; } // Domisili ZIp
        public string? BiologicalMother { get; set; } // Ibu Kandung
        public string? MotherNIK { get; set; }
        public long? ReligionId { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? MobilePhone { get; set; }

        //[RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? CurrentMobile { get; set; }

        public string? Gender { get; set; }
        public string? HomePhoneNumber { get; set; }

        public string? Npwp { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NoBpjsKs { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NoBpjsTk { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? SipNo { get; set; }

        public string? SipFile { get; set; }
        public DateTime? SipExp { get; set; }
        public string? StrNo { get; set; }
        public string? StrFile { get; set; }
        public DateTime? StrExp { get; set; }
        public long? SpecialityId { get; set; }
        public string? UserPhoto { get; set; }
        public long? JobPositionId { get; set; }
        public long? DepartmentId { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? EmergencyEmail { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? EmergencyPhone { get; set; }

        public string? BloodType { get; set; }
        public string? NoRm { get; set; } = "-";
        public string? DoctorCode { get; set; }
        public string? EmployeeCode { get; set; }
        public long? DegreeId { get; set; }
        public bool? IsEmployee { get; set; } = false;
        public bool? IsPharmacy { get; set; } = false;
        public bool? IsPatient { get; set; } = false;
        public bool? IsUser { get; set; } = false;
        public bool IsMcu { get; set; } = false;
        public bool IsHr { get; set; } = false;
        public bool IsSameDomicileAddress { get; set; } = true;
        public bool? IsDoctor { get; set; } = false;
        public bool? IsEmployeeRelation { get; set; } = false;
        public string? EmployeeType { get; set; }

        public string? NIP { get; set; }

        public string? Legacy { get; set; }
        public string? SAP { get; set; }
        public string? Oracle { get; set; }
        private bool _isPhysicion = false;
        private bool _isNurse = false;

        public bool IsPhysicion
        {
            get => _isPhysicion;
            set
            {
                _isPhysicion = value;
                if (value == true)
                {
                    _isNurse = false;
                }
            }
        }

        public bool IsNurse
        {
            get => _isNurse;
            set
            {
                _isNurse = value;
                if (value == true)
                {
                    _isPhysicion = false;
                }
            }
        }

        public string? EmployeeStatus { get; set; }
        public string? FamilyRelation { get; set; }
        public string? TypeNumber { get; set; }
        public string? Numbers { get; set; }
        public string? setNameFamily => $"{Name} ({FamilyRelation})";
        public DateTime? JoinDate { get; set; }
        public List<long>? DoctorServiceIds { get; set; } = [];
        public List<long> PatientAllergyIds { get; set; } = new List<long>();
        public long? SupervisorId { get; set; }  // ID Supervisor
        public long? OccupationalId { get; set; }  // ID Supervisor
        public string IsFamilyMedicalHistory { get; set; } = "No";
        public string? FamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistoryOther { get; set; }
        public string IsMedicationHistory { get; set; } = "No";
        public string? MedicationHistory { get; set; }
        public string? PastMedicalHistory { get; set; }

        //[SetToNull]
        public UserDto? Supervisor { get; set; }  // Referensi ke Supervisor

        //[SetToNull]
        public virtual SpecialityDto? Speciality { get; set; }

        //[SetToNull]
        //public virtual GenderDto? Gender { get; set; }

        //[SetToNull]
        public virtual Country? IdCardCountry { get; set; } // KTP Negara

        //[SetToNull]
        public virtual GroupDto? Group { get; set; }

        //[SetToNull]
        public virtual JobPositionDto? JobPosition { get; set; }

        //[SetToNull]
        public virtual DepartmentDto? Department { get; set; }

        //[SetToNull]
        public virtual OccupationalDto? Occupational { get; set; }

        //[SetToNull]
        public virtual PatientAllergyDto PatientAllergy { get; set; } = new();
    }

    public class GCGUserFormValidator : AbstractValidator<UserDto>
    {
        public GCGUserFormValidator()
        {
            RuleFor(x => x.CurrentMobile)
                .NotEmpty().WithMessage("Current Mobile is required");
        }
    }

    public class CreateUpdateUserDto
    {
        public long Id { get; set; }
        public long? GroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string PhysicanCode { get; set; } = string.Empty;

        //[Required]
        public string UserName { get; set; } = string.Empty;

        public string? Gender { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? MartialStatus { get; set; }
        public string? PlaceOfBirth { get; set; }

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? DateOfBirth { get; set; }

        public string? TypeId { get; set; } = "KTP"; // Jenis Identitas

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NoId { get; set; } // No Identitas // KTP

        public DateTime? ExpiredId { get; set; }
        public string? IdCardAddress1 { get; set; } // KTP Alamat 1
        public string? IdCardAddress2 { get; set; } // KTP Alamat 1
        public long? IdCardCountryId { get; set; } // KTP Negara
        public long? IdCardProvinceId { get; set; } // KTP Province
        public long? IdCardCityId { get; set; } // KTP City
        public long? IdCardDistrictId { get; set; } // KTP District
        public long? IdCardVillageId { get; set; } // KTP Village
        public string? IdCardRtRw { get; set; } // KTP  RTRW
        public long? IdCardZip { get; set; } // KTP Zip
        public string? DomicileAddress1 { get; set; }   // Domisili Alamat 1
        public string? DomicileAddress2 { get; set; }   // Domisili Alamat 2
        public long? DomicileCountryId { get; set; } // Domisili Negara
        public long? DomicileProvinceId { get; set; } // Domisili Province
        public long? DomicileCityId { get; set; } // Domisili City
        public long? DomicileDistrictId { get; set; } // Domisili District
        public long? DomicileVillageId { get; set; } // Domisili Village
        public string? DomicileRtRw { get; set; } // Domisili RtRw
        public long? DomicileZip { get; set; } // Domisili ZIp
        public string? BiologicalMother { get; set; } // Ibu Kandung
        public string? MotherNIK { get; set; }
        public long? ReligionId { get; set; }

        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? MobilePhone { get; set; }

        //[RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? CurrentMobile { get; set; }

        public string? HomePhoneNumber { get; set; }

        public string? Npwp { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NoBpjsKs { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? NoBpjsTk { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? SipNo { get; set; }

        public string? SipFile { get; set; }
        public DateTime? SipExp { get; set; }
        public string? StrNo { get; set; }
        public string? StrFile { get; set; }
        public DateTime? StrExp { get; set; }
        public long? SpecialityId { get; set; }
        public string? UserPhoto { get; set; }
        public long? JobPositionId { get; set; }
        public long? DepartmentId { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? EmergencyEmail { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? EmergencyPhone { get; set; }

        public string? BloodType { get; set; }
        public string? NoRm { get; set; } = "-";
        public string? DoctorCode { get; set; }
        public string? EmployeeCode { get; set; }
        public long? DegreeId { get; set; }
        public bool? IsEmployee { get; set; } = false;
        public bool? IsPharmacy { get; set; } = false;
        public bool? IsPatient { get; set; } = false;
        public bool? IsUser { get; set; } = false;
        public bool IsMcu { get; set; } = false;
        public bool IsHr { get; set; } = false;
        public bool IsSameDomicileAddress { get; set; } = true;
        public bool? IsDoctor { get; set; } = false;
        public bool? IsEmployeeRelation { get; set; } = false;
        public string? EmployeeType { get; set; }

        public string? NIP { get; set; }

        public string? Legacy { get; set; }
        public string? SAP { get; set; }
        public string? Oracle { get; set; }
        private bool _isPhysicion = false;
        private bool _isNurse = false;

        public bool IsPhysicion
        {
            get => _isPhysicion;
            set
            {
                _isPhysicion = value;
                if (value == true)
                {
                    _isNurse = false;
                }
            }
        }

        public bool IsNurse
        {
            get => _isNurse;
            set
            {
                _isNurse = value;
                if (value == true)
                {
                    _isPhysicion = false;
                }
            }
        }

        public string? EmployeeStatus { get; set; }
        public string? FamilyRelation { get; set; }
        public string? TypeNumber { get; set; }
        public string? Numbers { get; set; }
        public string? setNameFamily => $"{Name} ({FamilyRelation})";
        public DateTime? JoinDate { get; set; }
        public List<long>? DoctorServiceIds { get; set; } = [];
        public List<long> PatientAllergyIds { get; set; } = new List<long>();
        public long? SupervisorId { get; set; }  // ID Supervisor
        public long? OccupationalId { get; set; }  // ID Supervisor
        public string? IsFamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistoryOther { get; set; }
        public string? IsMedicationHistory { get; set; }
        public string? MedicationHistory { get; set; }
        public string? PastMedicalHistory { get; set; }
    }
}