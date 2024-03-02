namespace McDermott.Application.Features.Commands.Patient
{
    public class InsurancePolicyCommand
    {
        #region Get

        public class GetInsurancePolicyQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<List<InsurancePolicyDto>>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate;
        }

        public class GetInsurancePolicyCountQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<int>
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

        public class DeleteInsurancePolicyRequest(int id = 0, List<int>? ids = null) : IRequest<bool>
        {
            public int Id { get; set; } = id;
            public List<int> Ids { get; set; } = ids ?? [];
        }

        #endregion Delete
    }
}