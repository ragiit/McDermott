using McDermott.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Commands
{
    public class CronisCategoryCommand
    {
        public class GetCronisCategoryQuery : IRequest<List<CronisCategoryDto>>;

        public class GetCronisCategoryByIdQuery : IRequest<CronisCategoryDto>
        {
            public int Id { get; set; }

            public GetCronisCategoryByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateCronisCategoryRequest : IRequest<CronisCategoryDto>
        {
            public CronisCategoryDto CronisCategoryDto { get; set; }

            public CreateCronisCategoryRequest(CronisCategoryDto CronisCategoryDto)
            {
                this.CronisCategoryDto = CronisCategoryDto;
            }
        }

        public class UpdateCronisCategoryRequest : IRequest<bool>
        {
            public CronisCategoryDto CronisCategoryDto { get; set; }

            public UpdateCronisCategoryRequest(CronisCategoryDto CronisCategoryDto)
            {
                this.CronisCategoryDto = CronisCategoryDto;
            }
        }

        public class DeleteCronisCategoryRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteCronisCategoryRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListCronisCategoryRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListCronisCategoryRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}
