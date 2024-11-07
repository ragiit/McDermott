namespace McDermott.Application.Features.Commands.Config
{
    public class GroupMenuCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleGroupMenuQuery : IRequest<GroupMenuDto>
        {
            public List<Expression<Func<GroupMenu, object>>> Includes { get; set; }
            public Expression<Func<GroupMenu, bool>> Predicate { get; set; }
            public Expression<Func<GroupMenu, GroupMenu>> Select { get; set; }

            public List<(Expression<Func<GroupMenu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateGroupMenu(Expression<Func<GroupMenu, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GroupMenu, bool>> Predicate { get; } = predicate!;
        }

        public class GetGroupMenuQuery : IRequest<(List<GroupMenuDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<GroupMenu, object>>> Includes { get; set; }
            public Expression<Func<GroupMenu, bool>> Predicate { get; set; }
            public Expression<Func<GroupMenu, GroupMenu>> Select { get; set; }

            public List<(Expression<Func<GroupMenu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateGroupMenuQuery(List<GroupMenuDto> GroupMenusToValidate) : IRequest<List<GroupMenuDto>>
        {
            public List<GroupMenuDto> GroupMenusToValidate { get; } = GroupMenusToValidate;
        }

        public class ValidateGroupMenuQuery(Expression<Func<GroupMenu, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<GroupMenu, bool>> Predicate { get; } = predicate!;
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