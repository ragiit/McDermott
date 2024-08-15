namespace McHealthCare.Domain.Entities.Pharmacies
{
    public class ConcoctionLine : BaseAuditableEntity
    {
        public Guid? ConcoctionId { get; set; }
        public Guid? ProductId { get; set; }
        public List<Guid>? ActiveComponentId { get; set; } = [];
        public Guid? UomId { get; set; }
        public Guid? MedicamentDosage { get; set; }
        public Guid? MedicamentUnitOfDosage { get; set; }
        public long? Dosage { get; set; }
        public long? TotalQty { get; set; }
        public long? AvaliableQty { get; set; }

        
        public Product? Product { get; set; }

        
        public List<ActiveComponent>? ActiveComponent { get; set; }

        
        public Concoction? Concoction { get; set; }

        
        public Uom? Uom { get; set; }
    }
}