using McDermott.Application.Dtos;

namespace McDermott.Application.Features.Commands
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
                this.Id = id;
            }
        }
        #endregion 
    }
}