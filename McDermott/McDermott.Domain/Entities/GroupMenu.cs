namespace McDermott.Domain.Entities
{
    public class GroupMenu : BaseAuditableEntity
    {
        public bool IsDefaultData { get; set; }
        public long GroupId { get; set; }
        public long MenuId { get; set; }
        public bool? IsCreate { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsUpdate { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsImport { get; set; }

        [SetToNull]
        public virtual Group? Group { get; set; }

        [SetToNull]
        public virtual Menu? Menu { get; set; }
    }
}