namespace McDermott.Application.Features.Commands.Config
{
    public class CompanyCommand
    {
        public class GetCompanyQuery : IRequest<List<CompanyDto>>;

        public class GetCompanyByIdQuery : IRequest<CompanyDto>
        {
             public long Id { get; set; }

            public GetCompanyByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateCompanyRequest : IRequest<CompanyDto>
        {
            public CompanyDto CompanyDto { get; set; }

            public CreateCompanyRequest(CompanyDto CompanyDto)
            {
                this.CompanyDto = CompanyDto;
            }
        }

        public class UpdateCompanyRequest : IRequest<bool>
        {
            public CompanyDto CompanyDto { get; set; }

            public UpdateCompanyRequest(CompanyDto CompanyDto)
            {
                this.CompanyDto = CompanyDto;
            }
        }

        public class DeleteCompanyRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteCompanyRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListCompanyRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListCompanyRequest(List<long> Id)
            {
                this.Id = Id;
            }
        }
    }
}