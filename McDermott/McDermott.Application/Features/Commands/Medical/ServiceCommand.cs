using McDermott.Application.Dtos.Medical;

namespace McDermott.Application.Features.Commands.Medical
{
    public class ServiceCommand
    {
        public class GetServiceQuery : IRequest<List<ServiceDto>>;

        public class GetServiceByIdQuery : IRequest<ServiceDto>
        {
            public int Id { get; set; }

            public GetServiceByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateServiceRequest : IRequest<ServiceDto>
        {
            public ServiceDto ServiceDto { get; set; }

            public CreateServiceRequest(ServiceDto ServiceDto)
            {
                this.ServiceDto = ServiceDto;
            }
        }

        public class UpdateServiceRequest : IRequest<bool>
        {
            public ServiceDto ServiceDto { get; set; }

            public UpdateServiceRequest(ServiceDto ServiceDto)
            {
                this.ServiceDto = ServiceDto;
            }
        }

        public class DeleteServiceRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteServiceRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListServiceRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListServiceRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}