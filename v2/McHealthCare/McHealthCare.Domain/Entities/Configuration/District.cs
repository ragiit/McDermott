namespace McHealthCare.Domain.Entities.Configuration
{
    public class District : BaseAuditableEntity
    {
        public Guid CityId { get; set; }
        public Guid ProvinceId { get; set; }

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public virtual City? City { get; set; }
        public virtual Province? Province { get; set; }
    }
}