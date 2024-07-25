namespace McHealthCare.Domain.Entities
{
    public class PharmacyLog : BaseAuditableEntity
    {
        public Guid? PharmacyId { get; set; }
        public Guid? UserById { get; set; }
        public EnumStatusPharmacy? status { get; set; }

        public Pharmacy? Pharmacy { get; set; }
        //   public User? UserBy { get; set; }
    }
}