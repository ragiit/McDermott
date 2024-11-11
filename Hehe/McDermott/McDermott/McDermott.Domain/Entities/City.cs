namespace McDermott.Domain.Entities
{
    public partial class City : BaseAuditableEntity // Kabupaten
    {
        public long ProvinceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        [SetToNull]
        public virtual Province? Province { get; set; }

        [SetToNull]
        public virtual List<District>? Districts { get; set; }

        [SetToNull]
        public virtual List<Company>? Companies { get; set; }

        [SetToNull]
        public virtual List<Village>? Villages { get; set; }

        [SetToNull]
        public virtual List<HealthCenter>? HealthCenters { get; set; }
    }
}