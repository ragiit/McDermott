using Mapster;
using McHealthCare.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Configuration
{
    public class GroupMenuDto : IMapFrom<GroupMenu>
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid MenuId { get; set; }

        public GroupDto Group { get; set; } = new();
        public MenuDto Menu { get; set; } = new();

    }

    public class CreateUpdateGroupMenuDto : IMapFrom<GroupMenu>
    {
        public Guid Id { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? MenuId { get; set; } 
    }
}