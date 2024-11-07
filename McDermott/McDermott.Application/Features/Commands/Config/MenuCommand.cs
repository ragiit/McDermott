namespace McDermott.Application.Features.Commands.Config
{
    public class MenuCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetSingleMenuQuery : IRequest<MenuDto>
        {
            public List<Expression<Func<Menu, object>>> Includes { get; set; }
            public Expression<Func<Menu, bool>> Predicate { get; set; }
            public Expression<Func<Menu, Menu>> Select { get; set; }

            public List<(Expression<Func<Menu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetMenuQuery : IRequest<(List<MenuDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Menu, object>>> Includes { get; set; }
            public Expression<Func<Menu, bool>> Predicate { get; set; }
            public Expression<Func<Menu, Menu>> Select { get; set; }

            public List<(Expression<Func<Menu, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class BulkValidateMenuQuery(List<MenuDto> MenusToValidate) : IRequest<List<MenuDto>>
        {
            public List<MenuDto> MenusToValidate { get; } = MenusToValidate;
        }

        public class ValidateMenuQuery(Expression<Func<Menu, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Menu, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateMenuRequest(MenuDto MenuDto) : IRequest<MenuDto>
        {
            public MenuDto MenuDto { get; set; } = MenuDto;
        }

        public class CreateListMenuRequest(List<MenuDto> GeneralConsultanCPPTDtos) : IRequest<List<MenuDto>>
        {
            public List<MenuDto> MenuDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateMenuRequest(MenuDto MenuDto) : IRequest<MenuDto>
        {
            public MenuDto MenuDto { get; set; } = MenuDto;
        }

        public class UpdateListMenuRequest(List<MenuDto> MenuDtos) : IRequest<List<MenuDto>>
        {
            public List<MenuDto> MenuDtos { get; set; } = MenuDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteMenuRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}