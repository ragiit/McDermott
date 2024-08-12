namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DoctorSchedule : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public Guid? ServiceId { get; set; }
        public List<Guid>? PhysicionIds { get; set; }

        [SetToNull]
        public virtual Service? Service { get; set; }
        [SetToNull]
        public virtual List<ApplicationUser>? Physicion { get; set; }
    }
}