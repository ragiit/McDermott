namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientFamilyRelationCommand
    {
        #region Get

        public class GetPatientFamilyByPatientQuery(Expression<Func<PatientFamilyRelation, bool>>? predicate = null) : IRequest<List<PatientFamilyRelationDto>>
        {
            public Expression<Func<PatientFamilyRelation, bool>>? Predicate { get; } = predicate;
        }

        #endregion Get

        #region Create

        public class CreatePatientFamilyRelationRequest(PatientFamilyRelationDto PatientFamilyRelationDto) : IRequest<PatientFamilyRelationDto>
        {
            public PatientFamilyRelationDto PatientFamilyRelationDto { get; set; } = PatientFamilyRelationDto;
        }

        public class CreateListPatientFamilyRelationRequest(List<PatientFamilyRelationDto> PatientFamilyRelationDto) : IRequest<List<PatientFamilyRelationDto>>
        {
            public List<PatientFamilyRelationDto> PatientFamilyRelationDto { get; set; } = PatientFamilyRelationDto;
        }

        #endregion Create

        #region DELETE 
        public class DeletePatientFamilyRelationRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}