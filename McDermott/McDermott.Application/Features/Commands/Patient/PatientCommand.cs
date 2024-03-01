namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientCommand
    {
        #region Get
        public class GetUserPatientQuery : IRequest<List<UserDto>>;

        public class GetPatientAllergyQuery(Expression<Func<PatientAllergy, bool>>? predicate = null) : IRequest<List<PatientAllergyDto>>
        {
            public Expression<Func<PatientAllergy, bool>> Predicate { get; } = predicate;
        }
        #endregion

        #region Create
        public class CreatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto) : IRequest<PatientAllergyDto>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; } = PatientAllergyDto;
        }
        #endregion

        #region Update
        public class UpdatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto) : IRequest<bool>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; } = PatientAllergyDto;
        }

        #endregion
    }
}
