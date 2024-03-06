namespace McDermott.Application.Dtos.Config
{
    public class DistrictDto : IMapFrom<District>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kecamatan
        [Required]
        public long? CityId { get; set; } // Kabupaten
        [Required]
        public long? ProvinceId { get; set; }

        public virtual CityDto? City { get; set; }
        public virtual ProvinceDto? Province { get; set; }
    }
}