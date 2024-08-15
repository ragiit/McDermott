namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class ConcoctionLineDto : IMapFrom<ConcoctionLine>
    {
        public Guid Id { get; set; }
        public Guid? ConcoctionId { get; set; }
        public Guid? ProductId { get; set; }
        public string? ProductName { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public string? ActiveComponentName { get; set; }
        public Guid? UomId { get; set; }
        public string? UomName { get; set; }
        public long MedicamentDosage { get; set; } = 0;
        public long? MedicamentUnitOfDosage { get; set; }
        public long Dosage { get; set; } = 0;
        public long TotalQty { get; set; } = 0;
        public long? AvaliableQty { get; set; }

        public ProductDto? Product { get; set; }

        public List<ActiveComponentDto>? ActiveComponent { get; set; }

        public ConcoctionDto? Concoction { get; set; }

        public UomDto? Uom { get; set; }
    }
}