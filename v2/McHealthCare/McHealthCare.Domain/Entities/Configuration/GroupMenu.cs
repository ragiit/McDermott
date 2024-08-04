namespace McHealthCare.Domain.Entities
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

        public Group? Group { get; set; }
        public Menu? Menu { get; set; }
    }
}