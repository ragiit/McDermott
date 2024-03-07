using McDermott.Application.Dtos.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Queue
{
    public class DetailQueueDisplayCommand
    {
        public class GetDetailQueueDisplayQuery : IRequest<List<DetailQueueDisplayDto>>;

        public class GetDetailQueueDisplayByIdQuery : IRequest<DetailQueueDisplayDto>
        {
            public long Id { get; set; }

            public GetDetailQueueDisplayByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateDetailQueueDisplayRequest : IRequest<DetailQueueDisplayDto>
        {
            public DetailQueueDisplayDto DetailQueueDisplayDto { get; set; }

            public CreateDetailQueueDisplayRequest(DetailQueueDisplayDto DetailQueueDisplayDto)
            {
                this.DetailQueueDisplayDto = DetailQueueDisplayDto;
            }
        }

        public class UpdateDetailQueueDisplayRequest : IRequest<bool>
        {
            public DetailQueueDisplayDto DetailQueueDisplayDto { get; set; }

            public UpdateDetailQueueDisplayRequest(DetailQueueDisplayDto DetailQueueDisplayDto)
            {
                this.DetailQueueDisplayDto = DetailQueueDisplayDto;
            }
        }

        public class DeleteDetailQueueDisplayRequest : IRequest<bool>
        {
            public long Id { get; set; }

            public DeleteDetailQueueDisplayRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListDetailQueueDisplayRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListDetailQueueDisplayRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}
