namespace McDermott.Application.Features.Commands
{
    public class DistrictCommand
    {
        public class GetDistrictQuery : IRequest<List<DistrictDto>>;

        public class GetDistrictByIdQuery : IRequest<DistrictDto>
        {
            public int Id { get; set; }

            public GetDistrictByIdQuery(int id)
            {
                Id = id;
            }
        }

        public class CreateDistrictRequest : IRequest<DistrictDto>
        {
            public DistrictDto DistrictDto { get; set; }

            public CreateDistrictRequest(DistrictDto DistrictDto)
            {
                this.DistrictDto = DistrictDto;
            }
        }

        public class UpdateDistrictRequest : IRequest<bool>
        {
            public DistrictDto DistrictDto { get; set; }

            public UpdateDistrictRequest(DistrictDto DistrictDto)
            {
                this.DistrictDto = DistrictDto;
            }
        }

        public class DeleteDistrictRequest : IRequest<bool>
        {
            public int Id { get; set; }

            public DeleteDistrictRequest(int id)
            {
                Id = id;
            }
        }
        public class DeleteListDistrictRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListDistrictRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}