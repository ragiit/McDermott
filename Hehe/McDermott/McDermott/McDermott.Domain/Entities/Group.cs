namespace McDermott.Domain.Entities
{
    public partial class Group : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool IsDefaultData { get; set; }

        [SetToNull]
        public virtual List<User>? Users { get; set; }

        [SetToNull]
        public virtual List<GroupMenu>? GroupMenus { get; set; }
    }
}