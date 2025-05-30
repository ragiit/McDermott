﻿using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using McDermott.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Document = iTextSharp.text.Document;
using Path = System.IO.Path;

namespace McDermott.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public FileController(IMediator mediator, ApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [ModelBinder(BinderType = typeof(DataSourceLoadOptionsBinder))]
        public class DataSourceLoadOptions : DataSourceLoadOptionsBase
        {
        }

        public class DataSourceLoadOptionsBinder : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var loadOptions = new DataSourceLoadOptions();
                DataSourceLoadOptionsParser.Parse(loadOptions, key => bindingContext.ValueProvider.GetValue(key).FirstOrDefault());
                bindingContext.Result = ModelBindingResult.Success(loadOptions);
                return Task.CompletedTask;
            }
        }

        [HttpGet("yeay")]
        public async Task<IActionResult> GetProducts(DataSourceLoadOptions loadOptions)
        {
            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            loadOptions.PrimaryKey = new[] { "Id" };
            loadOptions.PaginateViaPrimaryKey = true;
            // Use your IDbContextFactory instance instead of _contextFactory

            //var aa = await _mediator.Send(new GetDistrictQuery());

            var loadResult = await DataSourceLoader.LoadAsync(_context.Districts, loadOptions);

            return Ok(loadResult);
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path is required.");
            }

            var fileName = Path.GetFileName(filePath);
            var contentType = "application/octet-stream"; // Default content type, can be determined based on file extension

            // Ensure the file path is within the wwwroot/files directory for security reasons
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);

            if (!System.IO.File.Exists(fullFilePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(fullFilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, contentType, fileName);
        }

        [HttpGet("download-pdf")]
        public IActionResult DownloadPdf()
        {
            // Step 1: Create a memory stream to store the PDF
            using var stream = new MemoryStream();

            // Step 2: Initialize the PDF document and writer
            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc, stream);
            writer.CloseStream = false; // Prevent the writer from closing the stream

            // Step 3: Open the document
            doc.Open();

            // Add content to the document
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            doc.Add(new Paragraph("KLINIK PRATAMA", headerFont));
            doc.Add(new Paragraph("PT. MCDERMOTT INDONESIA", headerFont));
            doc.Add(new Paragraph("Jl. Bawal, Batu Ampar, Batam 29452", regularFont));
            doc.Add(new Paragraph("Tel: (62) 778 414 001, Fax: (62) 778 411 913", regularFont));
            doc.Add(new Paragraph("\n"));

            // Step 4: Close the Document (finalizes content)
            doc.Close();

            // Step 5: Reset stream position to beginning
            stream.Seek(0, SeekOrigin.Begin);

            // Return the PDF file as a download
            return File(stream.ToArray(), "application/pdf", "MedicalReport.pdf");
        }
    }
}