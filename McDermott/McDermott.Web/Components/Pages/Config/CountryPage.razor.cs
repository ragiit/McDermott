using OfficeOpenXml;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CountryPage
    {
        private List<CountryDto> Countries = [];
        private GroupMenuDto UserAccessCRUID = new();

        private bool IsAccess = false;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            await LoadData();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                try
                {
                    var result = await NavigationManager.CheckAccessUser(oLocal);
                    IsAccess = result.Item1;
                    UserAccessCRUID = result.Item2;
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Countries = await Mediator.Send(new GetCountryQuery());
            PanelVisible = false;
        }

        #region Grid

        protected void SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
        {
            InvokeAsync(StateHasChanged);
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
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

                    var headerNames = new List<string>() { "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString().Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match the grid.");
                        return;
                    }

                    var countries = new List<CountryDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var country = new CountryDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim()
                        };

                        if (!Countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()) && !countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()))
                            countries.Add(country);
                    }

                    await Mediator.Send(new CreateListCountryRequest(countries));

                    await LoadData();
                }
                catch { }
            }
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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
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

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
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
                    await Mediator.Send(new DeleteListCountryRequest(a.Select(x => x.Id).ToList()));
                }
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

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCountryRequest(editModel));
                else
                    await Mediator.Send(new UpdateCountryRequest(editModel));

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