using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos
{
    public class ProvinceDto : IMapFrom<Province>
    {
        public int Id { get; set; }
        public int CountryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [Required]
        [StringLength(5)]
        public string Code { get; set; } = string.Empty; // State Code

        public CountryDto? Country { get; set; }
    }
}