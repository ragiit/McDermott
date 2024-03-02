namespace McDermott.Application.Features.Commands.Medical
{
    public class NursingDiagnosesCommand
    {
        #region Get

        public class GetNursingDiagnosesQuery(Expression<Func<NursingDiagnoses, bool>>? predicate = null) : IRequest<List<NursingDiagnosesDto>>
        {
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; } = predicate;
        }

        #endregion Get

        #region Create

        public class CreateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        public class CreateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
        }

        #endregion Create

        #region Update

        public class UpdateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<bool>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        #endregion Update

        #region Delete

        public class DeleteNursingDiagnosesRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}