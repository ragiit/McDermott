namespace McDermott.Application.Dtos.Pharmacies
{
    public class DrugFormDto : IMapFrom<DrugForm>
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }
}