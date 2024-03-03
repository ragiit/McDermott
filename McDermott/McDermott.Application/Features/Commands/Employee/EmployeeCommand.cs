namespace McDermott.Application.Features.Commands.Employee
{
    public class EmployeeCommand
    {
        #region Get

        public class GetUserEmployeeQuery : IRequest<List<UserDto>>;

        #endregion Get
    }
}