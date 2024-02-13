

namespace McDermott.Application.Dtos.Config
{
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        //[Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int? GenderId { get; set; }
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
        public int? IdCardCountryId { get; set; } // KTP Negara
        public int? IdCardProvinceId { get; set; } // KTP Province
        public int? IdCardCityId { get; set; } // KTP City
        public int? IdCardDistrictId { get; set; } // KTP District
        public int? IdCardVillageId { get; set; } // KTP Village
        public string? IdCardRtRw { get; set; } // KTP  RTRW
        public int? IdCardZip { get; set; } // KTP Zip
        public string? DomicileAddress1 { get; set; }   // Domisili Alamat 1
        public string? DomicileAddress2 { get; set; }   // Domisili Alamat 2
        public int? DomicileCountryId { get; set; } // Domisili Negara
        public int? DomicileProvinceId { get; set; } // Domisili Province
        public int? DomicileCityId { get; set; } // Domisili City
        public int? DomicileDistrictId { get; set; } // Domisili District
        public int? DomicileVillageId { get; set; } // Domisili Village
        public string? DomicileRtRw { get; set; } // Domisili RtRw
        public int? DomicileZip { get; set; } // Domisili ZIp
        public string? BiologicalMother { get; set; } // Ibu Kandung
        public string? MotherNIK { get; set; }
        public int? ReligionId { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? MobilePhone { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
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
        public int? SpecialityId { get; set; }
        public string? UserPhoto { get; set; }
        public int? JobPositionId { get; set; }
        public int? DepartmentId { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? EmergencyEmail { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
        public string? EmergencyPhone { get; set; }
        public string? BloodType { get; set; }
        public string? NoRm { get; set; } = "-";
        public string? DoctorCode { get; set; }
        public string? EmployeeCode { get; set; }
        public int? DegreeId { get; set; }
        public bool? IsEmployee { get; set; } = false;
        public bool? IsPatient { get; set; } = false;
        public bool? IsUser { get; set; } = false;
        public bool? IsDoctor { get; set; } = false;
        public bool? IsEmployeeRelation { get; set; } = false;
        public string? EmployeeType { get; set; }

        //[Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "The {0} field must contain only numbers.")]
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
        public DateTime? JoinDate { get; set; }

        public virtual GroupDto? Group { get; set; }
        public virtual JobPositionDto? JobPosition { get; set; }
        public virtual DepartmentDto? Department { get; set; }
        public virtual PatientAllergyDto PatientAllergy { get; set; } = new();
    }
}