using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Diagnostics;

namespace McDermott.Application.Features.Services
{
    public class DocumentProvider : IDocumentProvider
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentProvider(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<byte[]> GetDocumentAsync(string name, Dictionary<string, string> mergeFields, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Tentukan jalur lengkap ke file dengan menggunakan AppContext.BaseDirectory
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Surat", name);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            // Baca file secara asynchronous dan kembalikan byte array
            byte[] templateBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);

            using (MemoryStream memoryStream = new MemoryStream(templateBytes))
            {
                using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, true))
                {
                    var body = doc.MainDocumentPart.Document.Body;
                    foreach (var field in mergeFields)
                    {
                        foreach (var text in body.Descendants<Text>().Where(t => t.Text.Contains(field.Key)))
                        {
                            text.Text = text.Text.Replace(field.Key, field.Value);
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}