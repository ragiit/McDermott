namespace McDermott.Application.Dtos.Pharmacies
{
    public class SignaDto : IMapFrom<Signa>
    {
        public long Id { get; set; }
        public string? Name { get; set; }
    }
}