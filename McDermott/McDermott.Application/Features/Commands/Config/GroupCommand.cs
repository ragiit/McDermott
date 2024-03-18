namespace McDermott.Application.Features.Commands.Config
{
    public class GroupCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetGroupQuery(Expression<Func<Group, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GroupDto>>
        {
            public Expression<Func<Group, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateGroupRequest(GroupDto GroupDto) : IRequest<GroupDto>
        {
            public GroupDto GroupDto { get; set; } = GroupDto;
        }

        public class CreateListGroupRequest(List<GroupDto> GeneralConsultanCPPTDtos) : IRequest<List<GroupDto>>
        {
            public List<GroupDto> GroupDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateGroupRequest(GroupDto GroupDto) : IRequest<GroupDto>
        {
            public GroupDto GroupDto { get; set; } = GroupDto;
        }

        public class UpdateListGroupRequest(List<GroupDto> GroupDtos) : IRequest<List<GroupDto>>
        {
            public List<GroupDto> GroupDtos { get; set; } = GroupDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGroupRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}