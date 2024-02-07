using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class UserCommand
    {
        #region Get
        public class GetUserQuery : IRequest<List<UserDto>>;

        public class GetUserByIdQuery : IRequest<UserDto>
        {
            public int Id { get; set; }

            public GetUserByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class GetUserByEmailPasswordQuery : IRequest<UserDto>
        {
            public UserDto UserDto { get; set; }

            public GetUserByEmailPasswordQuery(UserDto UserDto)
            {
                this.UserDto = UserDto;
            }
        }

        public class GetDataUserForKioskQuery : IRequest<List<UserDto>>
        {
            public UserDto userDto { get; set; }
            public string Types {  get; set; }
            public string Number {  get; set; }

            public GetDataUserForKioskQuery(string Types, string Number)
            {
                this.userDto = userDto;
                this.Types = Types;
                this.Number = Number;
            }
        }

        #endregion

        #region Create
        public class CreateUserRequest : IRequest<UserDto>
        {
            public UserDto UserDto { get; set; }

            public CreateUserRequest(UserDto UserDto)
            {
                this.UserDto = UserDto;
            }
        }
        #endregion

        #region Update
        public class UpdateUserRequest : IRequest<bool>
        {
            public UserDto UserDto { get; set; }

            public UpdateUserRequest(UserDto UserDto)
            {
                this.UserDto = UserDto;
            }
        }
        #endregion

        #region Delete
        public class DeleteUserRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteUserRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListUserRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListUserRequest(List<int> id)
            {
                Id = id;
            }
        }
        #endregion 
    }
}