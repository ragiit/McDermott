namespace McDermott.Domain.Entities
{
    public class PatientFamilyRelation : BaseAuditableEntity
    {
        public int PatientId { get; set; }
        public int FamilyMemberId { get; set; }
        public int? FamilyId { get; set; }
        public string? Relation { get; set; }

        public virtual Family? Family { get; set; }
        public virtual User? Patient { get; set; }
        public virtual User? FamilyMember { get; set; }
    }
}