using McDermott.Application.Dtos.Config;

namespace McDermott.Application.Features.Commands.Config
{
    public class OccupationalCommand
    {
        public class GetOccupationalQuery : IRequest<List<OccupationalDto>>;

        public class GetOccupationalByIdQuery : IRequest<OccupationalDto>
        {
            public int Id { get; set; }

            public GetOccupationalByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateOccupationalRequest : IRequest<OccupationalDto>
        {
            public OccupationalDto OccupationalDto { get; set; }

            public CreateOccupationalRequest(OccupationalDto OccupationalDto)
            {
                this.OccupationalDto = OccupationalDto;
            }
        }

        public class UpdateOccupationalRequest : IRequest<bool>
        {
            public OccupationalDto OccupationalDto { get; set; }

            public UpdateOccupationalRequest(OccupationalDto OccupationalDto)
            {
                this.OccupationalDto = OccupationalDto;
            }
        }

        public class DeleteOccupationalRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteOccupationalRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListOccupationalRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListOccupationalRequest(List<int> id)
            {
                Id = id;
            }
        }
    }
}