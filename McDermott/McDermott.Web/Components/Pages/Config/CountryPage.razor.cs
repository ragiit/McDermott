using McDermott.Application.Features.Services;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CountryPage
    {
        private List<CountryDto> Countries = [];

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //    try
            //    {
            //        await GetUserInfo();
            //    }
            //    catch { }
            //}
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

        private Timer _timer;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }

        //[Inject]
        //private IHttpClientFactory HttpClientFactory2 { get; set; }

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            SelectedDataItems = [];
            var result = await MyQuery.GetCountries(HttpClientFactory, pageIndex, pageSize, searchTerm ?? "");
            Countries = result.Item1;
            totalCount = result.Item2;
            activePageIndex = pageIndex;
            PanelVisible = false;

            return;
            // Menggunakan InvokeAsync untuk memastikan manipulasi UI dilakukan di thread utama
            await InvokeAsync(() =>
                PanelVisible = true // Jika diperlukan, panel diperlihatkan di sini
            );

            // Memuat data
            try
            {
                Countries = await Mediator.Send(new GetCountryQuery());
                Grid.SelectRow(0);
            }
            catch { }

            // Refresh UI setelah memuat data selesai
            await InvokeAsync(() =>
            {
                PanelVisible = false; // Jika diperlukan, panel disembunyikan di sini
                StateHasChanged(); // Memastikan bahwa perubahan UI diterapkan
            });
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        #region Grid

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

                    var headerNames = new List<string>() { "Code", "Name" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var countries = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Code = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Name = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };

                        if (!Countries.Any(x => x.Name.Trim().ToLower() == country?.Name?.Trim().ToLower()) && !countries.Any(x => x.Name.Trim().ToLower() == country?.Name?.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListCountryRequest(countries));

                    await LoadData();
                    SelectedDataItems = [];

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        //public async Task ImportExcelFile(InputFileChangeEventArgs e)
        //{
        //    foreach (var file in e.GetMultipleFiles(1))
        //    {
        //        try
        //        {
        //            using MemoryStream ms = new MemoryStream();
        //            // copy data from file to memory stream
        //            await file.OpenReadStream().CopyToAsync(ms);
        //            // positions the cursor at the beginning of the memory stream
        //            ms.Position = 0;

        //            // create ExcelPackage from memory stream
        //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //            using ExcelPackage package = new ExcelPackage(ms);

        //            ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

        //            long colCount = ws.Dimension.End.Column;
        //            long rowCount = ws.Dimension.End.Row;
        //            var headerNames = new List<string>()
        //            {
        //                "Name",
        //                "Code"
        //            };

        //            for (long i = 1; i <= colCount; i++)
        //            {
        //                var a = headerNames[i - 1].Trim().ToLower();
        //                var b = ws.Cells[1, i].Value?.ToString().Trim().ToLower();
        //                if (a != b)
        //                {
        //                    ToastService.ShowInfo("The header must match the grid.");
        //                    return;
        //                }
        //            }

        //            var countries = new List<CountryDto>();

        //            // Start iterating from row 2
        //            for (long row = 1; row <= rowCount; row++)
        //            {
        //                var country = new CountryDto();

        //                // Access data from row 2 onwards
        //                country.Name = ws.Cells[row, 1].Value?.ToString(); // Assuming name is in the first column

        //                if (countries.Any(x => x.Name.ToLower().Trim().Equals(country.Name.ToLower().Trim())))
        //                    continue;

        //                country.Code = ws.Cells[row, 2].Value?.ToString(); // Assuming code is in the second column

        //                countries.Add(country);
        //            }

        //            // Now you have a list of CountryDto objects extracted from Excel
        //            // You can do further processing with this list, such as saving to database or any other operations
        //        }
        //        catch { }
        //    }
        //}

        //private async Task ImportExcelFile(InputFileChangeEventArgs e)
        //{
        //    foreach (var file in e.GetMultipleFiles(1))
        //    {
        //        try
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                // copy data from file to memory stream
        //                await file.OpenReadStream().CopyToAsync(ms);
        //                // positions the cursor at the beginning of the memory stream
        //                ms.Position = 0;

        //                // create ExcelPackage from memory stream
        //                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //                using (ExcelPackage package = new ExcelPackage(ms))
        //                {
        //                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();
        //                    long colCount = ws.Dimension.End.Column;
        //                    long rowCount = ws.Dimension.End.Row;
        //                    var s = ws.Cells[2, 2].Value;
        //                    // rest of the code here...
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //public class FileExportService
        //{
        //    public async Task<byte[]> GenerateExcelFileAsync(IEnumerable<MyData> data)
        //    {
        //        using var package = new ExcelPackage();
        //        var worksheet = package.Workbook.Worksheets.Add("Sheet1");
        //        Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#82b8d7");

        //        worksheet.Cells[1, 1].Value = "Code";
        //        worksheet.Cells[1, 2].Value = "Name";
        //        worksheet.Cells[1, 1].Style.Font.Bold = true;
        //        worksheet.Cells[1, 2].Style.Font.Bold = true;

        //        worksheet.Cells[1, 1].AddComment("Mandatory Coy");
        //        worksheet.Cells[1, 2].AddComment("Mandatory Iya nih");

        //        worksheet.Cells[1, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;
        //        worksheet.Cells[1, 2].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Hair;

        //        int row = 2;
        //        foreach (var item in data)
        //        {
        //            worksheet.Cells[row, 1].Value = item.Column1;
        //            worksheet.Cells[row, 2].Value = item.Column2;
        //            row++;
        //        }

        //        worksheet.Column(1).AutoFit();
        //        worksheet.Column(2).AutoFit();

        //        // Create the table
        //        var tableRange = worksheet.Cells[1, 1, 1, 2];

        //        var excelTable = worksheet.Tables.Add(tableRange, "Table");
        //        excelTable.TableStyle = OfficeOpenXml.Table.TableStyles.Light1;

        //        // Add borders to the table range
        //        tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //        tableRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        tableRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;

        //        // Add thick border to the header row
        //        tableRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        tableRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //        return await Task.FromResult(package.GetAsByteArray());
        //    }
        //}

        //public class MyData
        //{
        //    public string Column1 { get; set; }
        //    public string Column2 { get; set; }
        //}

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "country_template.xlsx",
            [
                new()
                {
                    Column = "Code",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
            ]);
        }

        public async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, DotNetStreamReference streamReference, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteCountryRequest(((CountryDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CountryDto>>();
                    await Mediator.Send(new DeleteCountryRequest(ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (CountryDto)e.EditModel;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCountryRequest(editModel));
                else
                    await Mediator.Send(new UpdateCountryRequest(editModel));

                //await hubConnection.SendAsync("SendCountry", editModel);
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Grid
    }
}