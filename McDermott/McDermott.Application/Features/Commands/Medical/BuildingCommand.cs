namespace McDermott.Application.Features.Commands.Medical
{
    public class BuildingCommand
    {
        public class GetBuildingQuery : IRequest<List<BuildingDto>>;

        public class GetBuildingByIdQuery : IRequest<BuildingDto>
        {
            public long Id { get; set; }

            public GetBuildingByIdQuery(long id)
            {
                Id = id;
            }
        }

        public class CreateBuildingRequest : IRequest<BuildingDto>
        {
            public BuildingDto BuildingDto { get; set; }

            public CreateBuildingRequest(BuildingDto BuildingDto)
            {
                this.BuildingDto = BuildingDto;
            }
        }

        public class GetBuildingLocationByBuildingIdRequest : IRequest<List<BuildingLocationDto>>
        {
            public long BuildingId { get; set; }

            public GetBuildingLocationByBuildingIdRequest(long BuildingId)
            {
                this.BuildingId = BuildingId;
            }
        }

        public class DeleteBuildingLocationByIdRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteBuildingLocationByIdRequest(List<long> Id)
            {
                this.Id = Id;
            }
        }

        public class CreateBuildingLocationRequest : IRequest<bool>
        {
            public List<BuildingLocationDto> BuildingLocationDtos { get; set; }

            public CreateBuildingLocationRequest(List<BuildingLocationDto> BuildingLocationDtos)
            {
                this.BuildingLocationDtos = BuildingLocationDtos;
            }
        }

        public class UpdateBuildingRequest : IRequest<bool>
        {
            public BuildingDto BuildingDto { get; set; }

            public UpdateBuildingRequest(BuildingDto BuildingDto)
            {
                this.BuildingDto = BuildingDto;
            }
        }

        public class DeleteBuildingRequest : IRequest<bool>
        {
            public long Id { get; set; }

            public DeleteBuildingRequest(long id)
            {
                Id = id;
            }
        }

        public class DeleteListBuildingRequest : IRequest<bool>
        {
            public List<long> Id { get; set; }

            public DeleteListBuildingRequest(List<long> id)
            {
                Id = id;
            }
        }
    }
}