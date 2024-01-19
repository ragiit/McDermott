namespace McDermott.Domain.Entities
{
    public class JobPosition : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}