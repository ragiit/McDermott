using System.ComponentModel.DataAnnotations;
using Mapster;
using McHealthCare.Domain.Common;
using McHealthCare.Domain.Entities.Medical;

namespace McHealthCare.Application.Dtos.Medical
{
    public class LabTestDto : IMapFrom<LabTest>
    {
        public Guid? SampleTypeId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(5)]
        public string? Code { get; set; }

        public string? ResultType { get; set; }

        [SetToNull]
        public virtual SampleTypeDto? SampleType { get; set; }
        [SetToNull]
        public virtual List<LabTestDetailDto>? LabTestDetail { get; set; }
        // [SetToNull]
        // public virtual List<GeneralConsultanMedicalSupportDto>? GeneralConsultanMedicalSupports { get; set; }
    }

    public class CreateUpdateLabTestDto : IMapFrom<LabTest>{
        public Guid? SampleTypeId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }
        [StringLength(5)]
        public string? Code { get; set; }

        public string? ResultType { get; set; }

        [SetToNull]
        public virtual SampleTypeDto? SampleType { get; set; }
        [SetToNull]
        public virtual List<LabTestDetailDto>? LabTestDetail { get; set; }
        // [SetToNull]
        // public virtual List<GeneralConsultanMedicalSupportDto>? GeneralConsultanMedicalSupports { get; set; }
    }
}