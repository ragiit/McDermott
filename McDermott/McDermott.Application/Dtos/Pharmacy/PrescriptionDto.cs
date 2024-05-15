namespace McDermott.Application.Dtos.Pharmacy
{
    public class PrescriptionDto
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public long? DrugFromId { get; set; }
        public long? DrugRouteId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? MedicamentGroupId { get; set; }
        public long? ProductId { get; set; }
        public long? UomId { get; set; }
        public string? ProductName { get; set; }
        public string? DosageFrequency { get; set; }
        public string? DrugRoutName { get; set; }
        public string? DrugDosageName { get; set; }
        public long? Stock { get; set; } // product stock
        public long? Dosage { get; set; }
        public long? PrescribedAmount { get; set; } // jumlah yg diresepkan
        public long? GivenAmount { get; set; } // jumlah yg diberikan
        public long? PriceUnit { get; set; }
        public long? PreTaxPrescription { get; set; }
        public long? PreTaxConcoction { get; set; }
        public long? Total { get; set; }

        public DrugFormDto? DrugForm { get; set; }
        public PharmacyDto? Pharmacy { get; set; }
        public ProductDto? Product { get; set; }
        public SignaDto? Signa { get; set; }
        public DrugRouteDto? DrugRoute { get; set; }
        public DrugDosageDto? DrugDosage { get; set; }
        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}