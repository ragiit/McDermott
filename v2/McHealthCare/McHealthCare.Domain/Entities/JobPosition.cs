namespace McHealthCare.Domain.Entities
{
    public class JobPosition : BaseAuditableEntity
    {
        public Guid? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        
        public Department? Department { get; set; }

        
        // public virtual List<User>? Users { get; set; }
    }
}