using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskConfigCommand
    {
        public class GetKioskConfigQuery : IRequest<List<KioskConfigDto>>;

        public class GetKioskConfigByIdQuery : IRequest<KioskConfigDto>
        {
             public long Id { get; set; }

            public GetKioskConfigByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateKioskConfigRequest : IRequest<KioskConfigDto>
        {
            public KioskConfigDto KioskConfigDto { get; set; }

            public CreateKioskConfigRequest(KioskConfigDto KioskConfigDto)
            {
                this.KioskConfigDto = KioskConfigDto;
            }
        }

        public class UpdateKioskConfigRequest : IRequest<bool>
        {
            public KioskConfigDto KioskConfigDto { get; set; }

            public UpdateKioskConfigRequest(KioskConfigDto KioskConfigDto)
            {
                this.KioskConfigDto = KioskConfigDto;
            }
        }

        public class DeleteKioskConfigRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteKioskConfigRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListKioskConfigRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListKioskConfigRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}