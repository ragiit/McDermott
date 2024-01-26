namespace McDermott.Application.Features.Commands
{
    public class InsuranceCommand
    {
        public class GetInsuranceQuery : IRequest<List<InsuranceDto>>;

        public class GetInsuranceByIdQuery : IRequest<InsuranceDto>
        {
            public int Id { get; set; }

            public GetInsuranceByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateInsuranceRequest : IRequest<InsuranceDto>
        {
            public InsuranceDto InsuranceDto { get; set; }

            public CreateInsuranceRequest(InsuranceDto InsuranceDto)
            {
                this.InsuranceDto = InsuranceDto;
            }
        }

        public class UpdateInsuranceRequest : IRequest<bool>
        {
            public InsuranceDto InsuranceDto { get; set; }

            public UpdateInsuranceRequest(InsuranceDto InsuranceDto)
            {
                this.InsuranceDto = InsuranceDto;
            }
        }

        public class DeleteInsuranceRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteInsuranceRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListInsuranceRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListInsuranceRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}