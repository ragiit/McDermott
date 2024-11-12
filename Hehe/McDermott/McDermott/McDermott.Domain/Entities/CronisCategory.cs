namespace McDermott.Domain.Entities
{
    public partial class CronisCategory : BaseAuditableEntity
    {
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }
    }
}