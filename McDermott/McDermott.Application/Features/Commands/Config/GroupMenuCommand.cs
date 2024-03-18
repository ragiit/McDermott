namespace McDermott.Application.Features.Commands.Config
{
    public class GroupMenuCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetGroupMenuQuery(Expression<Func<GroupMenu, bool>>? predicate = null, bool removeCache = false) : IRequest<List<GroupMenuDto>>
        {
            public Expression<Func<GroupMenu, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateGroupMenuRequest(GroupMenuDto GroupMenuDto) : IRequest<GroupMenuDto>
        {
            public GroupMenuDto GroupMenuDto { get; set; } = GroupMenuDto;
        }

        public class CreateListGroupMenuRequest(List<GroupMenuDto> GeneralConsultanCPPTDtos) : IRequest<List<GroupMenuDto>>
        {
            public List<GroupMenuDto> GroupMenuDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateGroupMenuRequest(GroupMenuDto GroupMenuDto) : IRequest<GroupMenuDto>
        {
            public GroupMenuDto GroupMenuDto { get; set; } = GroupMenuDto;
        }

        public class UpdateListGroupMenuRequest(List<GroupMenuDto> GroupMenuDtos) : IRequest<List<GroupMenuDto>>
        {
            public List<GroupMenuDto> GroupMenuDtos { get; set; } = GroupMenuDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteGroupMenuRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}