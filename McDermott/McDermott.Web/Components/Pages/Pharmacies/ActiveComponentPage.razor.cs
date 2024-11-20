using McDermott.Application.Dtos.Pharmacies;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Pharmacies.SignaCommand;

namespace McDermott.Web.Components.Pages.Pharmacies
{
    public partial class ActiveComponentPage
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

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                    }
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

        #region Static

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private List<ActiveComponentDto> ActiveComponents = [];
        private List<UomDto> Uoms = [];

        #endregion Static

        #region Load

        protected override async Task OnInitializedAsync()
        {
            //Uoms = await Mediator.Send(new GetUomQuery(x => x.Active == true));
            await GetUserInfo();
            await LoadData();
            await LoadDataUom();
        }

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.QueryGetHelper<ActiveComponent, ActiveComponentDto>(pageIndex, pageSize, searchTerm);
                ActiveComponents = a.Item1;
                totalCount = a.pageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Load

        #region Click

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            PanelVisible = true;
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as ActiveComponentDto ?? new()); 
            Uoms = (await Mediator.Send(new GetUomQuery
            {
                Predicate = x => x.Id == a.UomId
            })).Item1;
            PanelVisible = false;
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
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteActiveComponentRequest(((ActiveComponentDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteActiveComponentRequest(ids: SelectedDataItems.Adapt<List<ActiveComponentDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData(activePageIndex, pageSize);
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
                var editModel = (ActiveComponentDto)e.EditModel;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateActiveComponentRequest(editModel));
                else
                    await Mediator.Send(new UpdateActiveComponentRequest(editModel));

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Click

        #region Grid

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid

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

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => ExportTemp.Select(x => x.Column).ToList()[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        PanelVisible = false;
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var list = new List<ActiveComponentDto>();

                    var aa = new HashSet<string>();

                    var list1 = new List<UomDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var a = ws.Cells[row, 3].Value?.ToString()?.Trim();

                        if (!string.IsNullOrEmpty(a))
                            aa.Add(a.ToLower());
                    }
                     

                    list1 = (await Mediator.Send(new GetUomQuery
                    {
                        Predicate = x => aa.Contains(x.Name.ToLower()),
                        IsGetAll = true,
                        Select = x => new Uom
                        {
                            Id = x.Id,
                            Name = x.Name,
                        }
                    })).Item1;

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool isValid = true;

                        var uom = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        long? uomId = null;

                        if (!string.IsNullOrWhiteSpace(uom))
                        {
                            var cachedParent = list1.FirstOrDefault(x => x.Name.Equals(uom, StringComparison.CurrentCultureIgnoreCase));
                            if (cachedParent is null)
                            {
                                ToastService.ShowErrorImport(row, 3, uom ?? string.Empty);
                                isValid = false;
                            }
                            else
                            {
                                uomId = cachedParent.Id;
                            }
                        }

                        if (!isValid)
                            continue;

                        var c = new ActiveComponentDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            AmountOfComponent = ws.Cells[row, 2].Value?.ToInt32(),
                            UomId = uomId,
                        };

                        list.Add(c);
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.UomId, x.AmountOfComponent }).ToList();

                        var existingDepartments = await Mediator.Send(new BulkValidateActiveComponentQuery(list));

                        // Filter Department baru yang tidak ada di database
                        list = list.Where(Department =>
                            !existingDepartments.Any(ev =>
                                ev.Name == Department.Name &&
                                ev.AmountOfComponent == Department.AmountOfComponent &&
                                ev.UomId == Department.UomId)
                        ).ToList();

                        await Mediator.Send(new CreateListActiveComponentRequest(list));
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

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Amount of Component"},
            new() { Column = "UoM"}
        ];

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "active_component_template.xlsx", ExportTemp);
        }

        #region ComboboxUom

        private DxComboBox<UomDto, long?> refUomComboBox { get; set; }
        private int UomComboBoxIndex { get; set; } = 0;
        private int totalCountUom = 0;

        private async Task OnSearchUom()
        {
            await LoadDataUom();
        }

        private async Task OnSearchUomIndexIncrement()
        {
            if (UomComboBoxIndex < (totalCountUom - 1))
            {
                UomComboBoxIndex++;
                await LoadDataUom(UomComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUomIndexDecrement()
        {
            if (UomComboBoxIndex > 0)
            {
                UomComboBoxIndex--;
                await LoadDataUom(UomComboBoxIndex, 10);
            }
        }

        private async Task OnInputUomChanged(string e)
        {
            UomComboBoxIndex = 0;
            await LoadDataUom();
        }

        private async Task LoadDataUom(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUomQuery
                {
                    SearchTerm = refUomComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize
                });
                Uoms = result.Item1;
                totalCountUom = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxUom
    }
}