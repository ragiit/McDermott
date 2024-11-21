namespace McDermott.Application.Features.Commands.Config
{
    public class GroupCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleGroupQuery : IRequest<GroupDto>
        {
            public List<Expression<Func<Group, object>>> Includes { get; set; }
            public Expression<Func<Group, bool>> Predicate { get; set; }
            public Expression<Func<Group, Group>> Select { get; set; }

            public List<(Expression<Func<Group, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetGroupQuery : IRequest<(List<GroupDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Group, object>>> Includes { get; set; }
            public Expression<Func<Group, bool>> Predicate { get; set; }
            public Expression<Func<Group, Group>> Select { get; set; }

            public List<(Expression<Func<Group, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateGroupQuery(List<GroupDto> GroupsToValidate) : IRequest<List<GroupDto>>
        {
            public List<GroupDto> GroupsToValidate { get; } = GroupsToValidate;
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