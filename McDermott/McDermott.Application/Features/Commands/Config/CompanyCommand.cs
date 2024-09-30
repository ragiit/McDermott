namespace McDermott.Application.Features.Commands.Config
{
    public class CompanyCommand
    {
        #region

        public class GetCompanyQuery(Expression<Func<Company, bool>>? predicate = null, int pageIndex = 0, int? pageSize = 10, string? searchTerm = "", bool removeCache = false, List<Expression<Func<Company, object>>>? includes = null, Expression<Func<Company, Company>>? select = null) : IRequest<(List<CompanyDto>, int pageIndex, int pageSize, int pageCount)>
        {
            public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
            public string SearchTerm { get; } = searchTerm!;
            public int PageIndex { get; } = pageIndex;
            public int PageSize { get; } = pageSize ?? 10;

            public List<Expression<Func<Company, object>>> Includes { get; } = includes!;
            public Expression<Func<Company, Company>>? Select { get; } = select!;
        }

        public class BulkValidateCompanyQuery(List<CompanyDto> CompanysToValidate) : IRequest<List<CompanyDto>>
        {
            public List<CompanyDto> CompanysToValidate { get; } = CompanysToValidate;
        }

        public class ValidateCompanyQuery(Expression<Func<Company, bool>>? predicate = null) : IRequest<bool>
        {
            public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
        }

        #endregion GET (Bisa berdasarkan kondisi WHERE juga)

        #region CREATE

        public class CreateCompanyRequest(CompanyDto CompanyDto) : IRequest<CompanyDto>
        {
            public CompanyDto CompanyDto { get; set; } = CompanyDto;
        }

        public class CreateListCompanyRequest(List<CompanyDto> GeneralConsultanCPPTDtos) : IRequest<List<CompanyDto>>
        {
            public List<CompanyDto> CompanyDtos { get; set; } = GeneralConsultanCPPTDtos;
        }

        #endregion CREATE

        #region Update

        public class UpdateCompanyRequest(CompanyDto CompanyDto) : IRequest<CompanyDto>
        {
            public CompanyDto CompanyDto { get; set; } = CompanyDto;
        }

        public class UpdateListCompanyRequest(List<CompanyDto> CompanyDtos) : IRequest<List<CompanyDto>>
        {
            public List<CompanyDto> CompanyDtos { get; set; } = CompanyDtos;
        }

        #endregion Update

        #region DELETE

        public class DeleteCompanyRequest(long? id = null, List<long>? ids = null) : IRequest<bool>
        {
            public long Id { get; set; } = id ?? 0;
            public List<long> Ids { get; set; } = ids ?? [];
        }

        #endregion DELETE
    }
}