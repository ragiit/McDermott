namespace McDermott.Application.Features.Commands.Patient
{
    public class InsurancePolicyCommand
    {
        #region GET 

        public class GetInsurancePolicyQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null, bool removeCache = false) : IRequest<List<InsurancePolicyDto>>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
        }

        #endregion  

        #region CREATE

        public class CreateInsurancePolicyRequest(InsurancePolicyDto InsurancePolicyDto) : IRequest<InsurancePolicyDto>
        {
            public InsurancePolicyDto InsurancePolicyDto { get; set; } = InsurancePolicyDto;
        }

        public class CreateListInsurancePolicyRequest(List<InsurancePolicyDto> InsurancePolicyDtos) : IRequest<List<InsurancePolicyDto>>
        {
            public List<InsurancePolicyDto> InsurancePolicyDtos { get; set; } = InsurancePolicyDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateInsurancePolicyRequest(InsurancePolicyDto InsurancePolicyDto) : IRequest<InsurancePolicyDto>
        {
            public InsurancePolicyDto InsurancePolicyDto { get; set; } = InsurancePolicyDto;
        }

        public class UpdateListInsurancePolicyRequest(List<InsurancePolicyDto> InsurancePolicyDtos) : IRequest<List<InsurancePolicyDto>>
        {
            public List<InsurancePolicyDto> InsurancePolicyDtos { get; set; } = InsurancePolicyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteInsurancePolicyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}