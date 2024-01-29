namespace McDermott.Application.Features.Commands
{
    public class BuildingCommand
    {
        public class GetBuildingQuery : IRequest<List<BuildingDto>>;

        public class GetBuildingByIdQuery : IRequest<BuildingDto>
        {
            public int Id { get; set; }

            public GetBuildingByIdQuery(int id)
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
            public int BuildingId { get; set; }

            public GetBuildingLocationByBuildingIdRequest(int BuildingId)
            {
                this.BuildingId = BuildingId;
            }
        }

        public class DeleteBuildingLocationByIdRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteBuildingLocationByIdRequest(List<int> Id)
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
            public int Id { get; set; }

            public DeleteBuildingRequest(int id)
            {
                Id = id;
            }
        }

        public class DeleteListBuildingRequest : IRequest<bool>
        {
            public List<int> Id { get; set; }

            public DeleteListBuildingRequest(List<int> id)
            {
                this.Id = id;
            }
        }
    }
}