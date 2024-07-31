using System.ComponentModel.DataAnnotations;
using Mapster;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class LabTestDetailDto : IMapFrom<LabTestDetail>
    {
        public Guid? LabTestId { get; set; }
        public Guid? LabUomId { get; set; }
        public string? Name { get; set; }
        public string? ResultType { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRangeMale { get; set; }
        public string? NormalRangeFemale { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [SetToNull]
        public LabTestDto? LabTest { get; set; }
        [SetToNull]
        public LabUomDto? LabUom { get; set; }
    }

    public class CreateUpdateLabTestDetailDto : IMapFrom<LabTestDetail>{
        public Guid? LabTestId { get; set; }
        public Guid? LabUomId { get; set; }
        public string? Name { get; set; }
        public string? ResultType { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRangeMale { get; set; }
        public string? NormalRangeFemale { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [SetToNull]
        public virtual LabTestDto? LabTest { get; set; }
        [SetToNull]
        public virtual LabUomDto? LabUom { get; set; }
    }
}