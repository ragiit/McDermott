namespace McDermott.Application.Features.Commands.Medical
{
    public class CronisCategoryCommand
    {
        public class GetCronisCategoryQuery : IRequest<List<CronisCategoryDto>>;

        public class GetCronisCategoryByIdQuery : IRequest<CronisCategoryDto>
        {
             public long Id { get; set; }

            public GetCronisCategoryByIdQuery(long id)
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
             public long Id { get; set; }

            public DeleteCronisCategoryRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListCronisCategoryRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListCronisCategoryRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}