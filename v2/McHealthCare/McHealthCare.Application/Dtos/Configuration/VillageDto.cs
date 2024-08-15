namespace McHealthCare.Application.Dtos.Configuration
{
    public class VillageDto : IMapFrom<Village>
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid CityId { get; set; } // Kabupaten

        public Guid DistrictId { get; set; } // Kecamatan

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan

        [StringLength(10)]
        public string? PostalCode { get; set; }

        public virtual ProvinceDto? Province { get; set; }

        public virtual CityDto? City { get; set; }
    }

    public class CreateUpdateVillageDto : IMapFrom<Village>
    {
        public Guid Id { get; set; }
        public Guid ProvinceId { get; set; }
        public Guid CityId { get; set; } // Kabupaten

        public Guid DistrictId { get; set; } // Kecamatan

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan

        [StringLength(10)]
        public string? PostalCode { get; set; }
    }
}