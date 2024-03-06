namespace McDermott.Application.Features.Commands.Patient
{
    public class InsurancePolicyCommand
    {
        #region Get

        public class GetInsurancePolicyQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<List<InsurancePolicyDto>>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate;
        }

        public class GetInsurancePolicyCountQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<long>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate;
        }

        #endregion Get

        #region Create

        public class CreateInsurancePolicyRequest(InsurancePolicyDto InsurancePolicyDto) : IRequest<InsurancePolicyDto>
        {
            public InsurancePolicyDto InsurancePolicyDto { get; set; } = InsurancePolicyDto;
        }

        #endregion Create

        #region Update

        public class UpdateInsurancePolicyRequest(InsurancePolicyDto InsurancePolicyDto) : IRequest<bool>
        {
            public InsurancePolicyDto InsurancePolicyDto { get; set; } = InsurancePolicyDto;
        }

        #endregion Update

        #region Delete

        public class DeleteInsurancePolicyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}