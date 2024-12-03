using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Wizards;
using McDermott.Web.Components.Pages.Reports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using static McDermott.Application.Features.Commands.Transaction.GCReferToInternalCommand;

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
                    Pratitioner = new User
                    {
                        Name = x.Pratitioner.Name,
                    },

                    VisitNumber = x.VisitNumber,
                    ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                    PPKRujukanName = x.PPKRujukanName,
                    ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                    InsurancePolicyId = x.InsurancePolicyId,
                    ReferDiagnosisNm = x.ReferDiagnosisNm,
                    ReferDateVisit = x.ReferDateVisit,
                    PracticeScheduleTimeDate = x.PracticeScheduleTimeDate,
                    ReferralExpiry = x.ReferralExpiry,
                    ReferSelectFaskesDate = x.ReferSelectFaskesDate,
                    ReferralNo = x.ReferralNo
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

            myReport.xrLabelTsDokter.Text = gx.ReferVerticalSpesialisParentSpesialisName; // Spesialis
            myReport.xrLabelDi.Text = gx.PPKRujukanName;

            // Patient
            myReport.xrLabelNama.Text = gx.Patient?.Name ?? "-";
            myReport.xrLabelBPJS.Text = inspolcy.PolicyNumber;
            myReport.xrLabelDiagnosaa.Text = gx.ReferDiagnosisNm; // Diagnosa
            myReport.xrLabelUmur.Text = gx.Patient?.Age.ToString() ?? "-";
            myReport.xrLabelTahun.Text = gx.Patient?.DateOfBirth.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelStatusTanggunan.Text = inspolcy.JnsKelasKode;
            myReport.xrLabelGender.Text = gx.Patient?.Gender == "Male" ? "L" : "P";

            myReport.xrLabelRencana.Text = gx.ReferDateVisit.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelJadwal.Text = gx.PracticeScheduleTimeDate;
            myReport.xrLabelBerlakuDate.Text = gx.ReferralExpiry.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelSalamDate.Text = $"Salam sejawat,\r\n{gx.ReferSelectFaskesDate.GetValueOrDefault().ToString("dd MMMM yyyy")}";
            myReport.xrLabelDokter.Text = gx.Pratitioner?.Name ?? "-";

            myReport.xrBarCode1.Text = gx.ReferralNo;
            myReport.xrBarCode1.ShowText = false;

            // Export ke PDF
            myReport.ExportToPdf(stream);

            // Kembalikan sebagai array byte
            var ar = stream.ToArray();

            return File(ar, "application/pdf", "Report_RujukanBPJS.pdf");
        }

        [HttpGet("rujukan-bpjs-prb/{id}")]
        public async Task<IActionResult> DownloadReportRujukanBpjsPRB(long id)
        {
            using var stream = new MemoryStream();
            // Buat laporan
            var myReport = new ReportRujukanBPJS_PRB();

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
                    Pratitioner = new User
                    {
                        Name = x.Pratitioner.Name,
                    },

                    VisitNumber = x.VisitNumber,
                    ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                    PPKRujukanName = x.PPKRujukanName,
                    ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                    InsurancePolicyId = x.InsurancePolicyId,
                    ReferDiagnosisNm = x.ReferDiagnosisNm,
                    ReferDateVisit = x.ReferDateVisit,
                    PracticeScheduleTimeDate = x.PracticeScheduleTimeDate,
                    ReferralExpiry = x.ReferralExpiry,
                    ReferSelectFaskesDate = x.ReferSelectFaskesDate,
                    ReferralNo = x.ReferralNo
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

            myReport.xrLabelTsDokter.Text = gx.ReferVerticalSpesialisParentSpesialisName; // Spesialis
            myReport.xrLabelDi.Text = gx.PPKRujukanName;

            // Patient
            myReport.xrLabelNama.Text = gx.Patient?.Name ?? "-";
            myReport.xrLabelBPJS.Text = inspolcy.PolicyNumber;
            myReport.xrLabelDiagnosaa.Text = gx.ReferDiagnosisNm; // Diagnosa
            myReport.xrLabelUmur.Text = gx.Patient?.Age.ToString() ?? "-";
            myReport.xrLabelTahun.Text = gx.Patient?.DateOfBirth.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelStatusTanggunan.Text = inspolcy.JnsKelasKode;
            myReport.xrLabelGender.Text = gx.Patient?.Gender == "Male" ? "L" : "P";

            myReport.xrLabelRencana.Text = gx.ReferDateVisit.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelJadwal.Text = gx.PracticeScheduleTimeDate;
            myReport.xrLabelBerlakuDate.Text = gx.ReferralExpiry.GetValueOrDefault().ToString("dd-MMM-yyyy");
            myReport.xrLabelSalamDate.Text = $"Salam sejawat,\r\n{gx.ReferSelectFaskesDate.GetValueOrDefault().ToString("dd MMMM yyyy")}";
            myReport.xrLabelDokter.Text = gx.Pratitioner?.Name ?? "-";

            myReport.xrBarCode1.Text = gx.ReferralNo;
            myReport.xrBarCode1.ShowText = false;


            myReport.xrLabelRujukanBalikName.Text = gx.Patient?.Name ?? "-";

            // Export ke PDF
            myReport.ExportToPdf(stream);

            // Kembalikan sebagai array byte
            var ar = stream.ToArray();

            return File(ar, "application/pdf", "Report_RujukanBPJS_PRB.pdf");
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

        [HttpGet("mcd-referral/{id}")]
        public async Task<IActionResult> DownloadReportMcDReferral(long id)
        {
            using var stream = new MemoryStream();

            var ExtraReport = new McDReferral();
            var gs = await mediator.Send(new GetSingleGCReferToInternalQuery
            {
                Predicate = x => x.GeneralConsultanServiceId == id,
                Select = x => new GCReferToInternal
                {
                    GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                    DateRJMCINT = x.DateRJMCINT,
                    Number = x.Number,
                    ReferTo = x.ReferTo,
                    Hospital = x.Hospital,
                    CategoryRJMCINT = x.CategoryRJMCINT,
                    ExamFor = x.ExamFor,
                    TempDiagnosis = x.TempDiagnosis,
                    TherapyProvide = x.TherapyProvide,
                    InpatientClass = x.InpatientClass,
                    Specialist = x.Specialist,
                }
            }) ?? new();
            var gx = await mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == gs.GeneralConsultanServiceId,
                Select = x => new GeneralConsultanService
                {
                    PatientId = x.PatientId,
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

            var gp = await mediator.Send(new GetSingleUserQuery
            {
                Predicate = x => x.Id == gx.PatientId,
                Select = x => new User
                {
                    OccupationalId=x.OccupationalId,
                    Occupational = new Occupational
                    {
                        Name =x.Occupational.Name
                    },
                    Name =x.Name,
                    
                }
            });

            ExtraReport.xrDateRJ.Text = gs.DateRJMCINT.ToString("dd MMMM yyyy");
            ExtraReport.xrNumber.Text = gs.Number ?? "-";
            ExtraReport.xrTo.Text = gs.ReferTo ?? "-";
            ExtraReport.xrPatientName.Text = gp.Name ?? "-";
            ExtraReport.xrOccupational.Text =gp.Occupational.Name ?? "-";
            ExtraReport.xrNoEmployee.Text = gp.NIP ?? "-";
            ExtraReport.xrTempDiagnosis.Text = gs.TempDiagnosis ?? "-";
            ExtraReport.xrTherapyProvide.Text = gs.TherapyProvide ?? "-";
            ExtraReport.xrNotes.Text =  "";

            // Dynamically set the checkbox for the hospital
            ExtraReport.xrRSE.Checked = gs.Hospital == "RSE";
            ExtraReport.xrRSBK.Checked = gs.Hospital == "RSBK";
            ExtraReport.xrRSHB.Checked = gs.Hospital == "RSHB";
            ExtraReport.xrRSBP.Checked = gs.Hospital == "RSBP";
            ExtraReport.xrRSAB.Checked = gs.Hospital == "RSAB";
            ExtraReport.xrRSGH.Checked = gs.Hospital == "RSGH";
            ExtraReport.xrRSMA.Checked = gs.Hospital == "RSMA";
            ExtraReport.xrRSHBH.Checked = gs.Hospital == "RSHBH";
            ExtraReport.xrRSSD.Checked = gs.Hospital == "RSSD";

            // Dynamically set the checkbox for Category
            ExtraReport.xrKanker.Checked = gs.CategoryRJMCINT == "KANKER";
            ExtraReport.xrDependent.Checked = gs.CategoryRJMCINT == "DEPENDENT";
            ExtraReport.xrEmployee.Checked = gs.CategoryRJMCINT == "EMPLOYEE";
            ExtraReport.xrAccidentInside.Checked = gs.CategoryRJMCINT == "ACCIDENT Inside";
            ExtraReport.xrAccidentOutside.Checked = gs.CategoryRJMCINT == "ACCIDENT Outside";
            ExtraReport.xrKelainan.Checked = gs.CategoryRJMCINT == "KELAINAN BAWAAN";

            // Dynamically set the checkbox for Inpatient Class
            ExtraReport.xrClassVIP.Checked = gs.InpatientClass == "VIP Class";
            ExtraReport.xrClass1B.Checked = gs.InpatientClass == "Class 1 B";
            ExtraReport.xrClass2.Checked = gs.InpatientClass == "Class 2";

            // Dynamically set the checkbox for Examp For
            ExtraReport.xrFurther.Checked = gs.ExamFor == "Pemeriksaan / penanganan lebih lanjut";
            ExtraReport.xrSurgery.Checked = gs.ExamFor == "Pembedahan";
            ExtraReport.xrHospitalization.Checked = gs.ExamFor == "Perawatan";
            ExtraReport.xrMaternity.Checked = gs.ExamFor == "Bersalin";
            ExtraReport.xrRefaction.Checked = gs.ExamFor == "Pemeriksaan Refraksi Mata";
            ExtraReport.xrPhysiotherapy.Checked = gs.ExamFor == "Fisioterapy";

            // Dynamically set the checkbox for Specialist
            ExtraReport.xrDentist.Checked = gs.Specialist == "Dentist";
            ExtraReport.xrInternist.Checked = gs.Specialist == "Internist";
            ExtraReport.xrPulmonologist.Checked = gs.Specialist == "Pulmonologist";
            ExtraReport.xrCardiologist.Checked = gs.Specialist == "Cardiologist";
            ExtraReport.xrEye.Checked = gs.Specialist == "Eye";
            ExtraReport.xrENT.Checked = gs.Specialist == "ENT";
            ExtraReport.xrPaediatric.Checked = gs.Specialist == "Paediatric";
            ExtraReport.xrSurgeon.Checked = gs.Specialist == "Surgeon";
            ExtraReport.xrObstetrician.Checked = gs.Specialist == "Obstetrician";
            ExtraReport.xrNeurologist.Checked = gs.Specialist == "Neurologist";
            ExtraReport.xrUrologist.Checked = gs.Specialist == "Urologist";
            ExtraReport.xrNeurosurgeon.Checked = gs.Specialist == "Neurosurgeon";
            ExtraReport.xrOrthopaedic.Checked = gs.Specialist == "Orthopaedic";
            ExtraReport.xrPhysiotherapist.Checked = gs.Specialist == "Physiotherapist";
            ExtraReport.xrDermatologist.Checked = gs.Specialist == "Dermatologist";
            ExtraReport.xrPsychiatrist.Checked = gs.Specialist == "Psychiatrist";
            ExtraReport.xrLaboratorium.Checked = gs.Specialist == "Laboratorium";

            // Export ke PDF
            ExtraReport.ExportToPdf(stream);

            // Kembalikan sebagai array byte
            var ar = stream.ToArray();
            return File(ar, "application/pdf", "McD_Referral.pdf");
        }

        [HttpGet("mc-glasess-referral/{id}")]
        private async Task<IActionResult> DownloadMcGlasessReferral(long id)
        {
            using var stream = new MemoryStream();

            var ExtraReport = new McGlassesReport();

            var gs = await mediator.Send(new GetSingleGCReferToInternalQuery
            {
                Predicate = x => x.GeneralConsultanServiceId == id,
                Select = x => new GCReferToInternal
                {
                    GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                    DateRJMCINT = x.DateRJMCINT,
                    Number = x.Number,
                    ReferTo = x.ReferTo,
                    Hospital = x.Hospital,
                    CategoryRJMCINT = x.CategoryRJMCINT,
                    ExamFor = x.ExamFor,
                    TempDiagnosis = x.TempDiagnosis,
                    TherapyProvide = x.TherapyProvide,
                    InpatientClass = x.InpatientClass,
                    Specialist = x.Specialist,
                }
            }) ?? new();
            var gx = await mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == gs.GeneralConsultanServiceId,
                Select = x => new GeneralConsultanService
                {
                    PatientId = x.PatientId,
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

            var gp = await mediator.Send(new GetSingleUserQuery
            {
                Predicate = x => x.Id == gx.PatientId,
                Select = x => new User
                {
                    OccupationalId = x.OccupationalId,
                    Occupational = new Occupational
                    {
                        Name = x.Occupational.Name
                    },
                    Name = x.Name,

                }
            });

            ExtraReport.xrDateRJ.Text = gs.DateRJMCINT.ToString("dd MMMM yyyy");
            ExtraReport.xrNumber.Text = gs.Number ?? "-";
            ExtraReport.xrTo.Text = gs.ReferTo ?? "-";
            ExtraReport.xrPatientName.Text = gp.Name ?? "-";
            ExtraReport.xrOccupational.Text = gp.Occupational.Name ?? "-";
            ExtraReport.xrNoEmployee.Text = gx.Patient.Legacy ?? "-";
            ExtraReport.xrTempDiagnosis.Text = gs.TempDiagnosis ?? "-";
            ExtraReport.xrTherapyProvide.Text = gs.TherapyProvide ?? "-";
            ExtraReport.xrNotes.Text = "";

            
            // Dynamically set the checkbox for Category
            ExtraReport.xrKanker.Checked = gs.CategoryRJMCINT == "KANKER";
            ExtraReport.xrDependent.Checked = gs.CategoryRJMCINT == "DEPENDENT";
            ExtraReport.xrEmployee.Checked = gs.CategoryRJMCINT == "EMPLOYEE";
            ExtraReport.xrAccidentInside.Checked = gs.CategoryRJMCINT == "ACCIDENT Inside";
            ExtraReport.xrAccidentOutside.Checked = gs.CategoryRJMCINT == "ACCIDENT Outside";
            ExtraReport.xrKelainan.Checked = gs.CategoryRJMCINT == "KELAINAN BAWAAN";


            // Dynamically set the checkbox for Examp For
            ExtraReport.xrFurther.Checked = gs.ExamFor == "Pemeriksaan / penanganan lebih lanjut";
            ExtraReport.xrSurgery.Checked = gs.ExamFor == "Pembedahan";
            ExtraReport.xrHospitalization.Checked = gs.ExamFor == "Perawatan";
            ExtraReport.xrMaternity.Checked = gs.ExamFor == "Bersalin";
            ExtraReport.xrRefaction.Checked = gs.ExamFor == "Pemeriksaan Refraksi Mata";
            ExtraReport.xrPhysiotherapy.Checked = gs.ExamFor == "Fisioterapy";

            // Export ke PDF
            ExtraReport.ExportToPdf(stream);

            // Kembalikan sebagai array byte
            var ar = stream.ToArray();
            return File(ar, "application/pdf", "Mc_Glasess_Referral.pdf");
        }

        [HttpGet("safety-glasess-referral/{id}")]
        private async Task<IActionResult> DownloadSafetyGlassesReferral(long id)
        {
            using var stream = new MemoryStream();

            var ExtraReport = new SafetyGlassesReport();

            var gs = await mediator.Send(new GetSingleGCReferToInternalQuery
            {
                Predicate = x => x.GeneralConsultanServiceId == id,
                Select = x => new GCReferToInternal
                {
                    GeneralConsultanServiceId = x.GeneralConsultanServiceId,
                    DateRJMCINT = x.DateRJMCINT,
                    Number = x.Number,
                    ReferTo = x.ReferTo,
                    CategoryRJMCINT = x.CategoryRJMCINT,
                    ExamFor = x.ExamFor,
                    TempDiagnosis = x.TempDiagnosis,
                }
            }) ?? new();
            var gx = await mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == gs.GeneralConsultanServiceId,
                Select = x => new GeneralConsultanService
                {
                    PatientId=x.PatientId,
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

            var gp = await mediator.Send(new GetSingleUserQuery
            {
                Predicate = x => x.Id == gx.PatientId,
                Select = x => new User
                {
                    OccupationalId = x.OccupationalId,
                    Occupational = new Occupational
                    {
                        Name = x.Occupational.Name
                    },
                    Name = x.Name,

                }
            });

            ExtraReport.xrDatesRJ.Text = gs.DateRJMCINT.ToString("dd MMMM yyyy");
            ExtraReport.xrNumber.Text = gs.Number ?? "-";
            ExtraReport.xrReferTo.Text = gs.ReferTo ?? "-";
            ExtraReport.xrPatient.Text = gp.Name ?? "-";
            ExtraReport.xrOccupational.Text = gp.Occupational.Name ?? "-";
            ExtraReport.xrNoEmployee.Text = gx.Patient.Legacy ?? "-";
            ExtraReport.xrTempDiagnosis.Text = gs.TempDiagnosis ?? "-";
          
            // Export ke PDF
            ExtraReport.ExportToPdf(stream);

            // Kembalikan sebagai array byte
            var ar = stream.ToArray();
            return File(ar, "application/pdf", "Mc_Glasess_Referral.pdf");
        }
    }
}