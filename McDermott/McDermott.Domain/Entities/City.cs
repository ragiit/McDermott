namespace McDermott.Domain.Entities
{
    public partial class City : BaseAuditableEntity // Kabupaten
    {
        public int ProvinceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kabupaten/Kota

        public virtual Province? Province { get; set; }

        public virtual List<District>? Districts { get; set; }

        public virtual List<Company>? Companies { get; set; }
    }
}