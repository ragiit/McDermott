namespace McDermott.Domain.Entities
{
    public partial class BuildingLocation : BaseAuditableEntity
    {
        public int BuildingId { get; set; }
        public int LocationId { get; set; }

        public virtual Building? Building { get; set; }
        public virtual Location? Location { get; set; }
    }
}