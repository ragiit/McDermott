namespace McHealthCare.Domain.Entities.Pharmacies
{
    public class PharmacyLog : BaseAuditableEntity
    {
        public Guid? PharmacyId { get; set; }
        public string? UserById { get; set; }
        public EnumStatusPharmacy? status { get; set; }

        public Pharmacy? Pharmacy { get; set; }
        public ApplicationUser? UserBy { get; set; }
    }
}
