namespace McDermott.Application.Dtos.Config
{
    public class GroupMenuDto : IMapFrom<GroupMenu>
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long MenuId { get; set; }
        public bool Create { get; set; } = true;
        public bool Read { get; set; } = true;
        public bool Update { get; set; } = true;
        public bool Delete { get; set; } = true;
        public bool Import { get; set; } = true;
        public GroupDto? Group { get; set; }
        public MenuDto? Menu { get; set; }
    }
}