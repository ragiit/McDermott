namespace McDermott.Application.Features.Commands.Medical
{
    public class NursingDiagnosesCommand
    {
        #region GET 

        public class GetNursingDiagnosesQuery(Expression<Func<NursingDiagnoses, bool>>? predicate = null, bool removeCache = false) : IRequest<List<NursingDiagnosesDto>>
        {
            public Expression<Func<NursingDiagnoses, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        public class CreateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateNursingDiagnosesRequest(NursingDiagnosesDto NursingDiagnosesDto) : IRequest<NursingDiagnosesDto>
        {
            public NursingDiagnosesDto NursingDiagnosesDto { get; set; } = NursingDiagnosesDto;
        }

        public class UpdateListNursingDiagnosesRequest(List<NursingDiagnosesDto> NursingDiagnosesDtos) : IRequest<List<NursingDiagnosesDto>>
        {
            public List<NursingDiagnosesDto> NursingDiagnosesDtos { get; set; } = NursingDiagnosesDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteNursingDiagnosesRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}