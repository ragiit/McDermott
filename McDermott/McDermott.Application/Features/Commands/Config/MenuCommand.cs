namespace McDermott.Application.Features.Commands.Config
{
    public class MenuCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetMenuQuery(Expression<Func<Menu, bool>>? predicate = null) : IRequest<List<MenuDto>>
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