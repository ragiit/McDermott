namespace McDermott.Application.Features.Commands.Config
{
    public class GroupCommand
    {
        public class GetGroupQuery : IRequest<List<GroupDto>>;

        public class GetGroupByIdQuery : IRequest<GroupDto>
        {
            public long Id { get; set; }

            public GetGroupByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class DeleteListGroupMenuRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListGroupMenuRequest(List<long> id)
            {
                this.Id = id;
            }
        }

        public class DeleteGroupMenuByIdRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteGroupMenuByIdRequest(List<long> Id)
            {
                this.Id = Id;
            }
        }

        public class GetGroupMenuByGroupIdRequest : IRequest<List<GroupMenuDto>>
        {
            public long GroupId { get; set; }

            public GetGroupMenuByGroupIdRequest(long GroupId)
            {
                this.GroupId = GroupId;
            }
        }

        public class UpdateGroupMenuRequest : IRequest<bool>
        {
            public List<long> _ids;

            public List<GroupMenuDto> GroupMenuDto { get; set; }

            public UpdateGroupMenuRequest(List<GroupMenuDto> GroupMenuDto, List<long> ids)
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
            public long Id { get; set; }

            public DeleteGroupRequest(long id)
            {
                Id = id;
            }
        }
    }
}