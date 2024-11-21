namespace McDermott.Domain.Entities
{
    public class Signa : BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<Medicament>? Medicaments { get; set; }
    }
}