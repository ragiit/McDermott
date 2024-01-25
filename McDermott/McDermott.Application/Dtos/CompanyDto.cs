using System.ComponentModel.DataAnnotations;

namespace McDermott.Application.Dtos
{
    public class CompanyDto : IMapFrom<Company>
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public int CountryId { get; set; }

        public int? CurrencyId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(500)]
        public string Website { get; set; } = string.Empty;

        [StringLength(200)]
        public string VAT { get; set; } = string.Empty;

        [StringLength(300)]
        public string Street1 { get; set; } = string.Empty;

        [StringLength(200)]
        public string Street2 { get; set; } = string.Empty;

        [StringLength(5)]
        public string Zip { get; set; } = string.Empty;

        public string Logo { get; set; } = string.Empty;

        //Jon Table
        //public CityDto? City { get; set; }

        //public ProvinceDto? Province { get; set; }
        //public CountryDto? Country { get; set; }
        //public Currency? Currency { get; set; }
    }
}