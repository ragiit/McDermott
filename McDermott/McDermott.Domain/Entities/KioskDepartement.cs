namespace McDermott.Domain.Entities
{
    public class KioskDepartement : BaseAuditableEntity
    {
        public long? ServiceKId { get; set; }
        public long? ServicePId { get; set; }

        public virtual Service? ServiceK { get; set; }
        public virtual Service? ServiceP { get; set; }
    }
}