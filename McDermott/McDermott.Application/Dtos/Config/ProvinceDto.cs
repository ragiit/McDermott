namespace McDermott.Application.Dtos.Config
{
    public class ProvinceDto : IMapFrom<Province>
    {
        public int Id { get; set; }

        [Required]
        public int? CountryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string Code { get; set; } = string.Empty; // State Code

        public CountryDto? Country { get; set; }
    }
}