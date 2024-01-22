using static McDermott.Application.Features.Commands.UserCommand;

namespace McDermott.Application.Features.Queries
{
    public class UserQueryHandler
    {
        internal class GetAllUserQueryHandler : IRequestHandler<GetUserQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetAllUserQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
            {
                return await _unitOfWork.Repository<User>().Entities
                    .Include(x => x.Group)
                        .Select(User => User.Adapt<UserDto>())
                       .ToListAsync(cancellationToken);
            }
        }

        internal class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<User>().GetByIdAsync(request.Id);

                return result.Adapt<UserDto>();
            }
        }

        internal class CreateUserHandler : IRequestHandler<CreateUserRequest, UserDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public CreateUserHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserDto> Handle(CreateUserRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    if (!request.UserDto.TypeId.Contains("VISA"))
                        request.UserDto.ExpiredId = null;

                    // nanti dihapus
                    request.UserDto.UserName = request.UserDto.Email;

                    var result = await _unitOfWork.Repository<User>().AddAsync(request.UserDto.Adapt<User>());

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return result.Adapt<UserDto>();
                }
                catch (Exception e)
                {
                    Console.Write("😋" + e.Message);
                    throw;
                }
            }
        }

        internal class UpdateUserHandler : IRequestHandler<UpdateUserRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public UpdateUserHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                // nanti dihapus
                request.UserDto.UserName = request.UserDto.Email;

                await _unitOfWork.Repository<User>().UpdateAsync(request.UserDto.Adapt<User>());
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteUserHandler : IRequestHandler<DeleteUserRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteUserHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<User>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }

        internal class DeleteListUserHandler : IRequestHandler<DeleteListUserRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListUserHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListUserRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<User>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
    }
}