using McDermott.Application.Dtos;

namespace McDermott.Application.Features.Services
{
    public interface IFileExportService
    {
        Task<byte[]> GenerateColumnImportTemplateExcelFileAsync(List<ExportFileData> data);
    }
}