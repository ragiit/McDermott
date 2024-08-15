namespace McHealthCare.Domain.Entities.Products
{
    public class DrugForm : BaseAuditableEntity
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}