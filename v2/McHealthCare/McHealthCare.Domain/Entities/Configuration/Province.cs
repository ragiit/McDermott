﻿namespace McHealthCare.Domain.Entities.Configuration
{
    public class Province : BaseAuditableEntity
    {
        public Guid CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(5)]
        public string Code { get; set; } = string.Empty;

        public virtual Country? Country { get; set; }
    }
}