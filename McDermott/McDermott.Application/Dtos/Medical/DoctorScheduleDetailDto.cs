namespace McDermott.Application.Dtos.Medical
{
    public class DoctorScheduleDetailDto : IMapFrom<DoctorScheduleDetail>
    {
        public int Id { get; set; }
        public int DoctorScheduleId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DayOfWeek { get; set; } = string.Empty;

        public TimeSpan WorkFrom { get; set; } = DateTime.Now.TimeOfDay;

        public TimeSpan WorkTo { get; set; } = DateTime.Now.TimeOfDay;

        //public string WorkFromFormatString
        //{ get { return WorkFrom.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        //public string WorkToFormatString
        //{ get { return WorkTo.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public string WorkFromFormatString
        { get { return WorkFrom.ToString(@"hh\:mm"); } }

        public string WorkToFormatString
        { get { return WorkTo.ToString(@"hh\:mm"); } }

        public int Quota { get; set; } = 0;
        public bool UpdateToBpjs { get; set; } = false;

        public DoctorScheduleDto? DoctorSchedule { get; set; }
    }
}