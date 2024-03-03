namespace McDermott.Application.Dtos.Config
{
    public class ReligionDto : IMapFrom<Religion>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}