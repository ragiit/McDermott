﻿namespace McDermott.Application.Dtos.Employee
{
    public class SickLeaveDto : IMapFrom<SickLeave>
    {
        public long Id { get; set; }
        public long? GeneralConsultansId { get; set; }
        public long? PatientId { get; set; }
        public long? PhycisianId { get; set; }
        public long? NIP { get; set; }
        public long? Legacy { get; set; }
        public long? SAP { get; set; }
        public long? Oracel { get; set; }
        public string? PatientName { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? SIP { get; set; }
        public string? PhycisianName { get; set; }
        public string? YesOrNoEmployee { get; set; } = "No";
        public bool? isEmployee { get; set; } = false;
        public DateTime? brithday { get; set; }
        public string? NoRM { get; set; }
        public EnumStatusSickLeave Status { get; set; }
        public string? TypeLeave { get; set; }
        public string? Diagnosis { get; set; }
        public DateTime? StartSickLeave { get; set; }
        public DateTime? EndSickLeave { get; set; }

        public virtual GeneralConsultanServiceDto? GeneralConsultans { get; set; }
        public virtual UserDto? Patient { get; set; }
    }
}