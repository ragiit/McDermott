namespace McDermott.Application.Features.Commands
{
    public class UserCommand
    {
        public class GetUserQuery : IRequest<List<UserDto>>;

        public class GetUserByIdQuery : IRequest<UserDto>
        {
            public int Id { get; set; }

            public GetUserByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateUserRequest : IRequest<UserDto>
        {
            public UserDto UserDto { get; set; }

            public CreateUserRequest(UserDto UserDto)
            {
                this.UserDto = UserDto;
            }
        }

        public class UpdateUserRequest : IRequest<bool>
        {
            public UserDto UserDto { get; set; }

            public UpdateUserRequest(UserDto UserDto)
            {
                this.UserDto = UserDto;
            }
        }

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
    }
}