namespace McDermott.Application.Features.Commands.Config
{
    public class MenuCommand
    {
        public class GetMenuQuery : IRequest<List<MenuDto>>;

        public class GetMenuByIdQuery : IRequest<MenuDto>
        {
             public long Id { get; set; }

            public GetMenuByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateMenuRequest : IRequest<MenuDto>
        {
            public MenuDto MenuDto { get; set; }

            public CreateMenuRequest(MenuDto MenuDto)
            {
                this.MenuDto = MenuDto;
            }
        }

        public class UpdateMenuRequest : IRequest<bool>
        {
            public MenuDto MenuDto { get; set; }

            public UpdateMenuRequest(MenuDto MenuDto)
            {
                this.MenuDto = MenuDto;
            }
        }

        public class DeleteMenuRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteMenuRequest(long id)
            {
                Id = id;
            }
        }
    }
}