namespace McDermott.Domain.Entities
{
    public class GroupMenu : BaseAuditableEntity
    {
        public int GroupId { get; set; }
        public int MenuId { get; set; }
        public bool? Create { get; set; }
        public bool? Read { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public bool? Import { get; set; }
        public virtual Group? Group { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}