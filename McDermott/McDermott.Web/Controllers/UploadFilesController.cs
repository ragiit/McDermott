using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;
using System.Linq;
using Microsoft.Extensions.Logging;
using McDermott.Persistence.Context;
using MediatR;
using static McDermott.Application.Features.Commands.Inventory.MaintenanceRecordCommand;
using System;
using System.IO;

namespace McDermott.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {

        private readonly ILogger<UploadFilesController> _logger;
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public UploadFilesController(ILogger<UploadFilesController> logger, IMediator mediator, ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _mediator = mediator;
            _environment = environment;
            _context = context;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadMultipleFiles(IFormFileCollection myFile, long? productId, long? maintenanceId, string? sequenceNumber)
        {

            string uploadFolder = System.IO.Path.Combine(_environment.WebRootPath, "files", "DocumentMaintenance");

            // Buat folder jika belum ada
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            // Cek apakah ada file yang diupload
            if (myFile == null || myFile.Count == 0)
            {
                return BadRequest("Tidak ada file yang diupload.");
            }

            // List untuk menyimpan dokumen yang berhasil diupload
            var uploadedDocuments = new List<MaintenanceRecord>();

            foreach (var file in myFile)
            {
                try
                {
                    if (file.Length > 0)
                    {
                        string originalFileName = file.FileName;

                        // Pastikan nama file unik
                        string uniqueFileName = originalFileName;
                        string filePath = System.IO.Path.Combine(uploadFolder, uniqueFileName);

                        // Jika file dengan nama yang sama sudah ada, tambahkan nomor unik
                        int counter = 1;
                        while (System.IO.File.Exists(filePath))
                        {
                            // Tambahkan angka sebelum ekstensi file
                            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(originalFileName);
                            string fileExtension = System.IO.Path.GetExtension(originalFileName);
                            uniqueFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                            filePath = System.IO.Path.Combine(uploadFolder, uniqueFileName);
                            counter++;
                        }

                        // Simpan file ke folder yang ditentukan
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Buat objek FileDocument
                        var fileDocument = new MaintenanceRecord
                        {
                            DocumentName = uniqueFileName,
                            ProductId = productId,
                            MaintenanceId = maintenanceId,
                            SequenceProduct = sequenceNumber
                            //FilePath = System.IO.Path.Combine("files", "DocumentMaintenance", uniqueFileName), // Path relatif untuk disimpan di database
                            //FileSize = file.Length,
                            //FileExtension = System.IO.Path.GetExtension(file.FileName),
                            //UploadDate = DateTime.UtcNow
                        };

                        // Simpan ke database
                        try
                        {
                            var dataxs = await _mediator.Send(new CreateMaintenanceRecordRequest(fileDocument.Adapt<MaintenanceRecordDto>()));
                            uploadedDocuments.Add(fileDocument);
                        }
                        catch (Exception ex)
                        {
                            // Log error atau tangani kesalahan penyimpanan
                            return StatusCode(500, $"Kesalahan database: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Gagal menyimpan file {file.FileName}: {ex.Message}");
                }
            }

            // Simpan perubahan ke database

            // Kembalikan daftar dokumen yang berhasil diupload
            return Ok(new
            {
                message = "Files uploaded successfully.",
                files = uploadedDocuments.Select(doc => new
                {
                    doc.DocumentName,
                    doc.ProductId,
                    doc.MaintenanceId,
                    doc.SequenceProduct
                })
            });

        }


    }
}


