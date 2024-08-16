namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class DrugFormDto : IMapFrom<DrugForm>
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}