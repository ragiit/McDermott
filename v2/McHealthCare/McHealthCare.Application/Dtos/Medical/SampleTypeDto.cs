using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class SampleTypeDto : IMapFrom<SampleType>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual List<LabTest>? LabTests { get; set; }
    }

    public class CreateUpdateSampleTypeDto : IMapFrom<SampleType>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public virtual List<LabTest>? LabTests { get; set; }
    }
}