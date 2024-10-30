namespace McDermott.Application.Dtos.Transaction
{
    public class GeneralCosultanServiceAncDto : IMapFrom<GeneralCosultanServiceAnc>
    {
        public long PatientId { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public DateTime Date { get; set; }

        public UserDto? User { get; set; }
        public GeneralConsultanServiceDto? GeneralConsultanService { get; set; }
    }

    public class CreateUpdateGeneralCosultanServiceAncDto  
    {
        public long Id { get; set; }    
        public long PatientId { get; set; }
        public long GeneralConsultanServiceId { get; set; }
        public DateTime Date { get; set; } 
    }
}
