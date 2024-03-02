using McDermott.Application.Dtos.Queue;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskDepartementCommand
    {
        public class GetKioskDepartementQuery : IRequest<List<KioskDepartementDto>>;

        public class GetKioskDepartementByIdQuery : IRequest<KioskDepartementDto>
        {
            public int Id { get; set; }

            public GetKioskDepartementByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateKioskDepartementRequest : IRequest<KioskDepartementDto>
        {
            public KioskDepartementDto KioskDepartementDto { get; set; }

            public CreateKioskDepartementRequest(KioskDepartementDto KioskDepartementDto)
            {
                this.KioskDepartementDto = KioskDepartementDto;
            }
        }

        public class UpdateKioskDepartementRequest : IRequest<bool>
        {
            public KioskDepartementDto KioskDepartementDto { get; set; }

            public UpdateKioskDepartementRequest(KioskDepartementDto KioskDepartementDto)
            {
                this.KioskDepartementDto = KioskDepartementDto;
            }
        }

        public class DeleteKioskDepartementRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteKioskDepartementRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListKioskDepartementRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListKioskDepartementRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}