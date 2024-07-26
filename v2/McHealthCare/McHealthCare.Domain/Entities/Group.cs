namespace McHealthCare.Domain.Entities
{
    public partial class Group : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        
        // public virtual List<User>? Users { get; set; }

        
        public virtual List<GroupMenu>? GroupMenus { get; set; }
    }
}