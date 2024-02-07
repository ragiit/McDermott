using McDermott.Application.Dtos.Config;
using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Config.UserCommand;

namespace McDermott.Application.Features.Queries.Config
{
    public class UserQueryHandler
    {
        #region Get
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
                        .AsNoTracking()
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

        internal class GetUserByEmailPasswordQueryHandler : IRequestHandler<GetUserByEmailPasswordQuery, UserDto>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserByEmailPasswordQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<UserDto> Handle(GetUserByEmailPasswordQuery request, CancellationToken cancellationToken)
            {
                var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Email.Equals(request.UserDto.Email) && x.Password.Equals(request.UserDto.Password));

                return result.Adapt<List<UserDto>>().FirstOrDefault()!;
            }
        }

        internal class GetUserForKioskQueryhandler : IRequestHandler<GetDataUserForKioskQuery, List<UserDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetUserForKioskQueryhandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<UserDto>> Handle(GetDataUserForKioskQuery request, CancellationToken cancellationToken)
            {
               List<UserDto>  data = new List<UserDto>();
                if (request.Types == "Legacy")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Legacy!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }else if(request.Types == "Oracle")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.Oracle!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }else if(request.Types == "SAP")
                {
                    var result = await _unitOfWork.Repository<User>().GetAllAsync(x => x.SAP!.Equals(request.Number));
                    data = result.Adapt<List<UserDto>>().ToList();
                }
                return data;
            }
        }
        #endregion

        #region Create
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
        #endregion

        #region Update
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

                var user = request.UserDto.Adapt<User>();

                await _unitOfWork.Repository<User>().UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        #endregion

        #region Delete
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

        internal class DeleteListCountryHandler : IRequestHandler<DeleteListCountryRequest, bool>
        {
            private readonly IUnitOfWork _unitOfWork;

            public DeleteListCountryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeleteListCountryRequest request, CancellationToken cancellationToken)
            {
                await _unitOfWork.Repository<Country>().DeleteAsync(request.Id);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return true;
            }
        }
        #endregion 
    }
}