using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Patient
{
    public class InsurancePolicyCommand
    {
        #region Get
        public class GetInsurancePolicyQuery : IRequest<List<InsurancePolicyDto>>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; }

            public GetInsurancePolicyQuery(Expression<Func<InsurancePolicy, bool>> predicate)
            {
                Predicate = predicate;
            }
        }
        #endregion
    }
}
