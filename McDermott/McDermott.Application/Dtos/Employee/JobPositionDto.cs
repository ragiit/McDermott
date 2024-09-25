namespace McDermott.Application.Dtos.Employee
{
    public class JobPositionDto : IMapFrom<JobPosition>
    {
        public long Id { get; set; }
        public long? DepartmentId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual DepartmentDto? Department { get; set; }
    }

    public class CreateUpdateJobPositionDto
    {
        public long Id { get; set; }
        public long? DepartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}