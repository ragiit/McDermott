using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Patient
{
    public class FamilyCommand
    {
        public class GetFamilyQuery : IRequest<List<FamilyDto>>;

        public class GetFamilyByIdQuery : IRequest<FamilyDto>
        {
            public int Id { get; set; }

            public GetFamilyByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateFamilyRequest : IRequest<FamilyDto>
        {
            public FamilyDto FamilyDto { get; set; }

            public CreateFamilyRequest(FamilyDto FamilyDto)
            {
                this.FamilyDto = FamilyDto;
            }
        }

        public class UpdateFamilyRequest : IRequest<bool>
        {
            public FamilyDto FamilyDto { get; set; }

            public UpdateFamilyRequest(FamilyDto FamilyDto)
            {
                this.FamilyDto = FamilyDto;
            }
        }

        public class DeleteFamilyRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteFamilyRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListFamilyRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListFamilyRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
