namespace McDermott.Application.Dtos.Queue
{
    public class KioskDepartementDto : IMapFrom<KioskDepartement>
    {
         public long Id { get; set; }
        public long? ServiceKId { get; set; }
        public long? ServicePId { get; set; }

        public virtual ServiceDto? ServiceK { get; set; }
        public virtual ServiceDto? ServiceP { get; set; }
    }
}