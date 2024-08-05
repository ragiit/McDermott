namespace McHealthCare.Application.Interfaces
{
    public interface IDocumentProvider
    {
        Task<byte[]> GetDocumentAsync(string name, Dictionary<string, string> mergeFields, CancellationToken cancellationToken = default(CancellationToken));
    }
}