using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class GroupCommand
    {
        public class GetGroupQuery : IRequest<List<GroupDto>>;

        public class GetGroupByIdQuery : IRequest<GroupDto>
        {
            public int Id { get; set; }

            public GetGroupByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class DeleteListGroupMenuRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListGroupMenuRequest(List<int> id)
            {
                this.Id = id;
            }
        }

        public class DeleteGroupMenuByIdRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteGroupMenuByIdRequest(List<int> Id)
            {
                this.Id = Id;
            }
        }

        public class GetGroupMenuByGroupIdRequest : IRequest<List<GroupMenuDto>>
        {
            public int GroupId { get; set; }

            public GetGroupMenuByGroupIdRequest(int GroupId)
            {
                this.GroupId = GroupId;
            }
        }

        public class UpdateGroupMenuRequest : IRequest<bool>
        {
            public List<int> _ids;

            public List<GroupMenuDto> GroupMenuDto { get; set; }

            public UpdateGroupMenuRequest(List<GroupMenuDto> GroupMenuDto, List<int> ids)
            {
                this.GroupMenuDto = GroupMenuDto;
                this._ids = ids;
            }
        }

        public class CreateGroupMenuRequest : IRequest<bool>
        {
            public List<GroupMenuDto> GroupMenuDto { get; set; }

            public CreateGroupMenuRequest(List<GroupMenuDto> GroupMenuDto)
            {
                this.GroupMenuDto = GroupMenuDto;
            }
        }

        public class GetGroupByNameQuery : IRequest<GroupDto>
        {
            public string Name { get; set; }

            public GetGroupByNameQuery(string Name)
            {
                this.Name = Name;
            }
        }

        public class CreateGroupRequest : IRequest<GroupDto>
        {
            public GroupDto GroupDto { get; set; }

            public CreateGroupRequest(GroupDto GroupDto)
            {
                this.GroupDto = GroupDto;
            }
        }

        public class UpdateGroupRequest : IRequest<bool>
        {
            public GroupDto GroupDto { get; set; }

            public UpdateGroupRequest(GroupDto GroupDto)
            {
                this.GroupDto = GroupDto;
            }
        }

        public class DeleteGroupRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteGroupRequest(int id)
            {
                Id = id;
            }
        }
    }
}