using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos.Config
{
    public class CountryDto : IMapFrom<Country>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;


        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}