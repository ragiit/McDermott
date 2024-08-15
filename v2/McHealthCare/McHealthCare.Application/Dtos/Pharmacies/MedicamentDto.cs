namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class MedicamentDto : IMapFrom<Medicament>
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? FrequencyId { get; set; }
        public Guid? RouteId { get; set; }
        public Guid? FormId { get; set; }
        public Guid? UomId { get; set; } = null;
        public List<Guid>? ActiveComponentId { get; set; } = [];
        public bool? PregnancyWarning { get; set; }
        public bool? Cronies { get; set; }
        public bool? Pharmacologi { get; set; }
        public bool? Weather { get; set; }
        public bool? Food { get; set; }
        public string? MontlyMax { get; set; }
        public long Dosage { get; set; } = 0;

        public virtual ProductDto? Product { get; set; }

        public virtual DrugDosageDto? Frequency { get; set; }

        public virtual DrugFormDto? Form { get; set; }

        public virtual DrugRouteDto? Route { get; set; }

        public virtual UomDto? Uom { get; set; }
    }
}