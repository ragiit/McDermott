namespace McHealthCare.Domain.Entities
{
    public class MedicamentGroupDetail : BaseAuditableEntity
    {
        public Guid? MedicamentGroupId { get; set; }
        public Guid? MedicamentId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public Guid? SignaId { get; set; }
        public Guid? FrequencyId { get; set; }
        public Guid? UnitOfDosageId { get; set; }        
        public Guid? Dosage { get; set; }
        public Guid? QtyByDay { get; set; }
        public Guid? Days { get; set; }
        public Guid? TotalQty { get; set; }
        public bool? AllowSubtitation { get; set; }
        public string? MedicaneUnitDosage { get; set; }
        public string? MedicaneDosage { get; set; }
        public string? MedicaneName { get; set; }
        public string? Comment { get; set; }


        
        public Product? Medicament { get; set; }
        
        public  List<ActiveComponent>? ActiveComponent { get; set; }
        
        public  MedicamentGroup? MedicamentGroup { get; set; }        
        
        public  Uom? UnitOfDosage { get; set; }
        
        public DrugDosage? Frequency { get; set; }
        
        public Signa? Signa { get; set; }
    }
}
