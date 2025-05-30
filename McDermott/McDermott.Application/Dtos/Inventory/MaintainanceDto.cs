﻿using McDermott.Domain.Common;

namespace McDermott.Application.Dtos.Inventory
{
    public class MaintenanceDto : IMapFrom<Maintenance>
    {
        public long Id { get; set; }
        public long? RequestById { get; set; }
        public long? LocationId { get; set; }
        public string? Title { get; set; }
        public string? Sequence { get; set; }
        public string? RequestName { get; set; }
        public string? ResponsibleName { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime ScheduleDate { get; set; } = DateTime.Now;
        public long? ResponsibleById { get; set; }
        public string? EquipmentName { get; set; }
        public bool isCorrective { get; set; } = false;
        public bool isPreventive { get; set; } = false;
        public bool isInternal { get; set; } = false;
        public bool isExternal { get; set; } = false;
        public string? VendorBy { get; set; }
        public bool Recurrent { get; set; } = false;
        public int? RepeatNumber { get; set; }
        public string? RepeatWork { get; set; }
        public EnumStatusMaintenance? Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [SetToNull]
        public virtual UserDto? RequestBy { get; set; }

        [SetToNull]
        public virtual UserDto? ResponsibleBy { get; set; }

        [SetToNull]
        public virtual LocationDto? Location { get; set; }
    }
}