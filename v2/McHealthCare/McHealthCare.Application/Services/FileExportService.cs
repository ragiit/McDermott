using McHealthCare.Application.Dtos.Others;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McHealthCare.Application.Services
{
    public class FileExportService : IFileExportService
    {
        public async Task<byte[]> GenerateColumnImportTemplateExcelFileAsync(List<ExportFileData> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Sheet1");

            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#82b8d7");

            for (int i = 1; i <= data.Count; i++)
            {
                worksheet.Cells[1, i].Value = data.ToList()[i - 1].Column;

                if (!string.IsNullOrWhiteSpace(data[i - 1].Notes))
                {
                    //worksheet.Cells[1, i].AddComment(data[i - 1].Notes);
                    var comment = worksheet.Cells[1, i].AddComment(data[i - 1].Notes);
                    comment.AutoFit = true; // AutoFit the comment box size
                }

                worksheet.Cells[1, i].Style.Font.Bold = true;
                worksheet.Cells[1, i].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
                worksheet.Column(i).AutoFit();
            }

            // Create the table
            var tableRange = worksheet.Cells[1, 1, 1, data.Count];

            var excelTable = worksheet.Tables.Add(tableRange, "Table");
            excelTable.TableStyle = OfficeOpenXml.Table.TableStyles.Light1;

            // Add borders to the table range
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Add thick border to the header row
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            return await Task.FromResult(package.GetAsByteArray());
        }
    }

}
