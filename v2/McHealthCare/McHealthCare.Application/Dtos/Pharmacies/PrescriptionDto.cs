namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class PrescriptionDto : IMapFrom<Prescription>
    {
        public Guid Id { get; set; }
        public Guid PharmacyId { get; set; }
        public Guid? DrugFromId { get; set; }
        public Guid? DrugRouteId { get; set; }
        public Guid? DrugDosageId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? UomId { get; set; }
        public string? ProductName { get; set; }
        public string? DosageFrequency { get; set; }
        public List<Guid>? ActiveComponentId { get; set; } = [];
        public string? ActiveComponentNames { get; set; }
        public string? DrugRoutName { get; set; }
        public string? DrugDosageName { get; set; }
        public long? Stock { get; set; } // product stock
        public long? Dosage { get; set; }
        public long GivenAmount { get; set; } = 0; // jumlah yg diberikan
        public long? PriceUnit { get; set; }

        public DrugFormDto? DrugForm { get; set; }
        public PharmacyDto? Pharmacy { get; set; }
        public ProductDto? Product { get; set; }
        public SignaDto? Signa { get; set; }
        public DrugRouteDto? DrugRoute { get; set; }
        public DrugDosageDto? DrugDosage { get; set; }
        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}