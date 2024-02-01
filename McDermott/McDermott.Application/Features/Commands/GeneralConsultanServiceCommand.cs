using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands
{
    public class GeneralConsultanServiceCommand
    {
        public class GetGeneralConsultanServiceQuery : IRequest<List<GeneralConsultanServiceDto>>;

        public class GetGeneralConsultanServiceByIdQuery : IRequest<GeneralConsultanServiceDto>
        {
            public int Id { get; set; }

            public GetGeneralConsultanServiceByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateGeneralConsultanServiceRequest : IRequest<GeneralConsultanServiceDto>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; }

            public CreateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto)
            {
                this.GeneralConsultanServiceDto = GeneralConsultanServiceDto;
            }
        }

        public class UpdateGeneralConsultanServiceRequest : IRequest<bool>
        {
            public GeneralConsultanServiceDto GeneralConsultanServiceDto { get; set; }

            public UpdateGeneralConsultanServiceRequest(GeneralConsultanServiceDto GeneralConsultanServiceDto)
            {
                this.GeneralConsultanServiceDto = GeneralConsultanServiceDto;
            }
        }

        public class DeleteGeneralConsultanServiceRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteGeneralConsultanServiceRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListGeneralConsultanServiceRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListGeneralConsultanServiceRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}
