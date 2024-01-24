namespace McDermott.Application.Features.Commands
{
    public class ParentCategoryCommand
    {
        public class GetParentCategoryQuery : IRequest<List<ParentCategoryDto>>;

        public class GetParentCategoryByIdQuery : IRequest<ParentCategoryDto>
        {
            public int Id { get; set; }

            public GetParentCategoryByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateParentCategoryRequest : IRequest<ParentCategoryDto>
        {
            public ParentCategoryDto ParentCategoryDto { get; set; }

            public CreateParentCategoryRequest(ParentCategoryDto ParentCategoryDto)
            {
                this.ParentCategoryDto = ParentCategoryDto;
            }
        }

        public class UpdateParentCategoryRequest : IRequest<bool>
        {
            public ParentCategoryDto ParentCategoryDto { get; set; }

            public UpdateParentCategoryRequest(ParentCategoryDto ParentCategoryDto)
            {
                this.ParentCategoryDto = ParentCategoryDto;
            }
        }

        public class DeleteParentCategoryRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteParentCategoryRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListParentCategoryRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListParentCategoryRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}