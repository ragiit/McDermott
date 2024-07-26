namespace McHealthCare.Domain.Entities
{
    public class Stadium : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public Guid? Capacity { get; set; }
        public Guid? BuiltYear { get; set; }
        public Guid? PitchLength { get; set; }
        public Guid? PitchWidth { get; set; }
    }
}