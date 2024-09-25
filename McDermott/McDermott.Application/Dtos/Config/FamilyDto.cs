namespace McDermott.Application.Dtos.Config
{
    public partial class FamilyDto : IMapFrom<Family>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public long? InverseRelationId { get; set; }

        [NotMapped]
        public string? InverseRelationString => InverseRelation is null ? "" : $"{InverseRelation.Name}-{Name}";

        public FamilyDto? InverseRelation { get; set; }
    }

    public partial class CreateUpdateFamilyDto
    {
        public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public long? InverseRelationId { get; set; }
    }
}