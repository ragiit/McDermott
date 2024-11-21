namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientAllergyCommand
    {
        #region GET

        public class GetPatientAllergyQuery(Expression<Func<PatientAllergy, bool>>? predicate = null, bool removeCache = false) : IRequest<List<PatientAllergyDto>>
        {
            public Expression<Func<PatientAllergy, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion GET

        #region CREATE

        public class CreatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto) : IRequest<PatientAllergyDto>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; } = PatientAllergyDto;
        }

        public class CreateListPatientAllergyRequest(List<PatientAllergyDto> PatientAllergyDtos) : IRequest<List<PatientAllergyDto>>
        {
            public List<PatientAllergyDto> PatientAllergyDtos { get; set; } = PatientAllergyDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto) : IRequest<PatientAllergyDto>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; } = PatientAllergyDto;
        }

        public class UpdateListPatientAllergyRequest(List<PatientAllergyDto> PatientAllergyDtos) : IRequest<List<PatientAllergyDto>>
        {
            public List<PatientAllergyDto> PatientAllergyDtos { get; set; } = PatientAllergyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeletePatientAllergyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}