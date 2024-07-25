namespace McHealthCare.Domain.Entities
{
    public partial class Religion : BaseAuditableEntity // Agama
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
    }
}