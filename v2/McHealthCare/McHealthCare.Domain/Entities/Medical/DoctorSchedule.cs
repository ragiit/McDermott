namespace McHealthCare.Domain.Entities.Medical
{
    public partial class DoctorSchedule : BaseAuditableEntity
    {
        public string? Name { get; set; }
        public Guid? ServiceId { get; set; }
        public List<Guid>? PhysicionIds { get; set; }

        public virtual Service? Service { get; set; }

        public virtual List<ApplicationUser>? Physicion { get; set; }
    }
}