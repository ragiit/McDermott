namespace McDermott.Application.Dtos.Pharmacy
{
    public class PrescriptionDto
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public long? DrugFromId { get; set; }
        public long? SignaId { get; set; }
        public long? DrugRouteId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? ProductId { get; set; }
        public float Stock { get; set; } // product stock
        public float PrescribedAmount { get; set; } // jumlah yg diresepkan
        public float GivenAmount { get; set; } // jumlah yg diberikan
        public float PriceUnit { get; set; }
        public float PreTaxPrescription { get; set; }
        public float PreTaxConcoction { get; set; }
        public float Total { get; set; }

        public DrugFormDto? DrugForm { get; set; }
        public PharmacyDto? Pharmacy { get; set; }
        public ProductDto? Product { get; set; }
        public SignaDto? Signa { get; set; }
        public DrugRouteDto? DrugRoute { get; set; }
        public DrugDosageDto? DrugDosage { get; set; }
    }
}
