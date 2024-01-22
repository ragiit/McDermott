namespace McDermott.Application.Features.Commands
{
    public class CompanyCommand
    {
        public class GetCompanyQuery : IRequest<List<CompanyDto>>;

        public class GetCompanyByIdQuery : IRequest<CompanyDto>
        {
            public int Id { get; set; }

            public GetCompanyByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteCompanyRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListCompanyRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListCompanyRequest(List<int> Id)
            {
                this.Id = Id;
            }
        }
    }
}