using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Wizards;
using McDermott.Web.Components.Pages.Reports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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

        [HttpGet("a")]
        public async Task<IActionResult> A()
        {
            using var stream = new MemoryStream();

            // Buat laporan
            var report = new TestReportList();

            // Ambil data produk
            var products = await mediator.Send(new GetProductQueryNew
            {
                IsGetAll = true
            });

            // Set data source untuk laporan
            report.DataSource = products.Item1;

            // Buat XRTable di dalam Detail band
            XRTable table = new XRTable
            {
                WidthF = report.PageWidth - report.Margins.Left - report.Margins.Right // Sesuaikan dengan lebar halaman
            };

            // Tambahkan border ke tabel
            table.Borders = DevExpress.XtraPrinting.BorderSide.All;
            table.BorderWidth = 1f;
            table.BorderColor = System.Drawing.Color.Black;
            report.Detail.Controls.Add(table);

            // Memulai inisialisasi tabel
            table.BeginInit();

            // Loop melalui semua produk untuk membuat baris tabel
            foreach (var product in products.Item1)
            {
                XRTableRow row = new XRTableRow();

                // Buat sel untuk setiap kolom
                XRTableCell productName = new XRTableCell
                {
                    Text = product.Name // Isi dengan nama produk
                };

                //XRTableCell productPrice = new XRTableCell
                //{
                //    Text = product.Cost.ToString("C") // Isi dengan harga produk (format mata uang)
                //};

                // Tambahkan sel ke baris
                row.Cells.Add(productName);
                //row.Cells.Add(productPrice);

                // Tambahkan baris ke tabel
                table.Rows.Add(row);
            }

            // Selesaikan inisialisasi tabel
            table.EndInit();

            // Export laporan ke PDF
            report.ExportToPdf(stream);

            // Kembalikan sebagai array byte untuk didownload
            var pdfBytes = stream.ToArray();

            return File(pdfBytes, "application/pdf", "ProductList.pdf");
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
                Select = x => new GeneralConsultanService
                {
                    Patient = new User
                    {
                        Name = x.Patient.Name,
                        DateOfBirth = x.Patient.DateOfBirth,
                        Gender = x.Patient.Gender,
                    },

                    VisitNumber = x.VisitNumber,
                    ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                    InsurancePolicyId = x.InsurancePolicyId,
                }
            }) ?? new();

            var inspolcy = await mediator.Send(new GetSingleInsurancePolicyQuery
            {
                Predicate = x => x.Id == gx.InsurancePolicyId,
                Select = x => new InsurancePolicy
                {
                    PolicyNumber = x.PolicyNumber,
                    JnsKelasKode = x.JnsKelasKode
                }
            }) ?? new();

            // Header
            myReport.xrLabelNoKunjungan.Text = gx.VisitNumber;
            myReport.xrLabelFKTP.Text = gx.ReferVerticalSpesialisSaranaName;
            myReport.xrLabelKota.Text = gx.VisitNumber;
            myReport.xrLabelTsDokter.Text = gx.VisitNumber; // Spesialis
            myReport.xrLabelDi.Text = gx.VisitNumber;

            // Patient
            myReport.xrLabelNama.Text = gx.Patient?.Name ?? "-";
            myReport.xrLabelBPJS.Text = inspolcy.PolicyNumber;
            myReport.xrLabelDiagnosaa.Text = gx.VisitNumber; // Diagnosa
            myReport.xrLabelUmur.Text = gx.Patient?.Age.ToString() ?? "-";
            myReport.xrLabelTahun.Text = gx.Patient?.DateOfBirth.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelStatusTanggunan.Text = inspolcy.JnsKelasKode;
            myReport.xrLabelGender.Text = gx.Patient?.Gender == "Male" ? "L" : "P";

            myReport.xrLabelRencana.Text = gx.VisitNumber;
            myReport.xrLabelJadwal.Text = gx.ScheduleTime;
            myReport.xrLabelBerlakuDate.Text = gx.VisitNumber;
            myReport.xrLabelSalamDate.Text = gx.VisitNumber;
            myReport.xrLabelDokter.Text = gx.VisitNumber;

            myReport.xrBarCode1.Text = "0070B0531124P000486";
            myReport.xrBarCode1.ShowText = false;

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