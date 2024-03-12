namespace McDermott.Domain.Entities
{
    public partial class Degree : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [SetToNull]
        public virtual List<User>? Users { get; set; }
    }
}