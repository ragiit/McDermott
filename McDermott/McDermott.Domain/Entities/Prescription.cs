namespace McDermott.Domain.Entities
{
    public class Prescription : BaseAuditableEntity
    {
        public long PharmacyId { get; set; }
        public long? DrugFromId { get; set; }
        public long? DrugRouteId { get; set; }
        public long? DrugDosageId { get; set; }
        //public long? MedicamentGroupId { get; set; }
        public long? ProductId { get; set; }
        public long? UomId { get; set; }
        public string? ProductName { get; set; }
        public string? DosageFrequency { get; set; }
        public string? DrugRoutName { get; set; }
        //public string? DrugDosageName { get; set; }
        public long? Stock { get; set; } // product stock
        public long? Dosage { get; set; }
        //public long? PrescribedAmount { get; set; } // jumlah yg diresepkan
        public long? GivenAmount { get; set; } // jumlah yg diberikan
        public long? PriceUnit { get; set; }
        //public long? PreTaxPrescription { get; set; }
        //public long? PreTaxConcoction { get; set; }
        //public long? Total { get; set; }

        [SetToNull]
        public DrugForm? DrugForm { get; set; }
        [SetToNull]
        public Pharmacy? Pharmacy { get; set; }
        [SetToNull]
        public Product? Product { get; set; }
        [SetToNull]
        public Signa? Signa { get; set; }
        [SetToNull]
        public DrugRoute? DrugRoute { get; set; }
        [SetToNull]
        public DrugDosage? DrugDosage { get; set; }
        [SetToNull]
        public MedicamentGroup? MedicamentGroup { get; set; }
    }
}
