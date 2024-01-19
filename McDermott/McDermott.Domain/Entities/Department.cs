namespace McDermott.Domain.Entities
{
    public partial class Department : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}