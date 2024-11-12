namespace McDermott.Domain.Entities
{
    public class Stadium : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string City { get; set; }
        public long? Capacity { get; set; }
        public long? BuiltYear { get; set; }
        public long? PitchLength { get; set; }
        public long? PitchWidth { get; set; }
    }
}