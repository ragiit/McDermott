namespace McHealthCare.Application.Dtos.Employees
{
    public class DepartmentDto : IMapFrom<Department>
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public string? ManagerId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? DepartmentCategory { get; set; }
        public virtual ApplicationUserDto? Manager { get; set; }
        public virtual DepartmentDto? ParentDepartment { get; set; }
        public virtual CompanyDto? Company { get; set; }
    }

    public class CreateUpdateDepartmentDto
    {
        public Guid Id { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public string? ManagerId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? DepartmentCategory { get; set; }
    }
}