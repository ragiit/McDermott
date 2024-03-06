namespace McDermott.Domain.Entities
{
    public partial class Department : BaseAuditableEntity
    {
        public long? CompanyId { get; set; }
        public long? ParentDepartmentId { get; set; }
        public long? ManagerId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? DepartmentCategory { get; set; }

        [SetToNull]
        public virtual User? Manager { get; set; }
        [SetToNull]
        public virtual Department? ParentDepartment { get; set; }
        [SetToNull]
        public virtual Company? Company { get; set; }
    }
}