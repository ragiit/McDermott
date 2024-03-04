namespace McDermott.Application.Dtos.Config
{
    public class DistrictDto : IMapFrom<District>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kecamatan

        public int? CityId { get; set; } // Kabupaten
        public int? ProvinceId { get; set; }

        public virtual CityDto? City { get; set; }
        public virtual ProvinceDto? Province { get; set; }
    }
}