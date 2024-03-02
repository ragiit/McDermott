namespace McDermott.Application.Dtos.Employee
{
    public class JobPositionDto : IMapFrom<JobPosition>
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public virtual DepartmentDto? Department { get; set; }
    }
}