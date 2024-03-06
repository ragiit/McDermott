namespace McDermott.Application.Features.Commands.Config
{
    public class DistrictCommand
    {
        public class GetDistrictQuery : IRequest<List<DistrictDto>>;

        public class GetDistrictByIdQuery : IRequest<DistrictDto>
        {
             public long Id { get; set; }

            public GetDistrictByIdQuery(long id)
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
             public long Id { get; set; }

            public DeleteDistrictRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListDistrictRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListDistrictRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}