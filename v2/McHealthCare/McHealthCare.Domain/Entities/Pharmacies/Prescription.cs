namespace McHealthCare.Domain.Entities
{
    public class Prescription : BaseAuditableEntity
    {
        public Guid PharmacyId { get; set; }
        public Guid? DrugFromId { get; set; }
        public Guid? DrugRouteId { get; set; }
        public Guid? DrugDosageId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? UomId { get; set; }
        public List<Guid>? ActiveComponentId { get; set; } = [];
        public string? DosageFrequency { get; set; }
        public long? Stock { get; set; } // product stock
        public long? Dosage { get; set; }
        public long? GivenAmount { get; set; } // jumlah yg diberikan
        public long? PriceUnit { get; set; }

        
        public DrugForm? DrugForm { get; set; }

        
        public Pharmacy? Pharmacy { get; set; }

        
        public Product? Product { get; set; }

        
        public Signa? Signa { get; set; }

        
        public DrugRoute? DrugRoute { get; set; }

        
        public DrugDosage? DrugDosage { get; set; }

        
        public MedicamentGroup? MedicamentGroup { get; set; }

        
        public List<ActiveComponent>? ActiveComponent { get; set; }

        
        public List<StockOutPrescription>? StockOutPrescription { get;set; }
    }
}