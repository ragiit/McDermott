﻿namespace McHealthCare.Application.Dtos.Transaction
{
    public class GeneralConsultanCPPTDto : IMapFrom<GeneralConsultanCPPT>
    {
        public Guid Id { get; set; }
        public Guid GeneralConsultanServiceId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public virtual GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }
}