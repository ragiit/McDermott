namespace McHealthCare.Domain.Entities.Medical{
    public partial class BuildingLocation:BaseAuditableEntity{
        public Guid? BuildingId { get; set; }
        public Guid? LocationId { get; set; }

        public virtual Building? Building{ get; set; }
        public virtual Location? Location{ get; set; }
    }
}