namespace McDermott.Domain.Entities
{
    public class PharmacyLog : BaseAuditableEntity
    {
        public long? PharmacyId { get; set; }
        public long? UserById { get; set; }
        public EnumStatusPharmacy? status { get; set; }

        public Pharmacy? Pharmacy { get; set; }
        public User? UserBy { get; set; }
    }
}