namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientFamilyRelationCommand
    {
        #region Get

        public class GetSinglePatientFamilyRelationQuery : IRequest<PatientFamilyRelationDto>
        {
            public List<Expression<Func<PatientFamilyRelation, object>>> Includes { get; set; }
            public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; set; }
            public Expression<Func<PatientFamilyRelation, PatientFamilyRelation>> Select { get; set; }

            public List<(Expression<Func<PatientFamilyRelation, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetPatientFamilyRelationQuery : IRequest<(List<PatientFamilyRelationDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<PatientFamilyRelation, object>>> Includes { get; set; }
            public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; set; }
            public Expression<Func<PatientFamilyRelation, PatientFamilyRelation>> Select { get; set; }

            public List<(Expression<Func<PatientFamilyRelation, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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