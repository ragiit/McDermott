namespace McDermott.Domain.Entities
{
    public partial class Country : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        [SetToNull]
        public virtual List<Company>? Companies { get; set; }
        [SetToNull]
        public virtual List<Province>? Provinces { get; set; }
    }
}