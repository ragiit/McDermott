namespace McHealthCare.Domain.Entities.Pharmacies
{
    public class MedicamentGroup : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public bool? IsConcoction { get; set; }
        public string? PhycisianId { get; set; }
        public Guid? UoMId { get; set; }
        public Guid? FormDrugId { get; set; }

        public virtual Doctor? Phycisian { get; set; }

        public virtual Uom? UoM { get; set; }

        public virtual DrugForm? FormDrug { get; set; }
    }
}