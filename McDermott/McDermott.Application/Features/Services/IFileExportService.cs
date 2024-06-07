using McDermott.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Services
{
    public interface IFileExportService
    {
        Task<byte[]> GenerateColumnImportTemplateExcelFileAsync(List<ExportFileData> data);
    }
}
