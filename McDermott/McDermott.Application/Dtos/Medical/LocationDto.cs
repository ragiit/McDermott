namespace McDermott.Application.Dtos.Medical
{
    public class LocationDto : IMapFrom<Location>
    {
         public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(200)]
        public string Type { get; set; }
    }
}