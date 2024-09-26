using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.AllQueries
{
    public class CountModelCommand
    {
        #region GET

        public class GetInsurancePolicyCountQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate!;
        }

        public class GetPrescriptionCountQuery(Expression<Func<Domain.Entities.Pharmacy, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<Domain.Entities.Pharmacy, bool>> Predicate { get; } = predicate!;
        }

        public class GetGeneralConsultationCountQuery(Expression<Func<GeneralConsultanService, bool>>? predicate = null) : IRequest<int>
        {
            public Expression<Func<GeneralConsultanService, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

    }
}