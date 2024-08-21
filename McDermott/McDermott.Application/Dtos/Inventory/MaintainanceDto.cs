using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class MaintainanceDto : IMapFrom<Maintainance>
    {
        public long Id { get; set; }
        public long? RequestById { get; set; }
        public string? Title { get; set; }
        public string? Sequence { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public long? ResponsibleById { get; set; }
        public long? EquipmentId { get; set; }
        public string? SerialNumber { get; set; }
        public bool isCorrective { get; set; } = false;
        public bool isPreventive { get; set; } = false;
        public bool isInternal { get; set; } = false;
        public bool isExternal { get; set; } = false;
        public string? VendorBy { get; set; }
        public bool Recurrent { get; set; } = false;
        public int? RepeatNumber { get; set; }
        public EnumWorksDays? RepeatWork { get; set; }
        public EnumStatusMaintainance? Status { get; set; }
        public string? Note { get; set; }

        [SetToNull]
        public virtual UserDto? RequestBy { get; set; }

        [SetToNull]
        public virtual UserDto? ResponsibleBy { get; set; }

        [SetToNull]
        public virtual ProductDto? Equipment { get; set; }
    }
}