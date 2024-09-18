namespace McDermott.Application.Dtos.Config
{
    public class CityDto : IMapFrom<City>
    {
        public long Id { get; set; }

        [Required]
        public long? ProvinceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        public virtual ProvinceDto? Province { get; set; }
    }

    public class CreateUpdateCityDto
    {
        public long Id { get; set; }
        public long? ProvinceId { get; set; }
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota
    }
}