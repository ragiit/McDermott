namespace McHealthCare.Domain.Entities.Pharmacies
{
    public class ActiveComponent : BaseAuditableEntity
    {
        public Guid? UomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? AmountOfComponent { get; set; }

        public virtual Uom? Uom { get; set; }

        public virtual List<MedicamentGroupDetail>? MedicamentGroupDetails { get; set; }
    }
}