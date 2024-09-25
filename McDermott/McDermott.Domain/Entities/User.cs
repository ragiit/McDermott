namespace McDermott.Domain.Entities
{
    public partial class User : BaseAuditableEntity
    {
        public long? GroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        //[Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string? Gender { get; set; }
        public string? MartialStatus { get; set; }
        public string? PlaceOfBirth { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? TypeId { get; set; } = "KTP"; // Jenis Identitas
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
        public string? MobilePhone { get; set; }
        public string? CurrentMobile { get; set; }
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
        public bool IsSameDomicileAddress { get; set; } = true;
        public long? SpecialityId { get; set; }
        public string? UserPhoto { get; set; }
        public long? JobPositionId { get; set; }
        public long? DepartmentId { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? EmergencyEmail { get; set; }
        public string? EmergencyPhone { get; set; }
        public string? BloodType { get; set; }
        public string? NoRm { get; set; } = "-";
        public string? DoctorCode { get; set; }
        public string? EmployeeCode { get; set; }
        public long? DegreeId { get; set; }
        public bool IsEmployee { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsDefaultData { get; set; } = false;
        public bool IsPatient { get; set; } = false;
        public bool IsUser { get; set; } = false;
        public bool IsDoctor { get; set; } = false;
        public bool IsPhysicion { get; set; } = false;
        public bool IsNurse { get; set; } = false;
        public bool IsPharmacy { get; set; } = false;
        public bool IsMcu { get; set; } = false;
        public bool IsHr { get; set; } = false;
        public string PhysicanCode { get; set; } = string.Empty;
        public bool? IsEmployeeRelation { get; set; }
        public string? EmployeeType { get; set; }
        public string? EmployeeStatus { get; set; }
        public DateTime? JoinDate { get; set; }

        public string? NIP { get; set; }
        public string? Legacy { get; set; }
        public string? SAP { get; set; }
        public string? Oracle { get; set; }
        public List<long>? DoctorServiceIds { get; set; }
        public List<long> PatientAllergyIds { get; set; } = [];
        public bool IsWeatherPatientAllergyIds { get; set; }
        public bool IsPharmacologyPatientAllergyIds { get; set; }
        public bool IsFoodPatientAllergyIds { get; set; }
        public List<long> WeatherPatientAllergyIds { get; set; } = [];
        public List<long> PharmacologyPatientAllergyIds { get; set; } = [];
        public List<long> FoodPatientAllergyIds { get; set; } = []; public long? SupervisorId { get; set; }
        public long? OccupationalId { get; set; }

        public string? IsFamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistory { get; set; }
        public string? FamilyMedicalHistoryOther { get; set; }
        public string? IsMedicationHistory { get; set; }
        public string? MedicationHistory { get; set; }
        public string? PastMedicalHistory { get; set; }

        #region Relation

        //[SetToNull]
        public User? Supervisor { get; set; }

        //[SetToNull]
        public Occupational? Occupational { get; set; }

        //[SetToNull]
        public virtual Group? Group { get; set; }

        //[SetToNull]
        //public virtual Gender? Gender { get; set; }

        //[SetToNull]
        public virtual Country? IdCardCountry { get; set; } // KTP Negara

        //[SetToNull]
        public virtual Province? IdCardProvince { get; set; } // KTP Province

        //[SetToNull]
        public virtual City? IdCardCity { get; set; } // KTP Negara

        //[SetToNull]
        public virtual District? IdCardDistrict { get; set; } // KTP Negara

        //[SetToNull]
        public virtual Village? IdCardVillage { get; set; } // KTP Negara

        //[SetToNull]
        public virtual Country? DomicileCountry { get; set; } // Domisili Negara

        //[SetToNull]
        public virtual Province? DomicileProvince { get; set; } // Domisili Province

        //[SetToNull]
        public virtual City? DomicileCity { get; set; } // Domisili City

        //[SetToNull]
        public virtual District? DomicileDistrict { get; set; } // Domisili District

        //[SetToNull]
        public virtual Village? DomicileVillage { get; set; } // Domisili Village

        //[SetToNull]
        public virtual Religion? Religion { get; set; }

        //[SetToNull]
        public virtual Speciality? Speciality { get; set; }

        //[SetToNull]
        public virtual JobPosition? JobPosition { get; set; }

        //[SetToNull]
        public virtual Department? Department { get; set; }

        //[SetToNull]
        public virtual Degree? Degree { get; set; }

        //[SetToNull]
        public virtual List<PatientAllergy>? PatientAllergies { get; set; }

        #endregion Relation
    }
}