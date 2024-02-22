using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Transaction;

namespace McDermott.Application.Features.Commands.Queue
{
    public class KioskCommand
    {
        public class GetKioskQuery : IRequest<List<KioskDto>>;

        public class GetKioskByIdQuery : IRequest<KioskDto>
        {
            public int Id { get; set; }

            public GetKioskByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteKioskRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListKioskRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListKioskRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
