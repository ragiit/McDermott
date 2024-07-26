namespace McHealthCare.Domain.Entities
{
    public partial class Menu : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Icon { get; set; }
        public string? ParentMenu { get; set; }
        public Guid? Sequence { get; set; }
        public string? Html { get; set; }
        public string? Url { get; set; }

        
        public virtual List<GroupMenu>? GroupMenus { get; set; }
    }
}