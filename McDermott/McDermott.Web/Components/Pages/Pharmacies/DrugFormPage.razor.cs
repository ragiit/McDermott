using McDermott.Application.Dtos.Pharmacies;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Pharmacies.DrugFormCommand;
using static McDermott.Application.Features.Commands.Pharmacies.SignaCommand;

namespace McDermott.Web.Components.Pages.Pharmacies
{
    public partial class DrugFormPage
    {
        #region Relation Data

        private List<DrugFormDto> DataFormDrugs = [];

        #endregion Relation Data

        #region Properties Grid

        private IGrid Grid;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Properties Grid

        #region Variabel static

        private bool showForm { get; set; } = false;
        private bool PanelVisible { get; set; } = false;

        #endregion Variabel static

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

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
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
                var a = await Mediator.QueryGetHelper<DrugForm, DrugFormDto>(pageIndex, pageSize, searchTerm);
                DataFormDrugs = a.Item1;
                totalCount = a.pageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion async Data

        #region Grid

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        #endregion Grid

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
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #endregion Click

        #region function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDrugFormRequest(((DrugFormDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteDrugFormRequest(ids: SelectedDataItems.Adapt<List<DrugFormDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion function Delete

        #region function Save

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;

                var editModel = (DrugFormDto)e.EditModel;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateDrugFormRequest(editModel));
                else
                    await Mediator.Send(new UpdateDrugFormRequest(editModel));

                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion function Save

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private List<ExportFileData> ExportTemp =
        [
            new() { Column = "Name", Notes = "Mandatory" },
            new() { Column = "Code" }
        ];

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "drug_form_template.xlsx", ExportTemp);
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

                    var headerNames = new List<string>() { "Name", "Code" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString().Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match the grid.");
                        return;
                    }

                    var list = new List<DrugFormDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        list.Add(new DrugFormDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim(),
                            Code = ws.Cells[row, 2].Value?.ToString()?.Trim(),
                        });
                    }

                    if (list.Count > 0)
                    {
                        list = list.DistinctBy(x => new { x.Name, x.Code }).ToList();

                        var existingVillages = await Mediator.Send(new BulkValidateDrugFormQuery(list));

                        list = list.Where(village => !existingVillages.Any(ev => ev.Name == village.Name && ev.Code == village.Code)).ToList();

                        await Mediator.Send(new CreateListDrugFormRequest(list));
                        await LoadData(0, pageSize);
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
        }
    }
}