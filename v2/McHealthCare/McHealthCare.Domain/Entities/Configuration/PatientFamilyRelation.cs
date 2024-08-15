namespace McDermott.Domain.Entities
{
    public class PatientFamilyRelation : BaseAuditableEntity
    {
        public string PatientId { get; set; } = string.Empty;
        public string FamilyMemberId { get; set; } = string.Empty;
        public Guid? FamilyId { get; set; }
        public string? Relation { get; set; }

        [SetToNull]
        public virtual Family? Family { get; set; }

        [SetToNull]
        public virtual Patient? Patient { get; set; }

        [SetToNull]
        public virtual Patient? FamilyMember { get; set; }
    }
}