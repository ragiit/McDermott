namespace McDermott.Application.Dtos.Config
{
    public class VillageDto : IMapFrom<Village>
    {
        public long Id { get; set; }

        [Required]
        public long? ProvinceId { get; set; }

        [Required]
        public long? CityId { get; set; } // Kabupaten

        [Required]
        public long? DistrictId { get; set; } // Kecamatan

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan

        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty; // Kode Pos

        public virtual ProvinceDto? Province { get; set; }
        public virtual CityDto? City { get; set; }
        public virtual DistrictDto? District { get; set; }
    }

    public class CreateUpdateVillageDto
    {
        public long Id { get; set; }
        public long? ProvinceId { get; set; }
        public long? CityId { get; set; } // Kabupaten
        public long? DistrictId { get; set; } // Kecamatan
        public string Name { get; set; } = string.Empty; // Kelurahan
        public string PostalCode { get; set; } = string.Empty; // Kode Pos
    }
}