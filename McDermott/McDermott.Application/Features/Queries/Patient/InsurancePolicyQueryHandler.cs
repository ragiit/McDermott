using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static McDermott.Application.Features.Commands.Patient.InsurancePolicyCommand;

namespace McDermott.Application.Features.Queries.Patient
{
    public class InsurancePolicyQueryHandler
    {
        #region Get
        internal class GetInsurancePolicyQueryHandler : IRequestHandler<GetInsurancePolicyQuery, List<InsurancePolicyDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetInsurancePolicyQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<List<InsurancePolicyDto>> Handle(GetInsurancePolicyQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _unitOfWork.Repository<InsurancePolicy>().GetAsync(
                      query.Predicate,
                          x => x
                          .Include(z => z.Insurance)
                          .Include(z => z.User), cancellationToken);

                    return result.Adapt<List<InsurancePolicyDto>>();
                }
                catch (Exception e)
                {
                    return [];
                }
            }
        }
        #endregion
    }
}
