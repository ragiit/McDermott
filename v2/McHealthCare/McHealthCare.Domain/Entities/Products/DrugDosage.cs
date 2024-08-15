namespace McHealthCare.Domain.Entities.Products
{
    public class DrugDosage : BaseAuditableEntity
    {
        public Guid? DrugRouteId { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public float TotalQtyPerDay { get; set; }
        public float Days { get; set; }

        public virtual DrugRoute? DrugRoute { get; set; }
        public List<Medicament>? Medicaments { get; set; }
    }
}