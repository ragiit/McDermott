using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskCommand
    {
        public class GetKioskQuery : IRequest<List<KioskDto>>;

        public class GetKioskByIdQuery : IRequest<KioskDto>
        {
            public long Id { get; set; }

            public GetKioskByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateKioskRequest : IRequest<KioskDto>
        {
            public KioskDto KioskDto { get; set; }

            public CreateKioskRequest(KioskDto KioskDto)
            {
                this.KioskDto = KioskDto;
            }
        }

        public class UpdateKioskRequest : IRequest<bool>
        {
            public KioskDto KioskDto { get; set; }

            public UpdateKioskRequest(KioskDto KioskDto)
            {
                this.KioskDto = KioskDto;
            }
        }

        public class DeleteKioskRequest : IRequest<bool>
        {
            public long Id { get; set; }

            public DeleteKioskRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListKioskRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListKioskRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}