namespace McDermott.Application.Features.Commands.Medical
{
    public class InsuranceCommand
    {
        #region GET

        public class GetInsuranceQuery : IRequest<(List<InsuranceDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Insurance, object>>> Includes { get; set; }
            public Expression<Func<Insurance, bool>> Predicate { get; set; }
            public Expression<Func<Insurance, Insurance>> Select { get; set; }

            public List<(Expression<Func<Insurance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetSingleInsuranceQuery : IRequest<InsuranceDto>
        {
            public List<Expression<Func<Insurance, object>>> Includes { get; set; }
            public Expression<Func<Insurance, bool>> Predicate { get; set; }
            public Expression<Func<Insurance, Insurance>> Select { get; set; }

            public List<(Expression<Func<Insurance, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class ValidateInsuranceQuery(Expression<Func<Insurance, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Insurance, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET

        #region CREATE

        public class CreateInsuranceRequest(InsuranceDto InsuranceDto) : IRequest<InsuranceDto>
        {
            public InsuranceDto InsuranceDto { get; set; } = InsuranceDto;
        }

        public class BulkValidateInsuranceQuery(List<InsuranceDto> InsurancesToValidate) : IRequest<List<InsuranceDto>>
        {
            public List<InsuranceDto> InsurancesToValidate { get; } = InsurancesToValidate;
        }

        public class CreateListInsuranceRequest(List<InsuranceDto> InsuranceDtos) : IRequest<List<InsuranceDto>>
        {
            public List<InsuranceDto> InsuranceDtos { get; set; } = InsuranceDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateInsuranceRequest(InsuranceDto InsuranceDto) : IRequest<InsuranceDto>
        {
            public InsuranceDto InsuranceDto { get; set; } = InsuranceDto;
        }

        public class UpdateListInsuranceRequest(List<InsuranceDto> InsuranceDtos) : IRequest<List<InsuranceDto>>
        {
            public List<InsuranceDto> InsuranceDtos { get; set; } = InsuranceDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteInsuranceRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}