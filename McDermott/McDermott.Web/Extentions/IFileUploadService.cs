using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Web.Extentions
{
    public interface IFileUploadService
    {
        Task<(int, string)> UploadFileAsync(IBrowserFile file, int? maxFileSize = 0, string[] allowedExtensions = null);
        Task<string> DownloadFile(string fileName);
    }
}
