namespace McHealthCare.Domain.Entities.ClinicService
{
    public class DetailQueueDisplay : BaseAuditableEntity
    {
        public Guid? KioskQueueId { get; set; }
        public Guid? ServicekId { get; set; }
        public Guid? ServiceId { get; set; }
        public long? NumberQueue { get; set; }
    }
}