namespace McDermott.Application.Features.Commands.Medical
{
    public class SpecialityCommand
    {
        public class GetSpecialityQuery : IRequest<List<SpecialityDto>>;

        public class GetSpecialityByIdQuery : IRequest<SpecialityDto>
        {
            public int Id { get; set; }

            public GetSpecialityByIdQuery(int id)
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
            public int Id { get; set; }

            public DeleteSpecialityRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListSpecialityRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListSpecialityRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}