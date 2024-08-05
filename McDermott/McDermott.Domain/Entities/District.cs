namespace McDermott.Domain.Entities
{
    public partial class District : BaseAuditableEntity // Kecamatan
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // Kecamatan

        public Guid CityId { get; set; } // Kabupaten
        public Guid ProvinceId { get; set; }
         
        public virtual City? City { get; set; }
         
        public virtual Province? Province { get; set; }
    }
}