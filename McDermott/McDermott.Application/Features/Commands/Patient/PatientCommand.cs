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

        #endregion Get

        #region Create

        public class CreatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto) : IRequest<PatientAllergyDto>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; } = PatientAllergyDto;
        }

        #endregion Create

        #region Update

        public class UpdatePatientAllergyRequest(PatientAllergyDto PatientAllergyDto) : IRequest<PatientAllergyDto>
        {
            public PatientAllergyDto PatientAllergyDto { get; set; } = PatientAllergyDto;
        }

        #endregion Update
    }
}