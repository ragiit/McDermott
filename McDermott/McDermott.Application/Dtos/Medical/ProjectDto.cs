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

    public class CreateUpdateProjectDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}