

namespace McDermott.Domain.Entities
{
    public class UomCategory : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Type { get; set; }

        public virtual List<Uom>? Uoms { get; set; }
    }
}
