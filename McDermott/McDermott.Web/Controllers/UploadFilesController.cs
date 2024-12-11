using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Linq;

namespace McDermott.Web.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/upload-file")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        [HttpPost("UploadMultipleFiles")]
        public async Task<IActionResult> UploadMultipleFiles(
            [FromForm] List<IFormFile> files,
            [FromForm] MaintenanceProduct Data)
        {
            if (Data.ProductId <= 0 || Data.MaintenanceId <= 0)
                return BadRequest("Invalid parameters.");

            if (files == null || !files.Any())
                return BadRequest("No files uploaded.");

            var allowedExtensions = new List<string> { ".pdf", ".docx" };
            foreach (var file in files)
            {
                var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                    return BadRequest($"File type {fileExtension} is not allowed.");

                if (file.Length > 15_000_000) // 15 MB
                    return BadRequest("File size exceeds the maximum allowed size.");

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();

                    var base64Compressed = await CompressBase64Async(Convert.ToBase64String(fileBytes));
                    SaveToDatabase(file.FileName, base64Compressed, Data);
                }
            }

            return Ok("Files uploaded successfully.");
        }

        private async Task<string> CompressBase64Async(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            using (var memoryStream = new MemoryStream())
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                await gzipStream.WriteAsync(bytes, 0, bytes.Length);
                await gzipStream.FlushAsync();
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private void SaveToDatabase(string fileName, string base64Data, MaintenanceProduct Data)
        {
            // Simulasi penyimpanan ke database
            Console.WriteLine($"File {fileName} saved with data length: {base64Data.Length}");
            Console.WriteLine($"ProductId: {Data.ProductId}, MaintenanceId: {Data.MaintenanceId}");
        }

        [HttpPost("upload")]
        public ActionResult Upload(IFormFile myFile)
        {
            try
            {
                Console.WriteLine($"Uploading {myFile.FileName}");
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}


