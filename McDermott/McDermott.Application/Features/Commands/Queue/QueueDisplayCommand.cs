using McDermott.Application.Dtos.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Queue
{
    public class QueueDisplayCommand
    {
        public class GetQueueDisplayQuery : IRequest<List<QueueDisplayDto>>;

        public class GetQueueDisplayByIdQuery : IRequest<QueueDisplayDto>
        {
             public long Id { get; set; }

            public GetQueueDisplayByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateQueueDisplayRequest : IRequest<QueueDisplayDto>
        {
            public QueueDisplayDto QueueDisplayDto { get; set; }

            public CreateQueueDisplayRequest(QueueDisplayDto QueueDisplayDto)
            {
                this.QueueDisplayDto = QueueDisplayDto;
            }
        }

        public class UpdateQueueDisplayRequest : IRequest<bool>
        {
            public QueueDisplayDto QueueDisplayDto { get; set; }

            public UpdateQueueDisplayRequest(QueueDisplayDto QueueDisplayDto)
            {
                this.QueueDisplayDto = QueueDisplayDto;
            }
        }

        public class DeleteQueueDisplayRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteQueueDisplayRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListQueueDisplayRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListQueueDisplayRequest(List<long> id)
            {
                Id = id;

            }
        }
    }
}
