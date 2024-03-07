namespace McDermott.Application.Features.Commands.Medical
{
    public class InsuranceCommand
    {
        public class GetInsuranceQuery : IRequest<List<InsuranceDto>>;

        public class GetInsuranceByIdQuery : IRequest<InsuranceDto>
        {
             public long Id { get; set; }

            public GetInsuranceByIdQuery(long id)
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
             public long Id { get; set; }

            public DeleteInsuranceRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListInsuranceRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListInsuranceRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}