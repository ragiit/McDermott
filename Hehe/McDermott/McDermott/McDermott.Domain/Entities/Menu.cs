namespace McDermott.Domain.Entities
{
    public partial class Menu : BaseAuditableEntity
    {
        public long? ParentId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public long? Sequence { get; set; }
        public string? Url { get; set; }
        public bool IsDefaultData { get; set; }

        [SetToNull]
        public virtual List<GroupMenu>? GroupMenus { get; set; }

        public virtual Menu? Parent { get; set; }
    }
}