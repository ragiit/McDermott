using Microsoft.AspNetCore.Components.Forms;

namespace McDermott.Application.Interfaces.Repositories
{
    public interface IUploadDocumentService
    {
        Task<(int, string)> UploadDocumentAsync(IBrowserFile file, int? maxFileSize = 0, string[] allowedExtensions = null);

        Task<string?> DownloadDocument(string fileName);
    }
}