namespace McDermott.Domain.Entities
{
    public partial class Province : BaseAuditableEntity
    {
        public long CountryId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty; // State Name

        [StringLength(5)]
        public string? Code { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public Country? Country { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual List<City>? Cities { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual List<District>? Districts { get; set; }

        [SetToNull] // Tandai properti yang harus diatur ke null
        public virtual List<Company>? Companies { get; set; }
    }
}