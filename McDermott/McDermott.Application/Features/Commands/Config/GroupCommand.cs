using McDermott.Application.Interfaces.Repositories;

namespace McDermott.Application.Features.Commands.Config
{
    public class GroupCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetGroupQuery(Expression<Func<Group, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<GroupDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Group, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
        }

        public class ValidateGroupQuery(Expression<Func<Group, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Group, bool>> Predicate { get; } = predicate!;
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