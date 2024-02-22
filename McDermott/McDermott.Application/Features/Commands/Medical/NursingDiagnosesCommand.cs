using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Medical
{
    public class NursingDiagnosesCommand
    {
        #region Get

        public class GetNursingDiagnosesQuery(Expression<Func<NursingDiagnoses, bool>>? predicate = null) : IRequest<List<NursingDiagnosesDto>>
        {
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; } = predicate;
        }

        #endregion

        #region Create

        public class CreateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }
        public class CreateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
        }
        #endregion

        #region Update
        public class UpdateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<bool>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        #endregion

        #region Delete
        public class DeleteNursingDiagnosesRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}
