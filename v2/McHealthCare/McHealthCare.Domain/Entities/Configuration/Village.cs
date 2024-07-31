namespace McHealthCare.Domain.Entities.Configuration
{
    public partial class Village : BaseAuditableEntity // Kelurahan
    {
        public Guid ProvinceId { get; set; }
        public Guid CityId { get; set; } // Kabupaten

        public Guid DistrictId { get; set; } // Kecamatan

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan

        [StringLength(10)]
        public string? PostalCode { get; set; }

        public virtual Province? Province { get; set; }

        public virtual City? City { get; set; }
    }
}