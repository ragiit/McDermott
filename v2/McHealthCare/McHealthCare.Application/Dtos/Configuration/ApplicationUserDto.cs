using McHealthCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
        public string? Password { get; set; }
        public string? UserName => Email; 
         
        public virtual string? NormalizedUserName { get; set; }
         
        [ProtectedPersonalData]
        public virtual string? Email { get; set; } 
        public virtual string? NormalizedEmail { get; set; }
         
        [PersonalData]
        public virtual bool EmailConfirmed { get; set; } 
        public virtual string? PasswordHash { get; set; } 
        public virtual string? SecurityStamp { get; set; } 
        public string? ConcurrencyStamp { get; set; }
        [ProtectedPersonalData]
        public string? PhoneNumber { get; set; }
         
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }
         
        public  DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
         
        public int AccessFailedCount { get; set; } 
        public string? PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public EnumGender Gender { get; set; }
        public string? NoId { get; set; } 
        public string? TypeId { get; set; } = "KTP"; 
        public string? MartialStatus { get; set; }
        [Required]
        public string? MobilePhone { get; set; }
        [Required]
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

        public ReligionDto? Religion { get; set; } 
        public CountryDto? IdCardCountry { get; set; } 
        public ProvinceDto? IdCardProvince { get; set; }  
        public CityDto? IdCardCity { get; set; }  
        public DistrictDto? IdCardDistrict { get; set; }  
        public VillageDto? IdCardVillage { get; set; }  
        public CountryDto? DomicileCountry { get; set; }  
        public ProvinceDto? DomicileProvince { get; set; }  
        public CityDto? DomicileCity { get; set; }  
        public DistrictDto? DomicileDistrict { get; set; } 
        public VillageDto? DomicileVillage { get; set; }
        public GroupDto? Group { get; set; } 
        public PatientDto? Patient { get; set; }    
        public EmployeeDto? Employee { get; set; }  // Navigation property
        public DoctorDto? Doctor { get; set; }  // Navigation property
    }
}
