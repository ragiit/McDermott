namespace McDermott.Domain.Entities
{
    public class PatientFamilyRelation : BaseAuditableEntity
    {
        public long PatientId { get; set; }
        public long FamilyMemberId { get; set; }
        public long? FamilyId { get; set; }
        public string? Relation { get; set; }

        [SetToNull]
        public virtual Family? Family { get; set; }
        [SetToNull]
        public virtual User? Patient { get; set; }
        [SetToNull]
        public virtual User? FamilyMember { get; set; }
    }
}