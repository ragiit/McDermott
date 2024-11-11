namespace McDermott.Domain.Entities
{
    public class ActiveComponent : BaseAuditableEntity
    {
        public long? UomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? AmountOfComponent { get; set; }

        [SetToNull]
        public virtual Uom? Uom { get; set; }

        public virtual List<MedicamentGroupDetail>? MedicamentGroupDetails { get; set; }
    }
}