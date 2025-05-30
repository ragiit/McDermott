﻿namespace McHealthCare.Domain.Entities
{
    public class InventoryAdjusment : BaseAuditableEntity
    {
        public Guid? LocationId { get; set; }
        public Guid? CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public EnumStatusInventoryAdjustment Status { get; set; }

        public virtual Location? Location { get; set; }

        public virtual Company? Company { get; set; }

        public virtual IEnumerable<InventoryAdjusmentDetail>? InventoryAdjusmentDetails { get; set; }
    }
}