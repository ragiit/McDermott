﻿namespace McHealthCare.Application.Dtos.Transaction
{
    public class LabResultDetailDto : IMapFrom<LabResultDetail>
    {
        public Guid Id { get; set; }
        public Guid GeneralConsultanMedicalSupportId { get; set; }
        public string? Parameter { get; set; }
        public string? NormalRange { get; set; } = string.Empty;
        public Guid? LabUomId { get; set; }
        public string? Result { get; set; }
        public string? ResultType { get; set; }
        public string? ResultValueType { get; set; }
        public string? Remark { get; set; }

        [NotMapped]
        public bool IsFromDB { get; set; } = false;

        public LabUomDto? LabUom { get; set; }
        public GeneralConsultanMedicalSupportDto? GeneralConsultanMedicalSupport { get; set; }
    }
}