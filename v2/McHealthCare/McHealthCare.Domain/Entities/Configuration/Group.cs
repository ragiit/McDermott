namespace McHealthCare.Domain.Entities
{
    public class Group : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public virtual List<GroupMenu>? GroupMenus { get; set; }
    }
}