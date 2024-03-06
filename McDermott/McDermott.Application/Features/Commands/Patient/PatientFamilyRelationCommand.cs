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
        public class DeletePatientFamilyRelationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }
        #endregion
    }
}