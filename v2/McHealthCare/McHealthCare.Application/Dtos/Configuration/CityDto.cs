using Mapster;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Common.Interfaces;
using McHealthCare.Domain.Entities.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McHealthCare.Application.Dtos.Configuration

{
    public class CityDto : IMapFrom<City> 
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        public virtual ProvinceDto? Province { get; set; }

        public virtual List<VillageDto>? Villages { get; set; }
         
    }
}