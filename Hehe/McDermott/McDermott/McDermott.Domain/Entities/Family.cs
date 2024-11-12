namespace McDermott.Domain.Entities
{
    public partial class Family : BaseAuditableEntity
    {
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public long? InverseRelationId { get; set; }
        public Family? InverseRelation { get; set; }
    }
}