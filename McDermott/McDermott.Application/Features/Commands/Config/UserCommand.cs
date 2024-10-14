namespace McDermott.Application.Features.Commands.Config
{
    public class UserCommand
    {
        #region Get

        public class GetUserQuery2(Expression<Func<User, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<User, object>>>? includes = null, Expression<Func<User, User>>? select = null) : IRequest<(List<UserDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<User, object>>> Includes { get; } = includes!;
            public Expression<Func<User, User>>? Select { get; } = select!;
        }

        public class ValidateUserQuery(Expression<Func<User, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
        }

        public class GetUserQuerys(Expression<Func<User, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<UserDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; set; } = pageSize ?? 10;
        }

        public class BulkValidateUserQuery(List<UserDto> UsersToValidate) : IRequest<List<UserDto>>
        {
            public List<UserDto> UsersToValidate { get; } = UsersToValidate;
        }

        public class BulkValidateEmployeeQuery(List<UserDto> UsersToValidate) : IRequest<List<UserDto>>
        {
            public List<UserDto> UsersToValidate { get; } = UsersToValidate;
        }

        public class BulkValidatePatientQuery(List<UserDto> UsersToValidate) : IRequest<List<UserDto>>
        {
            public List<UserDto> UsersToValidate { get; } = UsersToValidate;
        }

        public class GetUserQuery(Expression<Func<User, bool>>? predicate = null, bool removeCache = false) : IRequest<List<UserDto>>
        {
            public Expression<Func<User, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        //public class GetSingleUserQuery(Expression<Func<User, bool>>? predicate = null) : IRequest<UserDto>
        //{
        //    public Expression<Func<User, bool>> Predicate { get; } = predicate!;
        //}

        public class GetSingleUserQuery : IRequest<UserDto>
        {
            public List<Expression<Func<User, object>>> Includes { get; set; }
            public Expression<Func<User, bool>> Predicate { get; set; }
            public Expression<Func<User, User>> Select { get; set; }
            public Expression<Func<User, object>> OrderBy { get; set; }
            public bool IsDescending { get; set; } = false; // default to ascending
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

        public class CreateListUserRequest(List<UserDto> UserDtos) : IRequest<List<UserDto>>
        {
            public List<UserDto> UserDtos { get; set; } = UserDtos;
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