namespace McDermott.Domain.Entities
{
    public class GroupMenu : BaseAuditableEntity
    {
        public long GroupId { get; set; }
        public long MenuId { get; set; }
        public bool? Create { get; set; }
        public bool? Read { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public bool? Import { get; set; }
        [SetToNull]
        public virtual Group? Group { get; set; }
        [SetToNull]
        public virtual Menu? Menu { get; set; }
    }
}