namespace McDermott.Application.Dtos.Config
{
    public class CityDto : IMapFrom<City>
    {
        public int Id { get; set; }
        [Required]
        public int? ProvinceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        public virtual ProvinceDto? Province { get; set; }
    }
}