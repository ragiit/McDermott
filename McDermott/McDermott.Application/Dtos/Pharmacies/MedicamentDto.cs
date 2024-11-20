using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Pharmacies
{
    public class MedicamentDto : IMapFrom<Medicament>
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? FrequencyId { get; set; }
        public long? RouteId { get; set; }
        public long? FormId { get; set; }
        public long? UomId { get; set; } = null;
        public List<long>? ActiveComponentId { get; set; } = [];
        public bool? PregnancyWarning { get; set; }
        public bool? Cronies { get; set; }
        public bool? Pharmacologi { get; set; }
        public bool? Weather { get; set; }
        public bool? Food { get; set; }
        public string? MontlyMax { get; set; }
        public long Dosage { get; set; } = 0;

        [SetToNull]
        public virtual ProductDto? Product { get; set; }

        [SetToNull]
        public virtual DrugDosageDto? Frequency { get; set; }

        [SetToNull]
        public virtual DrugFormDto? Form { get; set; }

        [SetToNull]
        public virtual DrugRouteDto? Route { get; set; }

        [SetToNull]
        public virtual UomDto? Uom { get; set; }

        [SetToNull]
        public virtual List<ActiveComponentDto>? ActiveComponent { get; set; }
    }
}