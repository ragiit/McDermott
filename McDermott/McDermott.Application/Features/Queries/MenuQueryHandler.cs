using static McDermott.Application.Features.Commands.MenuCommand;

namespace McDermott.Application.Features.Queries
{
    public class MenuQueryHandler
    {
        internal class GetAllMenuQueryHandler : IRequestHandler<GetMenuQuery, List<MenuDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllMenuQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<MenuDto>> Handle(GetMenuQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<Menu>().Entities
                        .Select(Menu => Menu.Adapt<MenuDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetMenuByIdQueryHandler : IRequestHandler<GetMenuByIdQuery, MenuDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetMenuByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MenuDto> Handle(GetMenuByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Menu>().GetByIdAsync(request.Id);

                return result.Adapt<MenuDto>();
            }
        }

        internal class CreateMenuHandler : IRequestHandler<CreateMenuRequest, MenuDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateMenuHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<MenuDto> Handle(CreateMenuRequest request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<Menu>().AddAsync(request.MenuDto.Adapt<Menu>());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return result.Adapt<MenuDto>();
            }
        }

        internal class UpdateMenuHandler : IRequestHandler<UpdateMenuRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateMenuHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateMenuRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Menu>().UpdateAsync(request.MenuDto.Adapt<Menu>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteMenuHandler : IRequestHandler<DeleteMenuRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteMenuHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteMenuRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Menu>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}