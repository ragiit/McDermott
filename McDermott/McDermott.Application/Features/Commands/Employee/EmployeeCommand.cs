using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Employee
{
    public class EmployeeCommand
    {
        #region Get
        public class GetUserEmployeeQuery : IRequest<List<UserDto>>;
        #endregion
    }
}
