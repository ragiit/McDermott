namespace McDermott.Domain.Entities
{
    public partial class BuildingLocation : BaseAuditableEntity
    {
        public long BuildingId { get; set; }
        public long LocationId { get; set; }

        [SetToNull]
        public virtual Building? Building { get; set; }

        [SetToNull]
        public virtual Location? Location { get; set; }
    }
}