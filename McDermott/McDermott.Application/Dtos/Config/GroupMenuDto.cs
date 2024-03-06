namespace McDermott.Application.Dtos.Config
{
    public class GroupMenuDto : IMapFrom<GroupMenu>
    {
         public long Id { get; set; }
        public long GroupId { get; set; }
        public long MenuId { get; set; }
        public bool Create { get; set; } = false;
        public bool Read { get; set; } = false;
        public bool Update { get; set; } = false;
        public bool Delete { get; set; } = false;
        public bool Import { get; set; } = false;
        public GroupDto? Group { get; set; }
        public MenuDto? Menu { get; set; }
    }
}