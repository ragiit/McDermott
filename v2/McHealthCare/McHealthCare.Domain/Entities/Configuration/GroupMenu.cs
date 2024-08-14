namespace McHealthCare.Domain.Entities.Configuration
{
    public class GroupMenu : BaseAuditableEntity
    {
        public Guid GroupId { get; set; }
        public Guid MenuId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsImport { get; set; }
        public bool IsDefaultData { get; set; } = false;
        public Group? Group { get; set; }
        public Menu? Menu { get; set; }
    }
}