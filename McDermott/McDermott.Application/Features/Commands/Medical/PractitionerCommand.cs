using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Medical
{
    public class PractitionerCommand
    {
        #region Get
        public class GetUserPractitionerQuery : IRequest<List<UserDto>>;
        #endregion
    }
}
