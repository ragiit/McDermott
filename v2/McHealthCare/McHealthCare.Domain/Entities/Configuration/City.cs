﻿namespace McHealthCare.Domain.Entities.Configuration

{
    public partial class City : BaseAuditableEntity
    {
        public Guid ProvinceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        public virtual Province? Province { get; set; }

        public virtual List<Village>? Villages { get; set; }
    }
}