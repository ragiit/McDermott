using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Dtos.Medical
{
    public class BuildingLocationDto : IMapFrom<BuildingDto>
    {
        public int Id { get; set; }
        public int BuildingId { get; set; }
        public int LocationId { get; set; }

        public BuildingDto? Building { get; set; }
        public LocationDto? Location { get; set; }
    }
}