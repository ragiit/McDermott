namespace McDermott.Domain.Entities
{
    public class DrugForm : BaseAuditableEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}