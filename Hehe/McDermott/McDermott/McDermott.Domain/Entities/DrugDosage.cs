namespace McDermott.Domain.Entities
{
    public class DrugDosage : BaseAuditableEntity
    {
        public long? DrugRouteId { get; set; }
        public string Frequency { get; set; } = string.Empty;
        public float TotalQtyPerDay { get; set; }
        public float Days { get; set; }

        [SetToNull]
        public virtual DrugRoute? DrugRoute { get; set; }
        public  List<Medicament>? Medicaments { get; set; }
    }
}
