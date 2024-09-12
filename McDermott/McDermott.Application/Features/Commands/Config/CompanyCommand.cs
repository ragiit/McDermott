namespace McDermott.Application.Features.Commands.Config
{
    public class CompanyCommand
    {
        #region GET (Bisa berdasarkan kondisi WHERE juga)

        public class GetCompanyQuery(Expression<Func<Company, bool>>? predicate = null, bool removeCache = false) : IRequest<List<CompanyDto>>
        {
            public Expression<Func<Company, bool>> Predicate { get; } = predicate!;
            public bool RemoveCache { get; } = removeCache!;
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