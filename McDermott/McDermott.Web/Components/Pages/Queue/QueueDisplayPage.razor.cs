using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class QueueDisplayPage
    {
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

        #region relation Data

        private List<DetailQueueDisplayDto> DetailQueueDisplay = new();
        private List<QueueDisplayDto> QueueDisplay = [];
        private List<CounterDto> Counters = [];
        public QueueDisplayDto FormDisplays = new();
        public QueueDisplayDto Displays = new();
        public DetailQueueDisplayDto DetDisplays = new();

        #endregion relation Data

        #region Grid Properties

        private IEnumerable<CounterDto>? selectedCounter { get; set; } = [];
        private bool PanelVisible { get; set; } = true;
        private bool showPopUp { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private List<CounterDto>? counteres = [];

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region async Data

        private string? CountName { get; set; } = string.Empty;
        private string? disName { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
        }

        private async Task LoadData()
        {
            showPopUp = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            QueueDisplay = await Mediator.Send(new GetQueueDisplayQuery());
            Counters = await Mediator.Send(new GetCounterQuery());
            counteres = [.. Counters.Where(x => x.Status == "on process")];
            QueueDisplay.ForEach(x => x.NameCounter = string.Join(",", counteres.Where(z => x.CounterIds != null && x.CounterIds.Contains(z.Id)).Select(z => z.Name).ToList()));
            PanelVisible = false;
        }

        #endregion async Data

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

                        //if (!Countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()) && !countries.Any(x => x.Name.Trim().ToLower() == country.Name.Trim().ToLower()))
                        ////countries.Add(country);
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

        #endregion Grid

        #region Function Button

        private async Task NewItem_Click()
        {
            showPopUp = true;
        }

        private async Task EditItem_Click()
        {
            FormDisplays = SelectedDataItems[0].Adapt<QueueDisplayDto>();
            selectedCounter = counteres.Where(x => FormDisplays.CounterIds.Contains(x.Id)).ToList();

            showPopUp = true;
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

        private async Task OnCancel()
        {
            showPopUp = false;
            await LoadData();
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        #endregion Function Button

        #region Method OnRenderTo

        private async Task OnRenderTo(QueueDisplayDto context)
        {
            var DisplayId = context.Id;
            NavigationManager.NavigateTo($"/queue/viewdisplay/{DisplayId}", true);
        }

        #endregion Method OnRenderTo

        #region Method Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteQueueDisplayRequest(((QueueDisplayDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<QueueDisplayDto>>();
                    await Mediator.Send(new DeleteListQueueDisplayRequest(a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception)
            {
            }
        }

        #endregion Method Delete

        #region Method save

        private async Task OnSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FormDisplays.Name))
                    return;

                if (FormDisplays.Id == 0)
                {
                    Displays.Name = FormDisplays.Name;
                    var ListCounter = selectedCounter.Select(x => x.Id).ToList();
                    FormDisplays.CounterIds?.AddRange(ListCounter);
                    await Mediator.Send(new CreateQueueDisplayRequest(FormDisplays));
                    ToastService.ShowSuccess("Configuration Display Success!");
                    //var cekDisplayId = await Mediator.Send(new GetQueueDisplayByIdQuery(DisplayId));

                    //foreach (var counter in ListCounter)
                    //{
                    //    DetDisplays.QueueDisplayId = DisplayId.Id;
                    //    DetDisplays.CounterId = counter;
                    //    await Mediator.Send(new CreateDetailQueueDisplayRequest(DetDisplays));
                    //}
                }
                else
                {
                    FormDisplays.CounterIds = selectedCounter.Select(x => x.Id).ToList();
                    await Mediator.Send(new UpdateQueueDisplayRequest(FormDisplays));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Method save

        // Model untuk menyimpan data tampilan grid
        public class DisplayDataModel
        {
            public int Id { get; set; }
            public string? Name { get; set; } = string.Empty;
            public string? CounterNames { get; set; } = string.Empty;
        }
    }
}