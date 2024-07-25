using Blazored.Toast.Services;
using McHealthCare.Application.Dtos.Others;
using McHealthCare.Application.Services;
using Microsoft.JSInterop;

namespace McHealthCare.Web.Extentions.CS
{
    public static class Helper
    {
        public static async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        public static void ShowErrorImport(this IToastService ToastService, int row, int col, string val)
        {
            ToastService.ShowInfo($"Data \"{val}\" in row {row} and column {col} is invalid");
        }
    }
}
