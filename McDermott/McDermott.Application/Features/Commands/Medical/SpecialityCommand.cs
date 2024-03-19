namespace McDermott.Application.Features.Commands.Medical
{
    public class SpecialityCommand
    {
        public class GetSpecialityQuery : IRequest<List<SpecialityDto>>;

        public class GetSpecialityByIdQuery : IRequest<SpecialityDto>
        {
            public long Id { get; set; }

            public GetSpecialityByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateSpecialityRequest : IRequest<SpecialityDto>
        {
            public SpecialityDto SpecialityDto { get; set; }

            public CreateSpecialityRequest(SpecialityDto SpecialityDto)
            {
                this.SpecialityDto = SpecialityDto;
            }
        }

        public class UpdateSpecialityRequest : IRequest<bool>
        {
            public SpecialityDto SpecialityDto { get; set; }

            public UpdateSpecialityRequest(SpecialityDto SpecialityDto)
            {
                this.SpecialityDto = SpecialityDto;
            }
        }

        public class DeleteSpecialityRequest : IRequest<bool>
        {
            public long Id { get; set; }

            public DeleteSpecialityRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListSpecialityRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListSpecialityRequest(List<long> id)
            {
                this.Id = id;
            }
        }
    }
}