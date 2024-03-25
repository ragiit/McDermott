namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class RaportPage
    {
        #region relation data

        private List<GeneralConsultanServiceDto> generalConsultans = [];
        private ReportDto FormReports = new();

        #endregion relation data

        #region variable static

        private string _selected;
        private string typeReport;
        private bool showForm { get; set; } = false;
        private DateTime DateTimeValue { get; set; } = DateTime.Now;

        private List<string> ListReport = new()
        {
            "Report of patient visits by period",
            "Report of patient visits by department",
            "Report of patient visits by family relation",
            "Report of patient registrations by period",
            "Report of patient visits by diagnosis",
            "Report of patient examinations by medical type",
            "Top diagnosis report by period",
            "Special cases report",
            "Report of validity period of medical personnel licenses"
        };

        private void selectedReport(string reports)
        {
            if (reports == null)
            {
                showForm = false;
            }
            else
            {
                showForm = true;
            }
        }

        #endregion variable static

        private async Task LoadData()
        {
            generalConsultans = await Mediator.Send(new GetGeneralConsultanServiceQuery());
        }

        private async Task Download()
        {
            //try
            //{
            //    await GenerateExcell();
            //}
            //catch (Exception ex)
            //{
            //    ToastService.ShowError(ex.Message);
            //}
            var a = FormReports;
            if (FormReports.report == "Report of patient visits by period")
            {
                await VisitByPeriode(FormReports);
            }
        }

        private async Task GenerateExcell()
        {
            byte[] fileContent;
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("MySheet");

                #region Header

                worksheet.Cells[1, 1].Value = "Student";
                worksheet.Cells[1, 1].Style.Font.Size = 12;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

                worksheet.Cells[1, 2].Value = "Roll";
                worksheet.Cells[1, 2].Style.Font.Size = 12;
                worksheet.Cells[1, 2].Style.Font.Bold = true;
                worksheet.Cells[1, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

                #endregion Header

                #region Row

                worksheet.Cells[2, 1].Value = "Argi";
                worksheet.Cells[2, 1].Style.Font.Size = 12;
                worksheet.Cells[2, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

                worksheet.Cells[2, 2].Value = "1001";
                worksheet.Cells[2, 2].Style.Font.Size = 12;
                worksheet.Cells[2, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

                #endregion Row

                #region Row

                worksheet.Cells[3, 1].Value = "Iwan";
                worksheet.Cells[3, 1].Style.Font.Size = 12;
                worksheet.Cells[3, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

                worksheet.Cells[3, 2].Value = "1002";
                worksheet.Cells[3, 2].Style.Font.Size = 12;
                worksheet.Cells[3, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

                #endregion Row

                fileContent = package.GetAsByteArray();
            }

            await JsRuntime.InvokeVoidAsync("saveAsFile", "Student.xlsx", Convert.ToBase64String(fileContent));
        }

        private async Task VisitByPeriode(ReportDto FormReports)
        {
            byte[] fileContent;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("en_US");

            var result = generalConsultans.Where(x => x.CreateDate.Value.Date >= FormReports.StartDate.Value.Date && x.CreateDate.Value.Date < FormReports.EndDate.Value.Date.AddDays(1) && x.StagingStatus == "Finished").ToList();

            var data = new List<VisitByPeriod>();
            foreach (var item in result)
            {
                var report = new VisitByPeriod
                {
                    TotalVisit = result.Count(),
                    Services = item.TypeMedical,
                    CountPatient = result.Where(x => x.TypeMedical == item.TypeMedical).Count(),
                };
                data.Add(report);
            }

            var header = new List<string>();
            header = new List<string>() { "Date", "Type Medical", "Patient Count" };

            ws.Cells[1, 2].Value = FormReports.report;
            ws.Cells[2, 1].Value = "Date Period";
            ws.Cells[3, 1].Value = "Total number of Visits";
            ws.Cells[4, 1].Value = "Date";
            ws.Cells[4, 2].Value = "Service";
            ws.Cells[4, 3].Value = "Total Patiens";

            ws.Cells[1, 2].Style.Font.Bold = true;
            ws.Cells[2, 1].Style.Font.Bold = true;
            ws.Cells[3, 1].Style.Font.Bold = true;
            ws.Cells[2, 2].Style.Font.Bold = true;
            ws.Cells[4, 1].Style.Font.Bold = true;
            ws.Cells[4, 2].Style.Font.Bold = true;
            ws.Cells[4, 3].Style.Font.Bold = true;

            ws.Cells[1, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[2, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[2, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[3, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[4, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[4, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[4, 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

            ws.Cells[2, 2].Value = FormReports.StartDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo) + " - " + FormReports.EndDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo);

            var generals = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.RegistrationDate.GetValueOrDefault().Date >= FormReports.StartDate && x.RegistrationDate <= FormReports.EndDate));

            int startRow = 5;

            var namee = new List<string>();

            foreach (var item in generals)
            {
                if (namee.Contains(item.Service?.Name))
                    continue;

                ws.Cells[startRow, 1].Value = item.AppoimentDate.GetValueOrDefault().Date.ToString("dd/MM/yyyy");
                ws.Cells[startRow, 2].Value = item.Service?.Name;
                ws.Cells[startRow, 3].Value = generals.Where(x => x.ServiceId == item.Id && x.RegistrationDate.Date == item.RegistrationDate.Date).Count();

                namee.Add(item.Service?.Name);

                startRow++;
            }

            string fileTitle = "Report of Patient visits by Period.xls";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            fileContent = pack.GetAsByteArray();
            await JsRuntime.InvokeVoidAsync("saveAsFile", fileTitle, Convert.ToBase64String(fileContent));
            //return File(stream, contentType, );
        }
    }
}