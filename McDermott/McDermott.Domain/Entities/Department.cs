namespace McDermott.Domain.Entities
{
    public partial class Department : BaseAuditableEntity
    { 
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; }

        [StringLength(200)]
        public string? ParentName { get; set; }

        public int? CompanyId { get; set; }
        public string? DepartmentCategory { get; set; }
        public string? Manager { get; set; }

        public virtual Company? Company { get; set; } 
    }
}