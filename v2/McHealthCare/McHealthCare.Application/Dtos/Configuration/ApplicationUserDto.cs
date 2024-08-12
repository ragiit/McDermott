using McHealthCare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class ApplicationUserDto : IMapFrom<ApplicationUser>
    {
        public Guid Id { get; set; }    
        public Guid? GroupId { get; set; }
        public Guid? ReligionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EnumGender Gender { get; set; }
        public string? NoId { get; set; } // No Identitas // KTP
        public string? TypeId { get; set; } = "KTP"; // Jenis Identitas
        public string? MartialStatus { get; set; }
        public string? MobilePhone { get; set; }
        public string? CurrentMobile { get; set; }
        public string? HomePhoneNumber { get; set; }
        public string? Npwp { get; set; }
        public string? EmergencyName { get; set; }
        public string? EmergencyRelation { get; set; }
        public string? EmergencyEmail { get; set; }
        public string? EmergencyPhone { get; set; }
        public bool IsResidenceAddress { get; set; }
        public string? IdCardAddress1 { get; set; } // KTP Alamat 1
        public string? IdCardAddress2 { get; set; } // KTP Alamat 1
        public Guid? IdCardCountryId { get; set; } // KTP Negara
        public Guid? IdCardProvinceId { get; set; } // KTP Province
        public Guid? IdCardCityId { get; set; } // KTP City
        public Guid? IdCardDistrictId { get; set; } // KTP District
        public Guid? IdCardVillageId { get; set; } // KTP Village
        public string? IdCardRtRw { get; set; } // KTP  RTRW
        public long? IdCardZip { get; set; } // KTP Zip
        public string? DomicileAddress1 { get; set; }   // Domisili Alamat 1
        public string? DomicileAddress2 { get; set; }   // Domisili Alamat 2
        public Guid? DomicileCountryId { get; set; } // Domisili Negara
        public Guid? DomicileProvinceId { get; set; } // Domisili Province
        public Guid? DomicileCityId { get; set; } // Domisili City
        public Guid? DomicileDistrictId { get; set; } // Domisili District
        public Guid? DomicileVillageId { get; set; } // Domisili Village
        public string? DomicileRtRw { get; set; } // Domisili RtRw
        public long? DomicileZip { get; set; } // Domisili ZIp
        public bool IsDefaultData { get; set; } = false;
        public string? Photo { get; set; }
        public string? PhotoBase64 { get; set; }
        public DateTime? ExpiredId { get; set; }

        public virtual ReligionDto? Religion { get; set; }

        public virtual CountryDto? IdCardCountry { get; set; } // KTP Negara


        public virtual ProvinceDto? IdCardProvince { get; set; } // KTP Province


        public virtual CityDto? IdCardCity { get; set; } // KTP Negara


        public virtual DistrictDto? IdCardDistrict { get; set; } // KTP Negara


        public virtual VillageDto? IdCardVillage { get; set; } // KTP Negara


        public virtual CountryDto? DomicileCountry { get; set; } // Domisili Negara


        public virtual ProvinceDto? DomicileProvince { get; set; } // Domisili Province


        public virtual CityDto? DomicileCity { get; set; } // Domisili City


        public virtual DistrictDto? DomicileDistrict { get; set; } // Domisili District


        public virtual VillageDto? DomicileVillage { get; set; } // Domisili Village
        public virtual GroupDto? Group { get; set; }  
    }
}
