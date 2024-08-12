namespace McDermott.Domain.Entities
{
    public class ConcoctionLine : BaseAuditableEntity
    {
        public long? ConcoctionId { get; set; }
        public long? ProductId { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public long? UomId { get; set; }
        public long? MedicamentDosage { get; set; }
        public long? MedicamentUnitOfDosage { get; set; }
        public long? Dosage { get; set; }
        public long? TotalQty { get; set; }
        public long? AvaliableQty { get; set; }

        [SetToNull]
        public Product? Product { get; set; }

        [SetToNull]
        public List<ActiveComponent>? ActiveComponent { get; set; }

        [SetToNull]
        public Concoction? Concoction { get; set; }

        [SetToNull]
        public Uom? Uom { get; set; }
    }
}