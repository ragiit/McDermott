namespace McDermott.Application.Dtos.Config
{
    public class OccupationalDto : IMapFrom<Occupational>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; } = string.Empty;
    }
}