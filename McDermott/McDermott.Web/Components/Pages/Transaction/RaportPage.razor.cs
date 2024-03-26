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
        private bool LicenseValidityPeriode { get; set; } = false;
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
<<<<<<< HEAD
            switch (FormReports.report)
=======
            
            var a = FormReports;
            if (FormReports.report == "Report of patient visits by period")
>>>>>>> malika
            {
                case "Report of validity period of medical personnel licenses":
                    await ReportMedicalPersonalLicence(FormReports);
                    break;

                case "Report of patient visits by period":
                    await VisitByPeriode(FormReports);
                    break;

                default:
                    break;
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

        #region Report

        private async Task ReportMedicalPersonalLicence(ReportDto formReports)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("en_US");

            var header = new List<string>() { "Name of Medical Personal", "Licence Validity Period" };

            ws.Cells[1, 1, 1, 2].Merge = true;

            ws.Cells[1, 1].Value = FormReports.report;
            ws.Cells[2, 1].Value = header[0];
            ws.Cells[2, 2].Value = header[1];

            ws.Cells[1, 1].Style.Font.Bold = true;
            ws.Cells[2, 1].Style.Font.Bold = true;
            ws.Cells[2, 2].Style.Font.Bold = true;

            ws.Cells[1, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[2, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
            ws.Cells[2, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

            // Set wrap text for column 1 and 2
            ws.Cells[1, 1].AutoFitColumns();
            ws.Cells[2, 1].AutoFitColumns();
            ws.Cells[2, 2].AutoFitColumns();

            var users = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true));

            int startRow = 3;

            foreach (var item in users.OrderBy(x => x.Name))
            {
                if (item.IsPhysicion)
                {
                    if (item.StrExp is not null && item.StrExp.Value.Date >= FormReports.StartDate!.Value.Date && item.StrExp.Value.Date <= FormReports.EndDate!.Value.Date)
                    {
                        ws.Cells[startRow, 1].Value = item.Name;
                        ws.Cells[startRow, 2].Value = item.StrExp.GetValueOrDefault().ToString("dd/MM/yyyy", cultureInfo);

                        startRow++;
                    }
                }
                else if (item.IsNurse)
                {
                    if (item.SipExp is not null && item.SipExp.Value.Date >= FormReports.StartDate!.Value.Date && item.SipExp.Value.Date <= FormReports.EndDate!.Value.Date)
                    {
                        ws.Cells[startRow, 1].Value = item.Name;
                        ws.Cells[startRow, 2].Value = item.SipExp.GetValueOrDefault().ToString("dd/MM/yyyy", cultureInfo);

                        startRow++;
                    }
                }
            }

            await SaveFileToSpreadSheetml(pack, $"{FormReports.report}.xlsx");
        }

        private async Task VisitByPeriode(ReportDto FormReports)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("en_US");

            var header = new List<string>() { "Date", "Type Medical", "Patient Count" };

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

            // Set wrap text for column 1 and 2
            ws.Column(1).Style.WrapText = true;
            ws.Column(2).Style.WrapText = true;

            ws.Cells[2, 2].Value = FormReports.StartDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo) + " - " + FormReports.EndDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo);

            var generals = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.StagingStatus != "Canceled" && x.RegistrationDate.GetValueOrDefault().Date >= FormReports.StartDate.GetValueOrDefault().Date && x.RegistrationDate.GetValueOrDefault().Date <= FormReports.EndDate.GetValueOrDefault().Date));

            int startRow = 5;

            var namee = new List<string>();

            long totalPatiens = 0;

            foreach (var item in generals)
            {
                if (namee.Contains(item.Service?.Name!))
                    continue;

                long count = generals.Where(x => x.ServiceId == item.ServiceId && x.RegistrationDate.Date == item.RegistrationDate.Date).Count();

                ws.Cells[startRow, 1].Value = item.RegistrationDate.Date.ToString("dd/MM/yyyy");
                ws.Cells[startRow, 2].Value = item.Service?.Name;
                ws.Cells[startRow, 3].Value = count;

                totalPatiens += count;

                namee.Add(item.Service?.Name!);

                startRow++;
            }

            ws.Cells[3, 2].Value = totalPatiens;

            await SaveFileToSpreadSheetml(pack, "Report of Patient visits by Period.xlsx");
        }

        private async Task SaveFileToSpreadSheetml(ExcelPackage excelPackage, string title)
        {
            await JsRuntime.InvokeVoidAsync("saveAsFile", title, Convert.ToBase64String(excelPackage.GetAsByteArray()));
        }

        #endregion Report
    }
}