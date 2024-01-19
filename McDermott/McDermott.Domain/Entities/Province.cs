namespace McDermott.Domain.Entities
{
    public partial class Province : BaseAuditableEntity
    {
        public int CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string Code { get; set; } = string.Empty; // State Code

        public Country? Country { get; set; }

        public virtual List<City>? Cities { get; set; }
        public virtual List<District>? Districts { get; set; }
        public virtual List<Company>? Companies { get; set; }
    }
}