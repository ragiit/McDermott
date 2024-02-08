using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientCommand
    {
        #region Get
        public class GetUserPatientQuery : IRequest<List<UserDto>>;
        #endregion
    }
}
