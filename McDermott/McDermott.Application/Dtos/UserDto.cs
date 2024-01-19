using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int? NoRm { get; set; }
        public int? GenderId { get; set; }
        public string? MartialStatus { get; set; }
        public string? PlaceOfBirth { get; set; }

        [DisplayFormat(DataFormatString = "dd MMMM yyyy")]
        public DateTime? DateOfBirth { get; set; } = DateTime.UtcNow;

        public string? TypeId { get; set; } = "KTP"; // Jenis Identitas
        public int? NoId { get; set; } // No Identitas
        public DateTime? ExpiredId { get; set; } = DateTime.UtcNow;  // Expired Identitas
        public string? IdCardAddress1 { get; set; } // KTP Alamat 1
        public string? IdCardAddress2 { get; set; } // KTP Alamat 1
        public int? IdCardCountryId { get; set; } // KTP Negara
        public int? IdCardProvinceId { get; set; } // KTP Province
        public int? IdCardCityId { get; set; } // KTP City
        public int? IdCardDistrictId { get; set; } // KTP District
        public int? IdCardVillageId { get; set; } // KTP Village
        public int? IdCardRtRw { get; set; } // KTP  RTRW
        public int? IdCardZip { get; set; } // KTP Zip
        public string? DomicileAddress1 { get; set; }   // Domisili Alamat 1
        public string? DomicileAddress2 { get; set; }   // Domisili Alamat 2
        public int? DomicileCountryId { get; set; } // Domisili Negara
        public int? DomicileProvinceId { get; set; } // Domisili Province
        public int? DomicileCityId { get; set; } // Domisili City
        public int? DomicileDistrictId { get; set; } // Domisili District
        public int? DomicileVillageId { get; set; } // Domisili Village
        public int? DomicileRtRw { get; set; } // Domisili RtRw
        public int? DomicileZip { get; set; } // Domisili ZIp
        public string? BiologicalMother { get; set; } // Ibu Kandung
        public string? MotherNIK { get; set; }
        public int? ReligionId { get; set; }
        public int? MobilePhone { get; set; }
        public int? HomePhoneNumber { get; set; }
        public string? Npwp { get; set; }
        public int? NoBpjsKs { get; set; }
        public int? NoBpjsTk { get; set; }
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
        public int? EmergencyPhone { get; set; }
        public string? BloodType { get; set; }
        public string? DoctorCode { get; set; }
        public string? EmployeeCode { get; set; }
        public int? DegreeId { get; set; }
        public bool? IsEmployee { get; set; } = false;
        public bool? IsPatient { get; set; } = false;
        public bool? IsUser { get; set; } = false;
        public bool? IsDoctor { get; set; } = false;

        public bool? IsPhysicion { get; set; } = false;
        public bool? IsNurse { get; set; } = false;
        public string? EmployeeStatus { get; set; }
        public DateTime? JoinDate { get; set; }
    }
}