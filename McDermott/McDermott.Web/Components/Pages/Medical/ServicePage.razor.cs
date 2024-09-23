using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;

namespace McDermott.Web.Components.Pages.Medical
{
    public partial class ServicePage
    {
        public List<ServiceDto> Services = [];
        public List<ServiceDto> ServicesK = new();
        public ServiceDto FormService = new();

        #region Default Grid

        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private string TextPopUp { get; set; } = string.Empty;
        private Timer _timer;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

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
                try
                {
                    await GetUserInfo();
                }
                catch { }
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

        private string KioskName { get; set; } = String.Empty;

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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
            await LoadDataService();
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

        #region ComboboxService

        private DxComboBox<ServiceDto, long?> refServiceComboBox { get; set; }
        private int ServiceComboBoxIndex { get; set; } = 0;
        private int totalCountService = 0;

        private async Task OnSearchService()
        {
            await LoadDataService();
        }

        private async Task OnSearchServiceIndexIncrement()
        {
            if (ServiceComboBoxIndex < (totalCountService - 1))
            {
                ServiceComboBoxIndex++;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServicendexDecrement()
        {
            if (ServiceComboBoxIndex > 0)
            {
                ServiceComboBoxIndex--;
                await LoadDataService(ServiceComboBoxIndex, 10);
            }
        }

        private async Task OnInputServiceChanged(string e)
        {
            ServiceComboBoxIndex = 0;
            await LoadDataService();
        }

        private async Task LoadDataService(int pageIndex = 0, int pageSize = 10, long? ServiceId = null)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetServiceQuery(x => x.IsKiosk == true, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refServiceComboBox?.Text ?? ""));
            ServicesK = result.Item1;
            totalCountService = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxService

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            PopUpVisible = false;
            SelectedDataItems = [];
            var result = await Mediator.Send(new GetServiceQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
            Services = result.Item1;
            activePageIndex = pageIndex;
            totalCount = result.pageCount;

            foreach (var i in Services)
            {
                if (i.IsKiosk == true && i.IsPatient == false)
                {
                    i.Flag = "Counter";
                    if (i.ServicedId != null && i.ServicedId != 0)
                    {
                        i.KioskName = Services.Where(x => x.Id == i.ServicedId).Select(z => z.Name).FirstOrDefault();
                    }
                    else
                    {
                        i.KioskName = "-";
                    }
                }
                else if (i.IsKiosk == false && i.IsPatient == true)
                {
                    i.Flag = "Patient";
                    if (i.ServicedId != null && i.ServicedId != 0)
                    {
                        i.KioskName = Services.Where(x => x.Id == i.ServicedId).Select(z => z.Name).FirstOrDefault();
                    }
                    else
                    {
                        i.KioskName = "-";
                    }
                }
                else if (i.IsKiosk == true && i.IsPatient == true)
                {
                    i.Flag = " Patient, Counter";
                    if (i.ServicedId != null && i.ServicedId != 0)
                    {
                        i.KioskName = Services.Where(x => x.Id == i.ServicedId).Select(z => z.Name).FirstOrDefault();
                    }
                    else
                    {
                        i.KioskName = "-";
                    }
                }
                else
                {
                    i.Flag = "-";
                    i.KioskName = "-";
                }
            }

            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteServiceRequest(((ServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<ServiceDto>>();
                    await Mediator.Send(new DeleteServiceRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (ServiceDto)e.EditModel;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateServiceRequest(editModel));
                else
                    await Mediator.Send(new UpdateServiceRequest(editModel));

                //await hubConnection.SendAsync("SendCountry", editModel);

                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            EditItemsEnabled = true;
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

                    var headerNames = new List<string>() { "Name", "Code", "Quota" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<ServiceDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        list.Add(new ServiceDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                            Quota = ws.Cells[row, 3].Value?.ToString()?.Trim(),
                        });
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Code, x.Quota }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateServiceQuery(list));

                        // Filter village baru yang tidak ada di database
                        list = list.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.Name == village.Name &&
                                ev.Quota == village.Quota &&
                                ev.Code == village.Code
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListServiceRequest(list));
                        await LoadData(0, pageSize);
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "services_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Code"
                },
                new()
                {
                    Column = "Quota"
                },
            ]);
        }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();

            return;
            FormService = new();
            PopUpVisible = true;
            TextPopUp = "Add Services";
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as ServiceDto ?? new());

            await LoadComboboxEdit(a);
        }

        private async Task LoadComboboxEdit(ServiceDto a)
        {
            if (a.IsPatient)
                ServicesK = (await Mediator.Send(new GetServiceQuery(x => x.Id == a.ServicedId && x.IsKiosk == true))).Item1;
        }

        private void OnCancel()
        {
            FormService = new();
            PopUpVisible = false;
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        #endregion Default Grid
    }
}