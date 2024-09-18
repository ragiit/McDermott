namespace McDermott.Application.Dtos.Config
{
    public class GroupMenuDto : IMapFrom<GroupMenu>
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long? MenuId { get; set; }
        public bool IsCreate { get; set; } = true;
        public bool IsRead { get; set; } = true;
        public bool IsUpdate { get; set; } = true;
        public bool IsDelete { get; set; } = true;
        public bool IsImport { get; set; } = true;
        public bool IsDefaultData { get; set; }
        public GroupDto? Group { get; set; }
        public MenuDto? Menu { get; set; }
    }

    public class CreateUpdateGroupMenuDto
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long? MenuId { get; set; }
        public bool IsCreate { get; set; } = true;
        public bool IsRead { get; set; } = true;
        public bool IsUpdate { get; set; } = true;
        public bool IsDelete { get; set; } = true;
        public bool IsImport { get; set; } = true;
        public bool IsDefaultData { get; set; }
    }
}