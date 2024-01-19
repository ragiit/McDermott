namespace McDermott.Domain.Entities
{
    public partial class Village : BaseAuditableEntity // Kelurahan
    {
        public int ProvinceId { get; set; }
        public int CityId { get; set; } // Kabupaten

        public int DistrictId { get; set; } // Kecamatan

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kelurahan

        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty; // Kode Pos

        public virtual Province? Province { get; set; }
        public virtual City? City { get; set; }
        public virtual District? District { get; set; }
    }
}