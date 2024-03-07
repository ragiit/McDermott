using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskQueueCommand
    {
        public class GetKioskQueueQuery : IRequest<List<KioskQueueDto>>;

        public class GetKioskQueueByIdQuery : IRequest<KioskQueueDto>
        {
             public long Id { get; set; }

            public GetKioskQueueByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateKioskQueueRequest : IRequest<KioskQueueDto>
        {
            public KioskQueueDto KioskQueueDto { get; set; }

            public CreateKioskQueueRequest(KioskQueueDto KioskQueueDto)
            {
                this.KioskQueueDto = KioskQueueDto;
            }
        }

        public class UpdateKioskQueueRequest : IRequest<bool>
        {
            public KioskQueueDto KioskQueueDto { get; set; }

            public UpdateKioskQueueRequest(KioskQueueDto KioskQueueDto)
            {
                this.KioskQueueDto = KioskQueueDto;
            }
        }

        public class DeleteKioskQueueRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteKioskQueueRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListKioskQueueRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListKioskQueueRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}