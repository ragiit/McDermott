using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Services
{
    public class DocumentProvider : IDocumentProvider
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentProvider(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public Task<byte[]> GetDocumentAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Tentukan jalur lengkap ke file dengan menggunakan AppContext.BaseDirectory
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Surat", name);

            // Baca file secara asynchronous dan kembalikan byte array
            return File.ReadAllBytesAsync(filePath, cancellationToken);
        }
    }
}