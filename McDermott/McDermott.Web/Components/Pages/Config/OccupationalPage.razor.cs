using static McDermott.Application.Features.Commands.Config.OccupationalCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class OccupationalPage
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
                    StateHasChanged();
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

        private List<string> extentions = new() { ".xlsx", ".xls" };
        private const string ExportFileName = "ExportResult";
        private IEnumerable<GridEditMode> GridEditModes { get; } = Enum.GetValues<GridEditMode>();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private int Value { get; set; } = 0;

        private int FocusedRowVisibleIndex { get; set; }
        private bool PanelVisible { get; set; }
        public IGrid Grid { get; set; }
        private List<OccupationalDto> Occupationals = new();

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteOccupationalRequest(((OccupationalDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<OccupationalDto>>();
                    await Mediator.Send(new DeleteOccupationalRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private bool EditItemsEnabled { get; set; }

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private bool UploadVisible { get; set; } = false;

        protected async Task SelectedFilesChangedAsync(IEnumerable<UploadFileInfo> files)
        {
            UploadVisible = files.ToList().Count == 0;
            try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading Excel file: {ex.Message}");
            }
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private object Data { get; set; }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<Occupational>(await Mediator.Send(new GetQueryOccupational()))
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

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var editModel = (Occupational)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateOccupationalRequest(editModel.Adapt<OccupationalDto>()));
                else
                    await Mediator.Send(new UpdateOccupationalRequest(editModel.Adapt<OccupationalDto>()));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "occupational_template.xlsx",
            [
                new()
                {
                    Column = "Name",
                    Notes = "Mandatory"
                },
                new()
                {
                    Column = "Description"
                },
            ]);
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

                    var headerNames = new List<string>() { "Name", "Description" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<OccupationalDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var c = new OccupationalDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Description = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        };
                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Description }).ToList();

                        // Panggil BulkValidateVillageQuery untuk validasi bulk
                        var existingVillages = await Mediator.Send(new BulkValidateOccupationalQuery(list));

                        // Filter village baru yang tidak ada di database
                        list = list.Where(village =>
                            !existingVillages.Any(ev =>
                                ev.Name == village.Name &&
                                ev.Description == village.Description
                            )
                        ).ToList();

                        await Mediator.Send(new CreateListOccupationalRequest(list));
                        await LoadData();
                        SelectedDataItems = [];
                    }

                    ToastService.ShowSuccessCountImported(list.Count);
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
                finally { PanelVisible = false; }
            }
            PanelVisible = false;
        }
    }
}