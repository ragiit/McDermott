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
        public string Id { get; set; } = Guid.Empty.ToString();   
        public Guid? GroupId { get; set; }
        public Guid? ReligionId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; 
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EnumGender Gender { get; set; }
        public string? NoId { get; set; } 
        public string? TypeId { get; set; } = "KTP"; 
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
        public string? IdCardAddress1 { get; set; } 
        public string? IdCardAddress2 { get; set; } 
        public Guid? IdCardCountryId { get; set; } 
        public Guid? IdCardProvinceId { get; set; } 
        public Guid? IdCardCityId { get; set; } 
        public Guid? IdCardDistrictId { get; set; } 
        public Guid? IdCardVillageId { get; set; } 
        public string? IdCardRtRw { get; set; } 
        public long? IdCardZip { get; set; } 
        public string? DomicileAddress1 { get; set; }   
        public string? DomicileAddress2 { get; set; }   
        public Guid? DomicileCountryId { get; set; } 
        public Guid? DomicileProvinceId { get; set; } 
        public Guid? DomicileCityId { get; set; } 
        public Guid? DomicileDistrictId { get; set; } 
        public Guid? DomicileVillageId { get; set; } 
        public string? DomicileRtRw { get; set; } 
        public long? DomicileZip { get; set; } 
        public bool IsDefaultData { get; set; } = false;
        public string? Photo { get; set; }
        public string? PhotoBase64 { get; set; }
        public DateTime? ExpiredId { get; set; }

        public virtual ReligionDto? Religion { get; set; } 
        public virtual CountryDto? IdCardCountry { get; set; } 
        public virtual ProvinceDto? IdCardProvince { get; set; }  
        public virtual CityDto? IdCardCity { get; set; }  
        public virtual DistrictDto? IdCardDistrict { get; set; }  
        public virtual VillageDto? IdCardVillage { get; set; }  
        public virtual CountryDto? DomicileCountry { get; set; }  
        public virtual ProvinceDto? DomicileProvince { get; set; }  
        public virtual CityDto? DomicileCity { get; set; }  
        public virtual DistrictDto? DomicileDistrict { get; set; } 
        public virtual VillageDto? DomicileVillage { get; set; }  
        public virtual GroupDto? Group { get; set; }  
    }
}
