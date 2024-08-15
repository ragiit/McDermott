namespace McHealthCare.Application.Dtos.Employees
{
    public class JobPositionDto : IMapFrom<JobPosition>
    {
        public Guid Id { get; set; }
        public Guid? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public DepartmentDto? Department { get; set; }
    }

    public class CreateUpdateJobPositionDto
    {
        public Guid Id { get; set; }
        public Guid? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}