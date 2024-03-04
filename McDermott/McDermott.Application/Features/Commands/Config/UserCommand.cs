namespace McDermott.Application.Features.Commands.Config
{
    public class UserCommand
    {
        #region Get
        public class GetUserQuery(Expression<Func<User, bool>>? predicate = null) : IRequest<List<UserDto>>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
        }

        public class GetDataUserForKioskQuery : IRequest<List<UserDto>>
        {
            public UserDto userDto { get; set; }
            public string Types { get; set; }
            public string Number { get; set; }

            public GetDataUserForKioskQuery(string Types, string Number)
            {
                this.Types = Types;
                this.Number = Number;
            }
        }

        #endregion Get

        #region Create

        public class CreateUserRequest(UserDto UserDto) : IRequest<UserDto>
        {
            public UserDto UserDto { get; set; } = UserDto;
        }

        #endregion Create

        #region Update

        public class UpdateUserRequest(UserDto UserDto) : IRequest<UserDto>
        {
            public UserDto UserDto { get; set; } = UserDto;
        }

        public class UpdateListUserRequest(List<UserDto> UserDto) : IRequest<List<UserDto>>
        {
            public List<UserDto> UserDtos { get; set; } = UserDto;
        }

        #endregion Update

        #region Delete


        public class DeleteUserRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}