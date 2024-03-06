namespace McDermott.Application.Dtos.Config
{
    public partial class FamilyDto : IMapFrom<Family>
    {
         public long Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? ParentRelation { get; set; }
        public string? ChildRelation { get; set; }

        [StringLength(100)]
        public string? Relation { get; set; }
    }
}