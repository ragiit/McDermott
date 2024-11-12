namespace McDermott.Application.Features.Commands.Config
{
    public class CompanyCommand
    {
        #region

        public class GetSingleCompanyQuery : IRequest<CompanyDto>
        {
            public List<Expression<Func<Company, object>>> Includes { get; set; }
            public Expression<Func<Company, bool>> Predicate { get; set; }
            public Expression<Func<Company, Company>> Select { get; set; }

            public List<(Expression<Func<Company, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
        }

        public class GetCompanyQuery : IRequest<(List<CompanyDto>, int PageIndex, int PageSize, int PageCount)>
        {
            public List<Expression<Func<Company, object>>> Includes { get; set; }
            public Expression<Func<Company, bool>> Predicate { get; set; }
            public Expression<Func<Company, Company>> Select { get; set; }

            public List<(Expression<Func<Company, object>> OrderBy, bool IsDescending)> OrderByList { get; set; } = [];

            public bool IsDescending { get; set; } = false; // default to ascending
            public int PageIndex { get; set; } = 0;
            public int PageSize { get; set; } = 10;
            public bool IsGetAll { get; set; } = false;
            public string SearchTerm { get; set; }
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