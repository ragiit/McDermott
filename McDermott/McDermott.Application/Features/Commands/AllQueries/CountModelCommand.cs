namespace McDermott.Application.Features.Commands.AllQueries
{
    public class CountModelCommand
    {
        #region GET

        public class GetInsurancePolicyCountQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate!;
        }

        public class GetWellnessAttendanceCountQuery(Expression<Func<WellnessProgramAttendance, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<WellnessProgramAttendance, bool>> Predicate { get; } = predicate!;
        }

        public class GetPrescriptionCountQuery(Expression<Func<Domain.Entities.Pharmacy, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<Domain.Entities.Pharmacy, bool>> Predicate { get; } = predicate!;
        }

        public class GetGeneralConsultationCountQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
        }

        public class GetClaimHistoryCountQuery(Expression<Func<ClaimHistory, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<ClaimHistory, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET
    }
}