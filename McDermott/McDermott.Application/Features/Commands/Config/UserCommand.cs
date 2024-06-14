namespace McDermott.Application.Features.Commands.Config
{
    public class UserCommand
    {
        #region Get

        public class GetUserQuery(Expression<Func<User, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UserDto>>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        public class GetUserInfoGroupQuery(Expression<Func<User, bool>>? predicate = null) : IRequest<List<UserDto>>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
        }

        public class GetDataUserForKioskQuery : IRequest<List<UserDto>>
        {
            public UserDto userDto { get; set; }
            public string Number { get; set; }

            public GetDataUserForKioskQuery(string Number)
            {
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

        public class DeleteUserRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}