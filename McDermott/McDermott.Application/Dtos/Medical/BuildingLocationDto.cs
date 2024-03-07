namespace McDermott.Application.Dtos.Medical
{
    public class BuildingLocationDto : IMapFrom<BuildingDto>
    {
         public long Id { get; set; }
        public long BuildingId { get; set; }
        public long LocationId { get; set; }

        public BuildingDto? Building { get; set; }
        public LocationDto? Location { get; set; }
    }
}