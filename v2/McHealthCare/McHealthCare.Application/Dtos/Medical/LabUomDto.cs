using Mapster;
using McHealthCare.Domain.Entities.Medical;
using System.ComponentModel.DataAnnotations;

namespace McHealthCare.Application.Dtos.Medical
{
    public class LabUomDto : IMapFrom<LabUom>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }

    public class CreateUpdateLabUomDto : IMapFrom<LabUom>
    {
        public Guid Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;
    }
}