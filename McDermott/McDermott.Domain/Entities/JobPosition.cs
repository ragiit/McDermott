namespace McDermott.Domain.Entities
{
    public class JobPosition : BaseAuditableEntity
    {
        public long? DepartmentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [SetToNull]
        public Department? Department { get; set; }

        [SetToNull]
        public virtual List<User>? Users { get; set; }
    }
}