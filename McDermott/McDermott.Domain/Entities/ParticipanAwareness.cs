namespace McDermott.Domain.Entities
{
    public class ParticipanAwareness : BaseAuditableEntity
    {
        public long? PatientId { get; set; }
        public long? AwarenessId { get; set; }

        [SetToNull]
        public virtual User? Patient { get; set; }
    }
}