namespace McHealthCare.Application.Dtos.Configuration
{
    public class GroupMenuDto : IMapFrom<GroupMenu>
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid MenuId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool VisibleImport { get; set; }
        public bool IsDefaultData { get; set; } = false;

        public GroupDto Group { get; set; } = new();
        public MenuDto Menu { get; set; } = new();
    }

    public class CreateUpdateGroupMenuDto : IMapFrom<GroupMenu>
    {
        public Guid Id { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? MenuId { get; set; }
        public bool IsCreate { get; set; }
        public bool IsRead { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool VisibleImport { get; set; }
    }
}