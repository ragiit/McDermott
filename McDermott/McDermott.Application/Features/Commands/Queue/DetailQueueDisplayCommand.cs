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
            public int Id { get; set; }

            public GetDetailQueueDisplayByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteDetailQueueDisplayRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListDetailQueueDisplayRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDetailQueueDisplayRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
