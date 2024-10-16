using FluentValidation;
using System.Globalization;

namespace McDermott.Application.Dtos.Medical
{
    public class DoctorScheduleSlotDto : IMapFrom<DoctorScheduleSlot>
    {
        public long Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required, NotMapped]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(7);

        public TimeSpan WorkFrom { get; set; } = DateTime.Now.TimeOfDay;
        public TimeSpan WorkTo { get; set; } = DateTime.Now.TimeOfDay;
        public long Quota { get; set; } = 0;
        public long ServiceId { get; set; }

        [Required]
        public long PhysicianId { get; set; }

        public string WorkFromFormatString
        { get { return WorkFrom.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        public string WorkToFormatString
        { get { return WorkTo.ToString(string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator) ? @"HH\:mm" : @"hh\:mm"); } }

        // Deprecated
        public long? DoctorScheduleId { get; set; }

        public string ResultWorkFormatStringKiosk
        {
            get
            {
                return $"{WorkFromFormatString}-{WorkToFormatString}";
            }
        }

        public DoctorScheduleDto? DoctorSchedule { get; set; }
        public UserDto? Physician { get; set; }
        public ServiceDto? Service { get; set; }
    }

    public class CreateUpdateDoctorScheduleSlotDto
    {
        public long Id { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required, NotMapped]
        public DateTime EndDate { get; set; }

        public TimeSpan WorkFrom { get; set; } = DateTime.Now.TimeOfDay;
        public TimeSpan WorkTo { get; set; } = DateTime.Now.TimeOfDay;
        public long Quota { get; set; } = 0;
        public long ServiceId { get; set; }

        [Required]
        public long PhysicianId { get; set; }
    }

    public class CreateUpdateDoctorScheduleSlotValidator : AbstractValidator<DoctorScheduleSlotDto>
    {
        public CreateUpdateDoctorScheduleSlotValidator()
        {
            RuleFor(x => x.PhysicianId).NotEmpty().WithMessage("Physician is required");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Physician is required");
            RuleFor(x => x.EndDate).NotEmpty().WithMessage("Physician is required");

            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("End date cannot be in the past");
            RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Start date cannot be in the past");
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date cannot be later than end date");
        }
    }
}