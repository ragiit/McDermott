namespace McHealthCare.Domain.Entities
{
    public class ConcoctionLine : BaseAuditableEntity
    {
        public Guid? ConcoctionId { get; set; }
        public Guid? ProductId { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public Guid? UomId { get; set; }
        public Guid? MedicamentDosage { get; set; }
        public Guid? MedicamentUnitOfDosage { get; set; }
        public Guid? Qty { get; set; }
        public Guid? TotalQty { get; set; }
        public Guid? AvaliableQty { get; set; }

        
        public Product? Product { get; set; }

        
        public List<ActiveComponent>? ActiveComponent { get; set; }

        
        public Concoction? Concoction { get; set; }

        
        public Uom? Uom { get; set; }
    }
}