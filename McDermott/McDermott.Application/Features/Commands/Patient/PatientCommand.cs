
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

        #region Create
        public class CreatePatientAllergyRequest : IRequest<PatientAllergyDto>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; }

            public CreatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto)
            {
                this.PatientAllergyDto = PatientAllergyDto;
            }
        }
        #endregion
    }
}
