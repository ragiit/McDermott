namespace McDermott.Application.Dtos.Patient
{
    public class PatientFamilyRelationDto : IMapFrom<PatientFamilyRelation>
    {
        public long Id { get; set; }

        [Required]
        public long PatientId { get; set; }

        [Required]
        public long FamilyMemberId { get; set; }

        [Required]
        public long? FamilyId { get; set; }

        public string? Relation { get; set; }

        public virtual FamilyDto? Family { get; set; }
        public virtual UserDto? Patient { get; set; }
        public virtual UserDto? FamilyMember { get; set; }
    }

    public class CreateUpdatePatientFamilyRelationDto
    {
        public long Id { get; set; }

        [Required]
        public long PatientId { get; set; }

        [Required]
        public long FamilyMemberId { get; set; }

        [Required]
        public long? FamilyId { get; set; }

        public string? Relation { get; set; }
    }
}