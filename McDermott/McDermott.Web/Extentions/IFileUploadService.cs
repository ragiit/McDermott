using Microsoft.AspNetCore.Components.Forms;

namespace McDermott.Web.Extentions
{
    public interface IFileUploadService
    {
        Task<(int, string)> UploadFileAsync(IBrowserFile file, int? maxFileSize = 0, string[] allowedExtensions = null);

        Task<string> DownloadFile(string fileName);
    }
}