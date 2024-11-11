namespace McDermott.Domain.Entities
{
    public class MedicamentGroupDetail : BaseAuditableEntity
    {
        public long? MedicamentGroupId { get; set; }
        public long? MedicamentId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public long? SignaId { get; set; }
        public long? FrequencyId { get; set; }
        public long? UnitOfDosageId { get; set; }        
        public long? Dosage { get; set; }
        public long? QtyByDay { get; set; }
        public long? Days { get; set; }
        public long? TotalQty { get; set; }
        public bool? AllowSubtitation { get; set; }
        public string? MedicaneUnitDosage { get; set; }
        public string? MedicaneDosage { get; set; }
        public string? MedicaneName { get; set; }
        public string? Comment { get; set; }


        [SetToNull]
        public Product? Medicament { get; set; }
        [SetToNull]
        public  List<ActiveComponent>? ActiveComponent { get; set; }
        [SetToNull]
        public  MedicamentGroup? MedicamentGroup { get; set; }        
        [SetToNull]
        public  Uom? UnitOfDosage { get; set; }
        [SetToNull]
        public DrugDosage? Frequency { get; set; }
        [SetToNull]
        public Signa? Signa { get; set; }
    }
}
