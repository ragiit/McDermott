

using McHealthCare.Domain.Entities.Inventory;

namespace McHealthCare.Domain.Entities.Products
{
    public class Medicament : BaseAuditableEntity
    {
        public Guid? ProductId { get; set; }
        public Guid? FrequencyId { get; set; }
        public Guid? RouteId { get; set; }
        public Guid? FormId { get; set; }
        public Guid? UomId { get; set; }
        public List<Guid>? ActiveComponentId { get; set; }
        public bool? PregnancyWarning { get; set; }
        public bool? Pharmacologi { get; set; }
        public bool? Weather { get; set; }
        public bool? Food { get; set; }
        public bool? Cronies { get; set; }
        public string? MontlyMax { get; set; }
        public string? Dosage { get; set; }


        public Uom? Uom { get; set; }
        public DrugForm? Form { get; set; }
        public DrugDosage? Frequency { get; set; }
        public Product? Product { get; set; }
        public DrugRoute? Route { get; set; }
        public List<ActiveComponent>? ActiveComponent { get; set; }
    }
}
