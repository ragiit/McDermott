

namespace McDermott.Application.Features.Commands.Patient
{
    public class PatientFamilyRelationCommand
    {
        #region Get
        public class GetPatientFamilyByPatientQuery : IRequest<List<PatientFamilyRelationDto>>
        {
            public Expression<Func<PatientFamilyRelation, bool>> Predicate { get; }

            public GetPatientFamilyByPatientQuery(Expression<Func<PatientFamilyRelation, bool>> predicate)
            {
                Predicate = predicate;
            }
        }
        #endregion
    }
}
