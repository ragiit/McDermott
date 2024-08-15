namespace McHealthCare.Application.Dtos.Configuration
{
    public class DistrictDto : IMapFrom<District>
    {
        public Guid Id { get; set; }

        [Required]
        public Guid? CityId { get; set; }

        [Required]
        public Guid? ProvinceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public virtual CityDto? City { get; set; }
        public virtual ProvinceDto? Province { get; set; }
    }

    public class CreateUpdateDistrictDto : IMapFrom<District>
    {
        public Guid Id { get; set; }

        public Guid CityId { get; set; }
        public Guid ProvinceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}