using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class LabUomDto : IMapFrom<LabUom>
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }

    public class CreateUpdateLabUom : IMapFrom<LabUom>
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}