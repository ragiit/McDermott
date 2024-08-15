using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class ProjectDto : IMapFrom<Project>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string? Code { get; set; }
    }

    public class CreateUpdateProjectDto : IMapFrom<Project>
    {
        public Guid Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string? Code { get; set; }
    }
}