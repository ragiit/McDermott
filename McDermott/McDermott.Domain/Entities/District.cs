namespace McDermott.Domain.Entities
{
    public partial class District : BaseAuditableEntity // Kecamatan
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kecamatan

        public long CityId { get; set; } // Kabupaten
        public long ProvinceId { get; set; }

        [SetToNull]
        public virtual City? City { get; set; }

        [SetToNull]
        public virtual Province? Province { get; set; }
    }
}