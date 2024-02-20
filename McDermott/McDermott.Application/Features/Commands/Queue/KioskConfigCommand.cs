using McDermott.Application.Dtos.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskConfigCommand
    {
        public class GetKioskConfigQuery : IRequest<List<KioskConfigDto>>;

        public class GetKioskConfigByIdQuery : IRequest<KioskConfigDto>
        {
            public int Id { get; set; }

            public GetKioskConfigByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteKioskConfigRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListKioskConfigRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListKioskConfigRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
