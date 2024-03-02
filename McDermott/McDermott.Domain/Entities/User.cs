namespace McDermott.Domain.Entities
{
    public partial class User : BaseAuditableEntity
    {
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

        public DateTime? DateOfBirth { get; set; }

        public string? TypeId { get; set; } = "KTP"; // Jenis Identitas
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
        public string? MobilePhone { get; set; }
        public string? HomePhoneNumber { get; set; }
        public string? Npwp { get; set; }
        public string? NoBpjsKs { get; set; }
        public string? NoBpjsTk { get; set; }
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
        public string? EmergencyPhone { get; set; }
        public string? BloodType { get; set; }
        public string? NoRm { get; set; } = "-";
        public string? DoctorCode { get; set; }
        public string? EmployeeCode { get; set; }
        public int? DegreeId { get; set; }
        public bool? IsEmployee { get; set; }
        public bool? IsPatient { get; set; }
        public bool? IsUser { get; set; }
        public bool? IsDoctor { get; set; }
        public bool? IsPhysicion { get; set; }
        public bool? IsNurse { get; set; }
        public bool? IsEmployeeRelation { get; set; }
        public string? EmployeeType { get; set; }
        public string? EmployeeStatus { get; set; }
        public DateTime? JoinDate { get; set; }
        public string? NIP { get; set; }
        public string? Legacy { get; set; }
        public string? SAP { get; set; }
        public string? Oracle { get; set; }
        public List<int>? DoctorServiceIds { get; set; }

        public virtual Group? Group { get; set; }
        public virtual Gender? Gender { get; set; }
        public virtual Country? IdCardCountry { get; set; } // KTP Negara
        public virtual Province? IdCardProvince { get; set; } // KTP Province
        public virtual City? IdCardCity { get; set; } // KTP Negara
        public virtual District? IdCardDistrict { get; set; } // KTP Negara
        public virtual Village? IdCardVillage { get; set; } // KTP Negara
        public virtual Country? DomicileCountry { get; set; } // Domisili Negara
        public virtual Province? DomicileProvince { get; set; } // Domisili Province
        public virtual City? DomicileCity { get; set; } // Domisili City
        public virtual District? DomicileDistrict { get; set; } // Domisili District
        public virtual Village? DomicileVillage { get; set; } // Domisili Village
        public virtual Religion? Religion { get; set; }
        public virtual Speciality? Speciality { get; set; }
        public virtual JobPosition? JobPosition { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Degree? Degree { get; set; }
    }
}