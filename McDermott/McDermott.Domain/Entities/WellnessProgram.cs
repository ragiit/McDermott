namespace McDermott.Domain.Entities
{
    public partial class WellnessProgram : BaseAuditableEntity
    {
        public long? AwarenessEduCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Slug { get; set; }
        public string? Slugeess { get; set; }
        public string? Content { get; set; }
        public EnumWellness Status { get; set; } = EnumWellness.Draft;

        public List<WellnessProgramDetail>? WellnessProgramDetails { get; set; }

        [SetToNull]
        public virtual AwarenessEduCategory? AwarenessEduCategory { get; set; }
    }
}