namespace McDermott.Application.Features.Commands.Config
{
    public class MenuCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetMenuQuery(Expression<Func<Menu, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<MenuDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Menu, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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