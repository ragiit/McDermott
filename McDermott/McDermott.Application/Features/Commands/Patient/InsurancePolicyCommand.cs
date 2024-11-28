namespace McDermott.Application.Features.Commands.Patient
{
    public class InsurancePolicyCommand
    {
        #region GET

        public class GetInsurancePolicyQuery : IRequest<(List<InsurancePolicyDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<InsurancePolicy, object>>> Includes { get; set; }
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; set; }
            public Expression<Func<InsurancePolicy, InsurancePolicy>>? Select { get; set; }

            public List<(Expression<Func<InsurancePolicy, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleInsurancePolicyQuery : IRequest<InsurancePolicyDto>
        {
            public List<Expression<Func<InsurancePolicy, object>>> Includes { get; set; }
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; set; }
            public Expression<Func<InsurancePolicy, InsurancePolicy>> Select { get; set; }

            public List<(Expression<Func<InsurancePolicy, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateInsurancePolicyQuery(Expression<Func<InsurancePolicy, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<InsurancePolicy, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

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