using McDermott.Application.Dtos.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands.Medical
{
    public class DiseaseCategoryCommand
    {
        public class GetDiseaseCategoryQuery : IRequest<List<DiseaseCategoryDto>>;

        public class GetDiseaseCategoryByIdQuery : IRequest<DiseaseCategoryDto>
        {
            public int Id { get; set; }

            public GetDiseaseCategoryByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateDiseaseCategoryRequest : IRequest<DiseaseCategoryDto>
        {
            public DiseaseCategoryDto DiseaseCategoryDto { get; set; }

            public CreateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto)
            {
                this.DiseaseCategoryDto = DiseaseCategoryDto;
            }
        }

        public class UpdateDiseaseCategoryRequest : IRequest<bool>
        {
            public DiseaseCategoryDto DiseaseCategoryDto { get; set; }

            public UpdateDiseaseCategoryRequest(DiseaseCategoryDto DiseaseCategoryDto)
            {
                this.DiseaseCategoryDto = DiseaseCategoryDto;
            }
        }

        public class DeleteDiseaseCategoryRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteDiseaseCategoryRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListDiseaseCategoryRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDiseaseCategoryRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}
