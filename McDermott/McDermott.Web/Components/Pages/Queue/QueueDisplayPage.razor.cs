using McDermott.Application.Dtos.Queue;
using Microsoft.AspNetCore.Components.Forms;
using OfficeOpenXml;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class QueueDisplayPage
    {
        #region relation Data

        private List<QueueDisplayDto> QueueDisplay = [];
        private List<CounterDto> Counters = [];
        private GroupMenuDto UserAccessCRUID = new();
        public QueueDisplayDto FormDisplays = new();

        #endregion relation Data

        private IEnumerable<CounterDto> selectedCounter { get; set; } = [];
        private bool IsAccess = false;
        private bool PanelVisible { get; set; } = true;
        private bool showPopUp { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private List<CounterDto>? counteres = [];

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
            QueueDisplay = await Mediator.Send(new GetQueueDisplayQuery());
            Counters = await Mediator.Send(new GetCounterQuery());
            counteres = [.. Counters.Where(x => x.Status == "on process")];
            //QueueDisplay.ForEach(x => x.NameCounter = string.Join(",", Counters.Where(z => x.CounterId != null && x.CounterId.Contains(z.Id)).Select(x => x.Name).ToList()));
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
            catch (Exception ee)
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
                    var ListCounter = selectedCounter.Select(x => x.Id).ToList();
                    //FormDisplays.CounterId?.AddRange(ListCounter);
                    await Mediator.Send(new CreateQueueDisplayRequest(FormDisplays));
                }
                else
                {
                    //FormDisplays.CounterId = selectedCounter.Select(x => x.Id).ToList();
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
    }
}