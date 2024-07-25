namespace McHealthCare.Domain.Entities
{
    public class GroupMenu : BaseAuditableEntity
    {
        public Guid GroupId { get; set; }
        public Guid MenuId { get; set; }
        public bool? Create { get; set; }
        public bool? Read { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public bool? Import { get; set; }

        public virtual Group? Group { get; set; }

        public virtual Menu? Menu { get; set; }
    }
}