namespace McDermott.Application.Features.Commands.Medical
{
    public class HealthCenterCommand
    {
        public class GetHealthCenterQuery : IRequest<List<HealthCenterDto>>;

        public class GetHealthCenterByIdQuery : IRequest<HealthCenterDto>
        {
             public long Id { get; set; }

            public GetHealthCenterByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateHealthCenterRequest : IRequest<HealthCenterDto>
        {
            public HealthCenterDto HealthCenterDto { get; set; }

            public CreateHealthCenterRequest(HealthCenterDto HealthCenterDto)
            {
                this.HealthCenterDto = HealthCenterDto;
            }
        }

        public class UpdateHealthCenterRequest : IRequest<bool>
        {
            public HealthCenterDto HealthCenterDto { get; set; }

            public UpdateHealthCenterRequest(HealthCenterDto HealthCenterDto)
            {
                this.HealthCenterDto = HealthCenterDto;
            }
        }

        public class DeleteHealthCenterRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteHealthCenterRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListHealthCenterRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListHealthCenterRequest(List<long> id)
            {
                this.Id = id;
            }
        }
    }
}