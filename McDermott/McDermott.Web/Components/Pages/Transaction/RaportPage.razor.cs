using Microsoft.AspNetCore.Mvc;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class RaportPage
    {
        #region relation data
        private List<GeneralConsultanServiceDto> generalConsultans = [];
        private ReportDto FormReports = new();
        #endregion
        #region variable static
        private string _selected;
        private string typeReport;
        private bool showForm { get; set; } = false;
        DateTime DateTimeValue { get; set; } = DateTime.Now;
        private List<string> ListReport = new()
        {
            "Report of patient visits per period",
            "Report of patient visits per department",
            "Report of patient visits per family relation",
            "Report of patient registrations per period",
            "Report of patient visits per diagnosis",
            "Report of patient examinations per medical type",
            "Top diagnosis report per period",
            "pecial cases report",
            "Report of validity period of medical personnel licenses"

        };

        private void selectedReport(string reports)
        {
           if(reports == null)
            {
                showForm = false;
            }
            else
            {
                showForm = true;
            }
        }
        #endregion
        private async Task LoadData()
        {
            generalConsultans = await Mediator.Send(new GetGeneralConsultanServiceQuery());
        }
        private async Task Download()
        {
            var a = FormReports;
            if(FormReports.report == "Report of patient visits per period")
            {
                VisitByPeriode(FormReports);
            }
        }

        private void VisitByPeriode(ReportDto FormReports)
        {
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;



            var pack = new ExcelPackage();
            var SheetTitle = FormReports.report;
            ExcelWorksheet ws = pack.Workbook.Worksheets.Add(SheetTitle);

            var cultureInfo = new System.Globalization.CultureInfo("id-ID");

           
            var result = generalConsultans.Where(x => x.CreateDate.Value.Date >= FormReports.StartDate.Value.Date && x.CreateDate.Value.Date < FormReports.EndDate.Value.Date.AddDays(1) && x.StagingStatus == "Finished").ToList();

            var data = new List<VisitByPeriod>();
            foreach (var item in result)
            {
                var report = new VisitByPeriod
                {
                    TotalVisit = result.Count(),
                    Services = item.TypeMedical,
                    CountPatient= result.Where(x=>x.TypeMedical == item.TypeMedical).Count(),
                };
                data.Add(report);
            }

            var header = new List<string>();
            header = new List<string>() { "Date", "Type Medical", "Patient Count" };

            ws.Cells[2, 1].Value = FormReports.report;
            ws.Cells[3, 1].Value = "Date Period";
            ws.Cells[3, 2].Value = FormReports.StartDate.Value.Date.ToString("dd MMMM yyyy", cultureInfo) +"-"+ FormReports.EndDate.Value.Date.ToString("dd MMMM yyyy", cultureInfo);

            ws.Cells[2, 1, 2, 3].Merge = true;



            string fileTitle = "Rekapitulasi Request PGS.xls";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            pack.SaveAs(stream);
            stream.Position = 0;
            //return File(stream, contentType, );
        }

    }
}
