using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos
{
    public class CityDto : IMapFrom<City>
    {
        public int Id { get; set; }
        public int ProvinceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        public virtual ProvinceDto? Province { get; set; }
    }
}