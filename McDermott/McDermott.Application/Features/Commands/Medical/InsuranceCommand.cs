namespace McDermott.Application.Features.Commands.Medical
{
    public class InsuranceCommand
    {
        #region GET

        public class GetInsuranceQuery(Expression<Func<Insurance, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false) : IRequest<(List<InsuranceDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Insurance, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;
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