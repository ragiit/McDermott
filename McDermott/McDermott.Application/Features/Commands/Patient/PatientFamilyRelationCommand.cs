namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientFamilyRelationCommand
    {
        #region Get

        public class GetPatientFamilyByPatientQuery : IRequest<List<PatientFamilyRelationDto>>
        {
            public Expression<Func<PatientFamilyRelation, bool>>? Predicate { get; }
            public bool ReturnSingle { get; } = false; // Parameter untuk menentukan apakah ingin mengembalikan satu data atau daftar data

            public GetPatientFamilyByPatientQuery(Expression<Func<PatientFamilyRelation, bool>>? predicate = null, bool ReturnSingle = false)
            {
                Predicate = predicate;
                this.ReturnSingle = ReturnSingle;
            }
        }

        #endregion Get

        #region Create

        public class CreatePatientFamilyRelationRequest : IRequest<PatientFamilyRelationDto>
        {
            public PatientFamilyRelationDto PatientFamilyRelationDto { get; set; }

            public CreatePatientFamilyRelationRequest(PatientFamilyRelationDto PatientFamilyRelationDto)
            {
                this.PatientFamilyRelationDto = PatientFamilyRelationDto;
            }
        }

        public class CreateListPatientFamilyRelationRequest : IRequest<List<PatientFamilyRelationDto>>
        {
            public List<PatientFamilyRelationDto> PatientFamilyRelationDto { get; set; }

            public CreateListPatientFamilyRelationRequest(List<PatientFamilyRelationDto> PatientFamilyRelationDto)
            {
                this.PatientFamilyRelationDto = PatientFamilyRelationDto;
            }
        }

        #endregion Create
    }
}