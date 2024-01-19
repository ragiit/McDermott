namespace McDermott.Domain.Entities
{
    public partial class Gender : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}