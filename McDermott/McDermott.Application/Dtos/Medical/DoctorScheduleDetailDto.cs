﻿using McDermott.Domain.Common;
using System.Globalization;

namespace McDermott.Application.Dtos.Medical
{
    public class DoctorScheduleDetailDto : IMapFrom<DoctorScheduleDetail>
    {
        public long Id { get; set; }

        public long DoctorScheduleId { get; set; }

        [Required]
        public long? ServiceId { get; set; }

        [Required]
        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        [Required]
        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        public string WorkFromFormatString
        { get { return WorkFrom.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public string WorkToFormatString
        { get { return WorkTo.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public long Quota { get; set; } = 0;
        public virtual Service? Service { get; set; }

        // Deprecated
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool UpdateToBpjs { get; set; } = false;

        [SetToNull]
        public virtual DoctorSchedule? DoctorSchedule { get; set; }

        //public long DoctorScheduleId { get; set; }

        //[Required]
        //[StringLength(200)]
        //public string Name { get; set; } = string.Empty;

        //[Required]
        //[StringLength(200)]
        //public string DayOfWeek { get; set; } = string.Empty;

        //public TimeSpan WorkFrom { get; set; } = new TimeSpan(8, 0, 0);

        //public TimeSpan WorkTo { get; set; } = new TimeSpan(12, 0, 0);

        ////public string WorkFromFormatString
        ////{ get { return WorkFrom.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        ////public string WorkToFormatString
        ////{ get { return WorkTo.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        //public string WorkFromFormatString
        //{ get { return WorkFrom.ToString(@"hh\:mm"); } }

        //public string WorkToFormatString
        //{ get { return WorkTo.ToString(@"hh\:mm"); } }

        //public long Quota { get; set; } = 0;
        //public bool UpdateToBpjs { get; set; } = false;

        //public DoctorScheduleDto? DoctorSchedule { get; set; }
    }

    public class CreateUpdateDoctorScheduleDetailDto
    {
        public long Id { get; set; }

        public long DoctorScheduleId { get; set; }
        public long ServiceId { get; set; }

        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan WorkFrom { get; set; }

        public TimeSpan WorkTo { get; set; }

        public string WorkFromFormatString
        { get { return WorkFrom.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public string WorkToFormatString
        { get { return WorkTo.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public long Quota { get; set; } = 0;

        // Deprecated
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public bool UpdateToBpjs { get; set; } = false;
    }
}