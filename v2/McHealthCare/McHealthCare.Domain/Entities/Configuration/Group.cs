namespace McHealthCare.Domain.Entities
{
    public class Group : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool IsDefaultData { get; set; } = false;

        public virtual List<GroupMenu>? GroupMenus { get; set; }
    }
}