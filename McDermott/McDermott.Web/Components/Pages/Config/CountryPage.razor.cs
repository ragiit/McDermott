using DocumentFormat.OpenXml.Office2010.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using McDermott.Web.Components.Layout;
using Document = iTextSharp.text.Document;
using DevExpress.Printing.Core.Native;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using static McDermott.Application.Features.Commands.GetDataCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CountryPage : IDisposable
    {
        private List<CountryDto> Countries = new();
        private Timer _timer;

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await GetUserInfo();
            }
        }

        private async Task GetUserInfo()
        {
            try
            {
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region Grid

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        private object Data { get; set; }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<Country>(await Mediator.Send(new GetQueryCountrylable()))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };
                Data = dataSource;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion Grid

        #region ImportExport

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "country_template.xlsx", new List<ExportFileData>
            {
                new() { Column = "Name", Notes = "Mandatory" },
                new() { Column = "Code"},
            });
        }

        public async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jsRuntime, IFileExportService file, string fileName, DotNetStreamReference streamReference, List<ExportFileData> data, string name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jsRuntime.InvokeVoidAsync("saveFileExcellExporrt", fileName, streamRef);
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Name", "Code", };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        list.Add(country);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Code, }).ToList();

                        // Panggil BulkValidateCountryQuery untuk validasi bulk
                        var existingCountrys = await Mediator.Send(new BulkValidateCountryQuery(list));

                        // Filter Country baru yang tidak ada di database
                        list = list.Where(Country =>
                            !existingCountrys.Any(ev =>
                                ev.Name == Country.Name &&
                                ev.Code == Country.Code
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListCountryRequest(list));
                        await LoadData();
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
                finally { PanelVisible = false; }
            }
            PanelVisible = false;
        }

        #endregion ImportExport

        #region Grid Events

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        [Inject]
        private IHttpClientFactory ClientFactory { get; set; }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
            //CreatePdf("ohYeah.pff");

            //var client = ClientFactory.CreateClient("ServerAPI");
            //var response = await client.GetAsync($"api/file/download-pdf");
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteCountryRequest(((CountryDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItems.Adapt<List<CountryDto>>();
                    await Mediator.Send(new DeleteCountryRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (CountryDto)e.EditModel;

                bool validate = await Mediator.Send(new ValidateCountryQuery(x => x.Id != editModel.Id && x.Name == editModel.Name && x.Code == editModel.Code));

                if (validate)
                {
                    ToastService.ShowInfo($"Country with name '{editModel.Name}' and code '{editModel.Code}' is already exists");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCountryRequest(editModel));
                else
                    await Mediator.Send(new UpdateCountryRequest(editModel));

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Grid Events

        public void CreatePdf(string outputPath)
        {
            // Step 1: Create Document and set page size
            Document doc = new Document(PageSize.A4);

            // Step 2: Set up PdfWriter
            PdfWriter.GetInstance(doc, new FileStream(outputPath, FileMode.Create));

            // Step 3: Open Document
            doc.Open();

            // Step 4: Add elements to the document
            // Define fonts
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

            // Add clinic information
            doc.Add(new Paragraph("KLINIK PRATAMA", headerFont));
            doc.Add(new Paragraph("PT. MCDERMOTT INDONESIA", headerFont));
            doc.Add(new Paragraph("Jl. Bawal, Batu Ampar, Batam 29452", regularFont));
            doc.Add(new Paragraph("Tel: (62) 778 414 001, Fax: (62) 778 411 913", regularFont));

            doc.Add(new Paragraph("\n"));

            // Add Date Section
            doc.Add(new Paragraph($"Tgl./Date: %Date%", regularFont));
            doc.Add(new Paragraph("MC: - Days", regularFont));
            doc.Add(new Paragraph("Refer to:", regularFont));

            doc.Add(new Paragraph("\n"));

            // Add Medical Report Title
            var reportTitle = new Paragraph("MEDICAL REPORT\nBUKTI PELAYANAN RAWAT JALAN", headerFont);
            reportTitle.Alignment = Element.ALIGN_CENTER;
            doc.Add(reportTitle);

            doc.Add(new Paragraph("\n"));

            // Add Patient Information
            doc.Add(new Paragraph("Name : %NamePatient%", regularFont));
            doc.Add(new Paragraph("Department : %Department%   Emp No: %NIP%", regularFont));
            doc.Add(new Paragraph("Diagnosis :    BPJS No:", regularFont));
            doc.Add(new Paragraph("Treatment :", regularFont));

            doc.Add(new Paragraph("\n"));

            // Add Placeholder for Signature Lines
            doc.Add(new Paragraph("_________________", regularFont));

            doc.Add(new Paragraph("\n"));

            // Add Checklist Table
            PdfPTable checklistTable = new PdfPTable(3);
            checklistTable.WidthPercentage = 100;
            checklistTable.SetWidths(new float[] { 5, 1, 1 }); // Column widths

            checklistTable.AddCell(new PdfPCell(new Phrase("NO", headerFont)));
            checklistTable.AddCell(new PdfPCell(new Phrase("Aspek Telaah", headerFont)));
            checklistTable.AddCell(new PdfPCell(new Phrase("Ya", headerFont)));
            checklistTable.AddCell(new PdfPCell(new Phrase("Tidak", headerFont)));

            string[] aspects = new string[]
            {
            "Kejelasan tulisan resep", "Tepat nama obat bentuk, kekuatan sediaan",
            "Tepat waktu dan frekuensi pemberian", "Tepat rute pemberian",
            "Tepat dosis", "Tepat indikasi", "Ada atau tidaknya duplikasi",
            "Interaksi obat", "Kontraindikasi", "Polifarmasi", "Alergi"
            };

            for (int i = 0; i < aspects.Length; i++)
            {
                checklistTable.AddCell(new PdfPCell(new Phrase((i + 1).ToString(), regularFont)));
                checklistTable.AddCell(new PdfPCell(new Phrase(aspects[i], regularFont)));
                checklistTable.AddCell(new PdfPCell(new Phrase("", regularFont))); // Placeholder for "Ya"
                checklistTable.AddCell(new PdfPCell(new Phrase("", regularFont))); // Placeholder for "Tidak"
            }

            doc.Add(checklistTable);

            doc.Add(new Paragraph("\n"));

            // Add Signature Section
            doc.Add(new Paragraph("Tanda Tangan", headerFont));
            doc.Add(new Paragraph("Petugas      : _______________", smallFont));
            doc.Add(new Paragraph("Pasien       : _______________", smallFont));

            doc.Add(new Paragraph("\n"));

            // Additional Information
            doc.Add(new Paragraph("Perubahan Resep", headerFont));
            doc.Add(new Paragraph("Petugas Farmasi Dokter Tertulis Menjadi", regularFont));

            // Add second checklist table for Obat Telaah section if needed.

            // Step 5: Close Document
            doc.Close();
        }
    }
}