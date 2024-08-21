namespace McDermott.Application.Dtos.Medical
{
    public class ProjectDto : IMapFrom<Project>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(5)]
        public string? Code { get; set; }
    }
}