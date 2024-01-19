using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos
{
    public class GroupMenuDto : IMapFrom<GroupMenu>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int MenuId { get; set; }
        public bool? Create { get; set; }
        public bool? Read { get; set; }
        public bool? Update { get; set; }
        public bool? Delete { get; set; }
        public bool? Import { get; set; }
        public GroupDto? Group { get; set; }
        public MenuDto? Menu { get; set; }
    }
}