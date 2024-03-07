namespace McDermott.Application.Dtos.Medical
{
    public class DoctorScheduleDto : IMapFrom<DoctorSchedule>
    {
         public long Id { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public long ServiceId { get; set; }

        public List<long>? PhysicionIds { get; set; }
        public string Physicions { get; set; } = string.Empty;

        public ServiceDto? Service { get; set; }
    }
}