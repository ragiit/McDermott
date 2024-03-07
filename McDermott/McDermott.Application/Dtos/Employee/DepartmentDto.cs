namespace McDermott.Application.Dtos.Employee
{
    public class DepartmentDto : IMapFrom<Department>
    {
         public long Id { get; set; }
        public long? CompanyId { get; set; }
        public long? ParentDepartmentId { get; set; }
        public long? ManagerId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? DepartmentCategory { get; set; }

        public virtual UserDto? Manager { get; set; }
        public virtual DepartmentDto? ParentDepartment { get; set; }
        public virtual CompanyDto? Company { get; set; }
    }
}