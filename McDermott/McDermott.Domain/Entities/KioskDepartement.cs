namespace McDermott.Domain.Entities
{
    public class KioskDepartement : BaseAuditableEntity
    {
        public int? ServiceKId { get; set; }
        public int? ServicePId { get; set; }

        public virtual Service? ServiceK { get; set; }
        public virtual Service? ServiceP { get; set; }
    }
}