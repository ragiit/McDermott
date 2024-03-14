namespace McDermott.Web.Components.Pages.Config
{
    public partial class CityPage
    {
        private DxUpload MyUpload { get; set; }

        private List<CityDto> Cities = [];
        private List<ProvinceDto> Provinces = [];
        private List<CountryDto> Countriestk = [];
        private HubConnection hubConnection;
        private GroupMenuDto UserAccessCRUID = new();

        private async Task SelectedFilesChanged(IEnumerable<UploadFileInfo> files)
        {
            //foreach (var file in files)
            //{
            //    if (file.Type == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            //    {
            //        using (var stream = new MemoryStream())
            //        {
            //            stream.Position = 0;

            //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //            // Gunakan EPPlus untuk membaca data Excel dari MemoryStream
            //            using (var package = new ExcelPackage(stream))
            //            {
            //                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Ambil worksheet pertama

            //                long rowCount = worksheet.Dimension.Rows;
            //                long colCount = worksheet.Dimension.Columns;

            //                // Lakukan sesuatu dengan data dari file Excel
            //                for (long row = 1; row <= rowCount; row++)
            //                {
            //                    for (long col = 1; col <= colCount; col++)
            //                    {
            //                        var cellValue = worksheet.Cells[row, col].Value;
            //                        // Lakukan sesuatu dengan nilai sel, misalnya, simpan ke dalam struktur data atau tampilkan di UI.
            //                        Console.WriteLine(cellValue);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            // Refresh tampilan setelah selesai
            InvokeAsync(StateHasChanged);
        }

        private void OnFileUploadStarted(FileUploadEventArgs e)
        {
            try
            {
                var a = e;

                InvokeAsync(StateHasChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void woi()
        {
        }

        #region Default Grid Components

        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private string names { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;

                //hubConnection = new HubConnectionBuilder()
                //    .WithUrl("http://localhost:5000/realTimeHub")
                //    .Build();
                //hubConnection.On<CountryDto>("ReceivedCountry", (e) =>
                //{
                //    names = e.Name;
                //});
                //await hubConnection.StartAsync();
            }
            catch { }

            Provinces = await Mediator.Send(new GetProvinceQuery());
            await LoadData();
        }

        private bool IsAccess = false;

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
            Cities = await Mediator.Send(new GetCityQuery());
            PanelVisible = false;
        }

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

        private async Task NewItem_Click(IGrid context)
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
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

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteCityRequest(((CityDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CityDto>>();
                    await Mediator.Send(new DeleteListCityRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (CityDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCityRequest(editModel));
                else
                    await Mediator.Send(new UpdateCityRequest(editModel));

                await LoadData();
            }
            catch { }
        }

        #endregion Default Grid Components
    }
}