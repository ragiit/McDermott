namespace McDermott.Application.Dtos.Employee
{
    public class DepartmentDto : IMapFrom<Department>
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? ParentDepartmentId { get; set; }
        public int? ManagerId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? DepartmentCategory { get; set; }

        public virtual UserDto? Manager { get; set; }
        public virtual DepartmentDto? ParentDepartment { get; set; }
        public virtual CompanyDto? Company { get; set; }
    }
}