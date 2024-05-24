namespace McDermott.Domain.Entities
{
    public class ConcoctionLine : BaseAuditableEntity
    {
        public long? ConcoctionId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? ActiveComponentId { get; set; }
        public long? UomId { get; set; }
        public long? MedicamentDosage { get; set; }
        public long? MedicamentUnitOfDosage { get; set; }
        public long? Qty { get; set; }
        public long? TotalQty { get; set; }
        public long? AvaliableQty { get; set; }

        public MedicamentGroup? MedicamentGroup { get; set; }
        public ActiveComponent? ActiveComponent { get; set; }
        public Concoction? Concoction { get; set; }
        public Uom? Uom { get; set; }
    }
}