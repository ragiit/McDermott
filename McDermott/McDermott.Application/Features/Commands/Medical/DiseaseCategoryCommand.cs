namespace McDermott.Application.Features.Commands.Medical
{
    public class DiseaseCategoryCommand
    {
        public class GetDiseaseCategoryQuery : IRequest<List<DiseaseCategoryDto>>;

        public class GetDiseaseCategoryByIdQuery : IRequest<DiseaseCategoryDto>
        {
            public long Id { get; set; }

            public GetDiseaseCategoryByIdQuery(long id)
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
            public long Id { get; set; }

            public DeleteDiseaseCategoryRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListDiseaseCategoryRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListDiseaseCategoryRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}