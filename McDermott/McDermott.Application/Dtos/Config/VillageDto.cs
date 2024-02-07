

namespace McDermott.Application.Dtos.Config
{
    public class VillageDto : IMapFrom<Village>
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; } // Kabupaten

        public int DistrictId { get; set; } // Kecamatan

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan


        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty; // Kode Pos

        public virtual ProvinceDto? Province { get; set; }
        public virtual CityDto? City { get; set; }
        public virtual DistrictDto? District { get; set; }
    }
}