namespace McDermott.Domain.Entities
{
    public class KioskDepartement : BaseAuditableEntity
    {
        public long? ServiceKId { get; set; }
        public long? ServicePId { get; set; }

        [SetToNull]
        public virtual Service? ServiceK { get; set; }
        [SetToNull]
        public virtual Service? ServiceP { get; set; }
    }
}