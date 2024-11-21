namespace McDermott.Application.Dtos.Transaction
{
    public class WellnessProgramAttendanceDto : IMapFrom<WellnessProgramAttendance>
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public long WellnessProgramDetailId { get; set; }
        public long PatientId { get; set; }
        public DateTime Date { get; set; }

        public User? Patient { get; set; }
        public WellnessProgramDto? WellnessProgram { get; set; }
        public WellnessProgramDetailDto? WellnessProgramDetail { get; set; }
    }

    public class CreateUpdateWellnessProgramAttendanceDto
    {
        public long Id { get; set; }
        public long WellnessProgramId { get; set; }
        public long WellnessProgramDetailId { get; set; }
        public long PatientId { get; set; }
        public DateTime Date { get; set; }
    }
}