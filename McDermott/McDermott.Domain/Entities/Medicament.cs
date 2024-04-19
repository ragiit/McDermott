

namespace McDermott.Domain.Entities
{
    public class Medicament : BaseAuditableEntity
    {
        public long? ProductId { get; set; }
        public long? SignaId { get; set; }
        public long? RouteId { get; set; }
        public long? FormId { get; set; }
        public long? UomId { get; set; }
        public List<long>? ActiveComponentId { get; set; }
        public bool? PregnancyWarning { get; set; }
        public bool? Pharmacologi { get; set; }
        public bool? Weather { get; set; }
        public bool? Food { get; set; }
        public bool? Cronies { get; set; }
        public string? MontlyMax { get; set; }
        public string? Dosage { get; set; }


        public Uom? Uom { get; set; }
        public DrugForm? Form { get; set; }
        public Signa? Signa { get; set; }
        public Product? Product { get; set; }
        public DrugRoute? Route { get; set; }
        public List<ActiveComponent>? ActiveComponent { get; set; }
    }
}
