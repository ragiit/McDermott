namespace McDermott.Domain.Entities
{
    public partial class Village : BaseAuditableEntity // Kelurahan
    {
        [Required]
        public long? ProvinceId { get; set; }

        [Required]
        public long? CityId { get; set; } // Kabupaten

        [Required]
        public long? DistrictId { get; set; } // Kecamatan

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan

        [StringLength(10)]
        public string? PostalCode { get; set; }

        [SetToNull]
        public virtual Province? Province { get; set; }

        [SetToNull]
        public virtual City? City { get; set; }

        [SetToNull]
        public virtual District? District { get; set; }
    }
}