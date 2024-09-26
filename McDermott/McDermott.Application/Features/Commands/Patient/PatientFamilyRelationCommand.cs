namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientFamilyRelationCommand
    {
        #region Get

        public class GetPatientFamilyRelationQuery(Expression<Func<PatientFamilyRelation, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<PatientFamilyRelationDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; set; } = pageSize ?? 10;
        }

        public class BulkValidatePatientFamilyRelationQuery(List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate) : IRequest<List<PatientFamilyRelationDto>>
        {
            public List<PatientFamilyRelationDto> PatientFamilyRelationsToValidate { get; } = PatientFamilyRelationsToValidate;
        }

        public class ValidatePatientFamilyRelationQuery(Expression<Func<PatientFamilyRelation, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; } = predicate!;
        }

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

        #region Update

        public class UpdatePatientFamilyRelationRequest(PatientFamilyRelationDto PatientFamilyRelationDto) : IRequest<PatientFamilyRelationDto>
        {
            public PatientFamilyRelationDto PatientFamilyRelationDto { get; set; } = PatientFamilyRelationDto;
        }

        public class UpdateListPatientFamilyRelationRequest(List<PatientFamilyRelationDto> PatientFamilyRelationDtos) : IRequest<List<PatientFamilyRelationDto>>
        {
            public List<PatientFamilyRelationDto> PatientFamilyRelationDtos { get; set; } = PatientFamilyRelationDtos;
        }

        #endregion Update

        #region DELETE

        public class DeletePatientFamilyRelationRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}