namespace McDermott.Domain.Entities
{
    public class ParticipanEdu : BaseAuditableEntity
    {
        public long? PatientId { get; set; }
        public long? EducationProgramId { get; set; }

        [SetToNull]
        public virtual User? Patient { get; set; }

        [SetToNull]
        public virtual EducationProgram? EducationProgram { get; set; }
    }
}