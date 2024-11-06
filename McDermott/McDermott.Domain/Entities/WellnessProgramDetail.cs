namespace McDermott.Domain.Entities
{
    public partial class WellnessProgramDetail : BaseAuditableEntity
    {
        public long WellnessProgramId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Notes { get; set; }
        public string? Slug { get; set; }

        public WellnessProgram? WellnessProgram { get; set; }
    }
}