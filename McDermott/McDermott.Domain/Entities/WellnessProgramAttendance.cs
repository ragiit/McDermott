namespace McDermott.Domain.Entities
{
    public partial class WellnessProgramAttendance : BaseAuditableEntity
    {
        public long WellnessProgramId { get; set; }
        public long WellnessProgramDetailId { get; set; }

        public long PatientId { get; set; }
        public DateTime Date { get; set; }

        public User? Patient { get; set; }

        public WellnessProgram? WellnessProgram { get; set; }
        public WellnessProgramDetail? WellnessProgramDetail { get; set; }
    }
}