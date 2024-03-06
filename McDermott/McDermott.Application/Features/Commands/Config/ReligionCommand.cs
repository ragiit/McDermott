namespace McDermott.Application.Features.Commands.Config
{
    public class ReligionCommand
    {
        public class GetReligionQuery : IRequest<List<ReligionDto>>;

        public class GetReligionByIdQuery : IRequest<ReligionDto>
        {
             public long Id { get; set; }

            public GetReligionByIdQuery(long id)
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
             public long Id { get; set; }

            public DeleteReligionRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListReligionRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListReligionRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}