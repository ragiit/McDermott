namespace McDermott.Application.Features.Commands.Config
{
    public class GenderCommand
    {
        public class GetGenderQuery : IRequest<List<GenderDto>>;

        public class GetGenderByIdQuery : IRequest<GenderDto>
        {
             public long Id { get; set; }

            public GetGenderByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateGenderRequest : IRequest<GenderDto>
        {
            public GenderDto GenderDto { get; set; }

            public CreateGenderRequest(GenderDto GenderDto)
            {
                this.GenderDto = GenderDto;
            }
        }

        public class UpdateGenderRequest : IRequest<bool>
        {
            public GenderDto GenderDto { get; set; }

            public UpdateGenderRequest(GenderDto GenderDto)
            {
                this.GenderDto = GenderDto;
            }
        }

        public class DeleteGenderRequest : IRequest<bool>
        {
             public long Id { get; set; }

            public DeleteGenderRequest(long id)
            {
                Id = id;
            }
        }
    }
}