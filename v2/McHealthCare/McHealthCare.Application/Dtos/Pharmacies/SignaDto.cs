namespace McHealthCare.Application.Dtos.Pharmacies
{
    public class SignaDto : IMapFrom<Signa>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }
}