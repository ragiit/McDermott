namespace McDermott.Application.Dtos.Medical
{
    public class BuildingDto : IMapFrom<Building>
    {
        public long Id { get; set; }
        public long? HealthCenterId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        //[Required]
        //[StringLength(200)]
        public string Code { get; set; } = string.Empty;

        public HealthCenterDto? HealthCenter { get; set; }
    }
}