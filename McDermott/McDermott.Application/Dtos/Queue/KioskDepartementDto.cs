namespace McDermott.Application.Dtos.Queue
{
    public class KioskDepartementDto : IMapFrom<KioskDepartement>
    {
        public int Id { get; set; }
        public int? ServiceKId { get; set; }
        public int? ServicePId { get; set; }

        public virtual ServiceDto? ServiceK { get; set; }
        public virtual ServiceDto? ServiceP { get; set; }
    }
}