using McHealthCare.Application.Dtos.Others;

namespace McHealthCare.Application.Services
{
    public interface IFileExportService
    {
        Task<byte[]> GenerateColumnImportTemplateExcelFileAsync(List<ExportFileData> data);
    }
}