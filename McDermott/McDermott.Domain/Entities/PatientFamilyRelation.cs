namespace McDermott.Domain.Entities
{
    public class PatientFamilyRelation : BaseAuditableEntity
    {
        public int PatientId { get; set; }
        public int FamilyMemberId { get; set; }
        public int? FamilyId { get; set; }
        public string? Relation { get; set; }

        [SetToNull]
        public virtual Family? Family { get; set; }
        [SetToNull]
        public virtual User? Patient { get; set; }
        [SetToNull]
        public virtual User? FamilyMember { get; set; }
    }
}