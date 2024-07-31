using System.ComponentModel.DataAnnotations;
using Mapster;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class SampleTypeDto : IMapFrom<SampleType>
    {
        [StringLength(200)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual List<LabTest>? LabTests { get; set; }
    }

    public class CreateUpdateSampleTypeDto : IMapFrom<SampleType>
    {
        [StringLength(200)]
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual List<LabTest>? LabTests { get; set; }
    }
}