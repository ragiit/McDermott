using OfficeOpenXml.Style;

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
            switch (FormReports.report)
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

        private async Task VisitByDiagnosis(ReportDto formReports)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("en_US");

            var header = new List<string>() { "Diagnosis", "Total Patient" };

            TemplateClinicName(ws);

            ws.Cells[6, 1].Value = "Date Period";
            ws.Cells[6, 2].Value = FormReports.StartDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo) + " - " + FormReports.EndDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo);
            ws.Cells[7, 1].Value = "Total number of Visits";
            ws.Cells[8, 1].Value = "Diagnosis Umum";

            ws.Cells[10, 1].Value = header[0];
            ws.Cells[10, 2].Value = header[1];
            ws.Cells[10, 1].Style.Font.Bold = true;
            ws.Cells[10, 2].Style.Font.Bold = true;
            ws.Cells[6, 1].Style.Font.Bold = true;
            ws.Cells[7, 1].Style.Font.Bold = true;
            ws.Cells[8, 1].Style.Font.Bold = true;
            ws.Cells[10, 1].AutoFitColumns();
            ws.Cells[10, 2].AutoFitColumns();

            ws.Cells[4, 2].AutoFitColumns();

            int startRow = 11;
            int count = 0;

            //Query

            var tableRange = ws.Cells[7, 1, 7 + count, 2];

            // Create the table
            var excelTable = ws.Tables.Add(tableRange, "Table");
            excelTable.TableStyle = OfficeOpenXml.Table.TableStyles.Light1;

            // Add borders to the table range
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Add thick border to the header row
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            await SaveFileToSpreadSheetml(pack, $"{FormReports.report}.xlsx");
        }

        private async Task ReportMedicalPersonalLicence(ReportDto formReports)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("en_US");

            var header = new List<string>() { "Name of Medical Personal", "Licence Validity Period" };

            TemplateClinicName(ws);

            ws.Cells[7, 1].Value = header[0];
            ws.Cells[7, 2].Value = header[1];

            ws.Cells[7, 1].Style.Font.Bold = true;
            ws.Cells[7, 2].Style.Font.Bold = true;

            ws.Cells[4, 2].AutoFitColumns();
            ws.Cells[7, 1].AutoFitColumns();

            var users = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true));

            int startRow = 8;

            int count = 0;

            foreach (var item in users.OrderBy(x => x.Name))
            {
                if (item.IsPhysicion)
                {
                    if (item.SipExp is not null)
                    {
                        ws.Cells[startRow, 1].Value = item.Name;
                        ws.Cells[startRow, 2].Value = item.SipExp.GetValueOrDefault().ToString("dd/MM/yyyy", cultureInfo);

                        startRow++;
                        count++;
                    }
                }
                else if (item.IsNurse)
                {
                    if (item.StrExp is not null)
                    {
                        ws.Cells[startRow, 1].Value = item.Name;
                        ws.Cells[startRow, 2].Value = item.StrExp.GetValueOrDefault().ToString("dd/MM/yyyy", cultureInfo);

                        startRow++;
                        count++;
                    }
                }
            }

            var tableRange = ws.Cells[7, 1, 7 + count, 2];

            // Create the table
            var excelTable = ws.Tables.Add(tableRange, "Table");
            excelTable.TableStyle = OfficeOpenXml.Table.TableStyles.Light1;

            // Add borders to the table range
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Add thick border to the header row
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            await SaveFileToSpreadSheetml(pack, $"{FormReports.report}.xlsx");
        }

        private void TemplateClinicName(ExcelWorksheet ws)
        {
            ws.Cells[2, 2].Value = FormReports.report;
            ws.Cells[3, 1].Value = "Clinic";
            ws.Cells[3, 2].Value = "McHealthCare";
            ws.Cells[4, 1].Value = "Address";
            ws.Cells[4, 2].Value = "Jalan Bawal No. 1 – Batu Ampar Batam Island 29452, Riau Islands Province";
            ws.Cells[5, 1].Value = "Date";
            ws.Cells[5, 2].Value = DateTime.Now.Date.ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("en_US"));

            ws.Cells[2, 2].Style.Font.Bold = true;
            ws.Cells[3, 1].Style.Font.Bold = true;
            ws.Cells[3, 2].Style.Font.Bold = true;
            ws.Cells[4, 1].Style.Font.Bold = true;
            ws.Cells[5, 1].Style.Font.Bold = true;
        }

        private async Task VisitByPeriode(ReportDto FormReports)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("en_US");

            var header = new List<string>() { "Date", "Type Medical", "Patient Count" };

            var currentProtocol = HttpContextAccessor.HttpContext.Request.Scheme;

            var currentHost = HttpContextAccessor.HttpContext.Request.Host.Value;
            var baseUrl = $"{currentProtocol}://{currentHost}";

            var apiUrl = $"{baseUrl}/mcdermott_logo.png";

            //// Lokasi penyimpanan gambar sementara
            //string imagePath = Path.Combine("wwwroot", "temp", "mcdermott_logo.png");

            //// Unduh gambar dari URL
            //using (WebClient webClient = new WebClient())
            //{
            //    webClient.DownloadFile(apiUrl, imagePath);
            //}

            // Tambahkan gambar ke file Excel
            //var picture1 = ws.Drawings.AddPicture("Image1", new FileInfo("wwwroot\\mcdermott_logo.png"));
            //picture1.From.Column = 1; // Kolom 1
            //picture1.From.Row = 1; // Baris 1
            //picture1.SetSize(100, 100); // Atur ukuran gambar (opsional)

            //var picture2 = ws.Drawings.AddPicture("Image2", new FileInfo("wwwroot\\mcdermott_logo.png"));
            //picture2.From.Column = 1; // Kolom 1
            //picture2.From.Row = 2; // Baris 2
            //picture2.SetSize(100, 100); // Atur ukuran gambar (opsional)

            //// Gabung sel untuk gambar
            //ws.Cells[1, 1, 2, 2].Merge = true;

            //// Atur properti style untuk sel gabungan
            //ws.Cells[1, 1, 2, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //ws.Cells[1, 1, 2, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            TemplateClinicName(ws);

            ws.Cells[6, 1].Value = "Date Period";
            ws.Cells[6, 2].Value = FormReports.StartDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo) + " - " + FormReports.EndDate.Value.Date.ToString("dd/MM/yyyy", cultureInfo);
            ws.Cells[7, 1].Value = "Total number of Visits";

            ws.Cells[6, 1].Style.Font.Bold = true;
            ws.Cells[7, 1].Style.Font.Bold = true;
            ws.Cells[9, 3].Style.Font.Bold = true;

            var generals = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.RegistrationDate.GetValueOrDefault().Date >= FormReports.StartDate.GetValueOrDefault().Date && x.RegistrationDate.GetValueOrDefault().Date <= FormReports.EndDate.GetValueOrDefault().Date));

            ws.Cells[9, 1].Value = "Date";
            ws.Cells[9, 2].Value = "Service";
            ws.Cells[9, 3].Value = "Total Patients";

            int startRow = 10;

            var namee = new List<string>();

            long totalPatiens = 0;
            int counts = 0;

            foreach (var item in generals)
            {
                if (namee.Contains(item.Service?.Name!))
                    continue;

                long count = generals.Where(x => x.ServiceId == item.ServiceId && x.RegistrationDate.Date == item.RegistrationDate.Date).Count();

                ws.Cells[startRow, 1].Value = item.RegistrationDate.Date.ToString("dd/MM/yyyy", cultureInfo);
                ws.Cells[startRow, 2].Value = item.Service?.Name;
                ws.Cells[startRow, 3].Value = count;

                totalPatiens += count;

                namee.Add(item.Service?.Name!);

                startRow++;
                counts++;
            }

            ws.Cells[7, 2].Value = totalPatiens;

            var tableRange = ws.Cells[9, 1, 9 + counts, 3];

            // Create the table
            var excelTable = ws.Tables.Add(tableRange, "Table");
            excelTable.TableStyle = OfficeOpenXml.Table.TableStyles.Light1;

            // Add borders to the table range
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Add thick border to the header row
            tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

            ws.Cells[2, 2].AutoFitColumns();
            ws.Cells[4, 2].AutoFitColumns();
            ws.Cells[7, 1].AutoFitColumns();
            ws.Cells[9, 3].AutoFitColumns();

            await SaveFileToSpreadSheetml(pack, "Report of Patient visits by Period.xlsx");
        }

        private async Task VisitByDepartement(ReportDto FormRepots)
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
            ws.Cells[4, 1].Value = "Departement Name";
            ws.Cells[4, 2].Value = "Total Patiens";

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

            var generals = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.RegistrationDate.GetValueOrDefault().Date >= FormReports.StartDate.GetValueOrDefault().Date && x.RegistrationDate.GetValueOrDefault().Date <= FormReports.EndDate.GetValueOrDefault().Date));

            int startRow = 5;

            var namee = new List<string>();

            long totalPatiens = 0;

            foreach (var item in generals)
            {
                if (namee.Contains(item.Patient?.Department?.Name!))
                    continue;

                long count = generals.Where(x => x.Patient.DepartmentId == item.Patient.DepartmentId && x.RegistrationDate.Date == item.RegistrationDate.Date).Count();

                //ws.Cells[startRow, 1].Value = item.RegistrationDate.Date.ToString("dd/MM/yyyy");
                ws.Cells[startRow, 1].Value = item.Patient?.Department?.Name;
                ws.Cells[startRow, 2].Value = count;

                totalPatiens += count;

                namee.Add(item.Patient?.Department?.Name!);

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