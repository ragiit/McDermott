namespace McDermott.Application.Dtos.Patient
{
    public class PatientFamilyRelationDto : IMapFrom<PatientFamilyRelation>
    {
         public long Id { get; set; }
        public long PatientId { get; set; }
        public long? FamilyMemberId { get; set; }
        public long? FamilyId { get; set; }
        public string? Relation { get; set; }

        public virtual FamilyDto? Family { get; set; }
        public virtual UserDto? Patient { get; set; }
        public virtual UserDto? FamilyMember { get; set; }
    }
}