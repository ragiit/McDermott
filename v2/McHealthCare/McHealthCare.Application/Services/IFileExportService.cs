using McHealthCare.Application.Dtos.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Services
{
    public interface IFileExportService
    {
        Task<byte[]> GenerateColumnImportTemplateExcelFileAsync(List<ExportFileData> data);
    }
}
