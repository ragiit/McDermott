using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Interfaces.Repositories
{
    public interface IUploadDocumentService
    {
        Task<(int, string)> UploadDocumentAsync(IBrowserFile file, int? maxFileSize = 0, string[] allowedExtensions = null);
        Task<string?> DownloadDocument(string fileName);
    }
}
