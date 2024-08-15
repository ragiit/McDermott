namespace McHealthCare.Application.Dtos.Transaction
{
    public class GeneralConsultanlogDto : IMapFrom<GeneralConsultationLog>
    {
        public Guid? GeneralConsultanServiceId { get; set; }
        public Guid? ProcedureRoomId { get; set; }
        public string? UserById { get; set; }
        public string? Status { get; set; }

        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
        public GeneralConsultanMedicalSupportDto? ProcedureRoom { get; set; }
        public ApplicationUserDto? UserBy { get; set; }
    }
}