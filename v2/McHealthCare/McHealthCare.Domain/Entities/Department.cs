namespace McHealthCare.Domain.Entities
{
    public partial class Department : BaseAuditableEntity
    {
        public Guid? CompanyId { get; set; }
        public Guid? ParentDepartmentId { get; set; }
        public Guid? ManagerId { get; set; }

        [StringLength(200)]
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? DepartmentCategory { get; set; }

        //public virtual User? Manager { get; set; }

        public virtual Department? ParentDepartment { get; set; }

        public virtual Company? Company { get; set; }
    }
}