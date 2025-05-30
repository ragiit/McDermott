﻿namespace McDermott.Application.Dtos.Queue
{
    public class KioskQueueDto : IMapFrom<KioskQueue>
    {
        public long Id { get; set; }
        public long? KioskId { get; set; }
        public long? ServiceId { get; set; }
        public long? ServiceKId { get; set; }
        public long? QueueNumber { get; set; }

        public string? QueueNumberString => QueueNumber.HasValue
            ? QueueNumber.Value.ToString().PadLeft(3, '0')
            : null;

        public DateTime? CreatedDate { get; set; }
        public string? Queues { get; set; }
        public string? QueueStage { get; set; }
        public string? QueueStatus { get; set; }
        public long? ClassTypeId { get; set; }
        public string? NameClass { get; set; } = string.Empty;

        public virtual KioskDto? Kiosk { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual ServiceDto? ServiceK { get; set; }
        public virtual ClassTypeDto? ClassType { get; set; }
    }
}