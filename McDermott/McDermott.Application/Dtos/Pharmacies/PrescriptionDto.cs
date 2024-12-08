using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Pharmacies
{
    public class PrescriptionDto : IMapFrom<Prescription>
    {
        public long Id { get; set; }
        public long PharmacyId { get; set; }
        public long? DrugFromId { get; set; }
        public long? DrugRouteId { get; set; }
        public long? DrugDosageId { get; set; }
        public long? ProductId { get; set; }
        public long? UomId { get; set; }
        public string? ProductName { get; set; }
        public string? DosageFrequency { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public string? ActiveComponentNames { get; set; }
        public string? DrugRoutName { get; set; }
        public string? DrugDosageName { get; set; }
        public long? Stock { get; set; } // product stock
        public long? Dosage { get; set; }
        public long GivenAmount { get; set; } = 0; // jumlah yg diberikan
        public long? PriceUnit { get; set; }

        [NotMapped]
        public string DosageAndFrequency
        {
            get
            {
                return $"{Dosage.ToString()}/{DrugDosage.Frequency}";
            }

            set { }
        }

        [SetToNull]
        public DrugFormDto? DrugForm { get; set; }

        [SetToNull]
        public PharmacyDto? Pharmacy { get; set; }

        [SetToNull]
        public ProductDto? Product { get; set; }

        [SetToNull]
        public SignaDto? Signa { get; set; }

        [SetToNull]
        public DrugRouteDto? DrugRoute { get; set; }

        [SetToNull]
        public DrugDosageDto? DrugDosage { get; set; }

        [SetToNull]
        public MedicamentGroupDto? MedicamentGroup { get; set; }
    }
}