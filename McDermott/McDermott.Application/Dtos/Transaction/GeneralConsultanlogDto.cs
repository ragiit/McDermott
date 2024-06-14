

namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralConsultanlogDto: IMapFrom<GeneralConsultationLog>
    {
        public long? GeneralConsultanServiceId { get; set; }
        public long? ProcedureRoomId { get; set; }
        public long? UserById { get; set; }
        public string? Status { get; set; }

        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public GeneralConsultanMedicalSupportDto? ProcedureRoom { get; set; }
        public UserDto? UserBy { get; set; }
    }
}
