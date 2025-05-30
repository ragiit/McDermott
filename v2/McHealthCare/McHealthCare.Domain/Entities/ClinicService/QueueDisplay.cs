﻿namespace McHealthCare.Domain.Entities.ClinicService
{
    public class QueueDisplay : BaseAuditableEntity
    {
        public string? Name { get; set; }

        public List<Guid>? CounterIds { get; set; }

        public virtual List<Counter>? Counter { get; set; }
    }
}