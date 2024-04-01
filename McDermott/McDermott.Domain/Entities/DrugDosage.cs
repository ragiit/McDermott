namespace McDermott.Domain.Entities
{
    public class DrugDosage : BaseAuditableEntity
    {
        public long? DrugRouteId { get; set; }
        public string Frequency { get; set; } = string.Empty;

        [SetToNull]
        public virtual DrugRoute? DrugRoute { get; set; }
    }
}
