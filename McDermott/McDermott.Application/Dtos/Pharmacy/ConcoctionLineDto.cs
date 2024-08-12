using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Pharmacy
{
    public class ConcoctionLineDto : IMapFrom<ConcoctionLine>
    {
        public long Id { get; set; }
        public long? ConcoctionId { get; set; }
        public long? ProductId { get; set; }
        public string? ProductName { get; set; }
        public List<long>? ActiveComponentId { get; set; } = [];
        public string? ActiveComponentName { get; set; }
        public long? UomId { get; set; }
        public string? UomName { get; set; }
        public long MedicamentDosage { get; set; } = 0;
        public long? MedicamentUnitOfDosage { get; set; }
        public long Dosage { get; set; } = 0;
        public long TotalQty { get; set; } = 0;
        public long? AvaliableQty { get; set; }

        [SetToNull]
        public ProductDto? Product { get; set; }

        [SetToNull]
        public List<ActiveComponentDto>? ActiveComponent { get; set; }

        [SetToNull]
        public ConcoctionDto? Concoction { get; set; }

        [SetToNull]
        public UomDto? Uom { get; set; }
    }
}