namespace McHealthCare.Domain.Entities
{
    public class PatientFamilyRelation : BaseAuditableEntity
    {
        public Guid PatientId { get; set; }
        public Guid FamilyMemberId { get; set; }
        public Guid? FamilyId { get; set; }
        public string? Relation { get; set; }

        public virtual Family? Family { get; set; }

        // public virtual User? Patient { get; set; }

        //public virtual User? FamilyMember { get; set; }
    }
}