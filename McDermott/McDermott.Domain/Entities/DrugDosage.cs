namespace McDermott.Domain.Entities
{
    public class DrugDosage : BaseAuditableEntity
    {
        public long? DrugRouteId { get; set; }
        public string Frequency { get; set; } = string.Empty;

        public virtual DrugRoute? DrugRoute { get; set; }
    }
}
