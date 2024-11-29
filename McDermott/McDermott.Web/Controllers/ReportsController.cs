using DevExpress.XtraReports.Wizards;
using McDermott.Web.Components.Pages.Reports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace McDermott.Web.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/reports")]
    [ApiController]
    public class ReportsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        [HttpGet("download")]
        public IActionResult DownloadReport()
        {
            var reportBytes = GenerateReport();

            return File(reportBytes, "application/pdf", "Report.pdf");
        }

        [HttpGet("rujukan-bpjs/{id}")]
        public async Task<IActionResult> DownloadReportRujukanBpjs(long id)
        {
            using var stream = new MemoryStream();
            // Buat laporan
            var myReport = new ReportRujukanBPJS();

            var gx = await mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == id,
                Select = x => x
            });

            myReport.xrLabelNoKunjungan.Text = gx.VisitNumber;
            myReport.xrLabelFKTP.Text = gx.VisitNumber;
            myReport.xrLabelKota.Text = gx.VisitNumber;
            myReport.xrLabelTsDokter.Text = gx.VisitNumber; // diagnosa
            myReport.xrLabelDi.Text = gx.VisitNumber;
            myReport.xrLabelNama.Text = gx.VisitNumber;
            myReport.xrLabelBPJS.Text = gx.VisitNumber;
            myReport.xrLabelDiagnosaa.Text = gx.VisitNumber;
            myReport.xrLabelUmur.Text = gx.VisitNumber;
            myReport.xrLabelTahun.Text = gx.VisitNumber;
            myReport.xrLabelStatusTanggunan.Text = gx.VisitNumber;
            myReport.xrLabelTahun.Text = gx.VisitNumber;
            myReport.xrLabelRencana.Text = gx.VisitNumber;
            myReport.xrLabelJadwal.Text = gx.VisitNumber;
            myReport.xrLabelBerlakuDate.Text = gx.VisitNumber;
            myReport.xrLabelSalamDate.Text = gx.VisitNumber;
            myReport.xrLabelDokter.Text = gx.VisitNumber;

            // Export ke PDF
            myReport.ExportToPdf(stream);

            // Kembalikan sebagai array byte
            var ar = stream.ToArray();

            return File(ar, "application/pdf", "Report_RujukanBPJS.pdf");
        }

        public byte[] GenerateReport()
        {
            using (var stream = new MemoryStream())
            {
                // Buat laporan
                var myReport = new MyReport();
                myReport.xrLabelRujukan.Text = "Ganjar Pranowo";

                // Export ke PDF
                myReport.ExportToPdf(stream);

                // Kembalikan sebagai array byte
                return stream.ToArray();
            }
        }
    }
}