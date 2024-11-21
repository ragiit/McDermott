using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;

namespace McDermott.Application.Features.Services
{
    public class UploadDocumentService : IUploadDocumentService
    {
        private readonly IWebHostEnvironment _environment;

        public UploadDocumentService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        //Download Document

        public async Task<string?> DownloadDocument(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_environment.WebRootPath, "files", fileName);
                return filePath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(int, string)> UploadDocumentAsync(IBrowserFile file, int? maxFileSize = 0, string[] allowedExtensions = null)
        {
            if (file == null)
                return (0, "Null File");

            var uploadDirectory = Path.Combine(_environment.WebRootPath, "files");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            if (maxFileSize > 0 && file.Size > maxFileSize)
            {
                return (0, $"File: {file.Name} exceeds the maximum allowed file size.");
            }

            var fileExtension = Path.GetExtension(file.Name);
            if (allowedExtensions is not null && allowedExtensions.Length > 0 && !allowedExtensions.Contains(fileExtension))
            {
                return (0, $"File: {file.Name}, File type not allowed");
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}"; // Menggunakan nama file unik
            var path = Path.Combine(uploadDirectory, fileName);
            await using var fs = new FileStream(path, FileMode.Create);
            await file.OpenReadStream(maxFileSize ?? file.Size).CopyToAsync(fs);

            return (1, fileName);
        }
    }
}