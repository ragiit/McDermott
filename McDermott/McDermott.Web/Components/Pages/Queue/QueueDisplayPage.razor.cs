using DevExpress.Blazor;
using McDermott.Application.Dtos.Queue;
using OfficeOpenXml;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;
using static McDermott.Application.Features.Commands.Queue.QueueDisplayCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class QueueDisplayPage
    {
        #region relation Data

        private List<DetailQueueDisplayDto> DetailQueueDisplay = [];
        private List<QueueDisplayDto> QueueDisplay = [];
        private List<CounterDto> Counters = [];
        public List<TempDetailQueueDisplayDto> TempDisplays = new();
        private GroupMenuDto UserAccessCRUID = new();
        public TempDetailQueueDisplayDto FormDisplays = new();
        public QueueDisplayDto Displays = new();
        public DetailQueueDisplayDto DetDisplays = new();

        #endregion relation Data

        #region Grid Properties

        private IEnumerable<CounterDto> selectedCounter { get; set; } = [];
        private bool IsAccess = false;
        private bool PanelVisible { get; set; } = true;
        private bool showPopUp { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private List<CounterDto>? counteres = [];

        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        #endregion Grid Properties

        #region
        private string? CountName { get; set; } = string.Empty;

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
            showPopUp = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            DetailQueueDisplay = await Mediator.Send(new GetDetailQueueDisplayQuery());
            QueueDisplay = await Mediator.Send(new GetQueueDisplayQuery());
            Counters = await Mediator.Send(new GetCounterQuery());
            counteres = [.. Counters.Where(x => x.Status == "on process")];

            // Join data DetailQueueDisplay dan QueueDisplay berdasarkan QueueDisplayId
            var joinedData = DetailQueueDisplay.Join(QueueDisplay,
                                                    detail => detail.QueueDisplayId,
                                                    queue => queue.Id,
                                                    (detail, queue) => new { Detail = detail, Queue = queue });

            // Mengelompokkan data berdasarkan ID tampilan
            var groupedData = joinedData.GroupBy(item => item.Queue.Id);

            // Membuat objek TempDisplayDto dari data yang telah digabungkan
            TempDisplays.Clear(); // Bersihkan koleksi sebelum menambahkan data baru

            foreach (var group in groupedData)
            {
                var displayQueueName = group.First().Queue.Name;
                var counterNames = string.Join(", ", group.Select(item => counteres.FirstOrDefault(counter => counter.Id == item.Detail.CounterId)?.Name));

                TempDisplays.Add(new TempDetailQueueDisplayDto
                {
                    Name = displayQueueName,
                    NameCounter = counterNames
                });
            }
            PanelVisible = false;
        }

        #endregion
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

        private async Task OnRenderTo(DetailQueueDisplayDto context)
        {
            var DisplayId = context.Id;
            //NavigationManager.NavigateTo($"/queue/viewdisplay/{DisplayId}", true);
        }

        #endregion Method OnRenderTo

        #region Method Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDetailQueueDisplayRequest(((DetailQueueDisplayDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DetailQueueDisplayDto>>();
                    await Mediator.Send(new DeleteListDetailQueueDisplayRequest(a.Select(x => x.Id).ToList()));
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

                if (FormDisplays != null)
                {
                    Displays.Name = FormDisplays.Name;

                    var DisplayId = await Mediator.Send(new CreateQueueDisplayRequest(Displays));
                    //var cekDisplayId = await Mediator.Send(new GetQueueDisplayByIdQuery(DisplayId));

                    var ListCounter = selectedCounter.Select(x => x.Id).ToList();
                    foreach (var counter in ListCounter)
                    {
                        DetDisplays.QueueDisplayId = DisplayId.Id;
                        DetDisplays.CounterId = counter;
                        await Mediator.Send(new CreateDetailQueueDisplayRequest(DetDisplays));
                    }
                }

                //    //FormDisplays.CounterId = selectedCounter.Select(x => x.Id).ToList();
                //    await Mediator.Send(new UpdateQueueDisplayRequest(FormDisplays));

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