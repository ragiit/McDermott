namespace McDermott.Application.Features.Commands
{
    public class ReligionCommand
    {
        public class GetReligionQuery : IRequest<List<ReligionDto>>;

        public class GetReligionByIdQuery : IRequest<ReligionDto>
        {
            public int Id { get; set; }

            public GetReligionByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateReligionRequest : IRequest<ReligionDto>
        {
            public ReligionDto ReligionDto { get; set; }

            public CreateReligionRequest(ReligionDto ReligionDto)
            {
                this.ReligionDto = ReligionDto;
            }
        }

        public class UpdateReligionRequest : IRequest<bool>
        {
            public ReligionDto ReligionDto { get; set; }

            public UpdateReligionRequest(ReligionDto ReligionDto)
            {
                this.ReligionDto = ReligionDto;
            }
        }

        public class DeleteReligionRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteReligionRequest(int id)
            {
                Id = id;
            }
        }
    }
}