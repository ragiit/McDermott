using System.Globalization;

namespace McDermott.Application.Dtos.Medical
{
    public class DoctorScheduleSlotDto : IMapFrom<DoctorScheduleSlot>
    {
        public long Id { get; set; }
        public long DoctorScheduleId { get; set; }
        public long? PhysicianId { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan WorkFrom { get; set; } = DateTime.Now.TimeOfDay;

        public TimeSpan WorkTo { get; set; } = DateTime.Now.TimeOfDay;

        public string WorkFromFormatString
        { get { return WorkFrom.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public string WorkToFormatString
        { get { return WorkTo.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public DoctorScheduleDto? DoctorSchedule { get; set; }
        public UserDto? Physician { get; set; }
    }
}