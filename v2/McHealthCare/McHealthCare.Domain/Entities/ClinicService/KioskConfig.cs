namespace McHealthCare.Domain.Entities
{
    public class KioskConfig : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public List<long>? ServiceIds { get; set; }

        //public virtual Service? Service { get; set; }
    }
}