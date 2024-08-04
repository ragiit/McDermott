using Microsoft.AspNetCore.Components;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class GroupMenuPage
    {
        #region Default Variables & Forms

        private IGrid Grid { get; set; }
        private IGrid GridGroupMenu { get; set; }
        private bool PanelVisible { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; } = -1;
        private int FocusedRowVisibleIndex2 { get; set; } = -1;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new List<object>();
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; } = new List<object>();
        private string Url => Helper.URLS.FirstOrDefault(x => x == "configuration/groups") ?? string.Empty;
        public bool IsLoading { get; set; } = false;
        private string PageName => new Uri(NavigationManager.Uri).PathAndQuery.Replace(NavigationManager.BaseUri, "/");

        #endregion Default Variables & Forms

        #region Default Methods

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                SetLoading(true);
                Groups = await Mediator.Send(new GetGroupQuery());
                Menus = await Mediator.Send(new GetMenuQuery());
            }
            catch (Exception ex)
            {
                // Log error or handle it
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async Task LoadDataGroupMenuAsync()
        {
            try
            {
                GroupMenus = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == Group.Id));
                var a = "ad";
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error loading group menu: {ex.Message}");
            }
        }

        private async Task LoadDataByIdAsync(Guid id)
        {
            try
            {
                Group = (await Mediator.Send(new GetGroupQuery(x => x.Id == id))).FirstOrDefault() ?? new GroupDto();
                await LoadDataGroupMenuAsync();

                if (Group.Id == Guid.Empty)
                {
                    NavigateToUrl(Url);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data by ID: {ex.Message}");
            }
        }

        private void NavigateToUrl(string relativeUrl, bool forceLoad = false)
        {
            var absoluteUrl = $"{NavigationManager.BaseUri}{relativeUrl}";
            NavigationManager.NavigateTo(absoluteUrl, forceLoad);
        }

        private void InitializeNew(bool isParam = false)
        {
            Group = new GroupDto();
            GroupMenus.Clear();

            if (!isParam)
                NavigateToUrl($"{Url}/{EnumPageMode.Create.GetDisplayName()}");
        }

        private async Task InitializeEditAsync()
        {
            if (SelectedDataItems.Count > 0)
            {
                var id = SelectedDataItems[0].Adapt<GroupDto>().Id;
                NavigateToUrl($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{id}");
                await LoadDataByIdAsync(id);
            }
        }

        private async Task HandleValidSubmitAsync()
        {
            try
            {
                Group = Group.Id == Guid.Empty
                    ? await Mediator.Send(new CreateGroupRequest(Group))
                    : await Mediator.Send(new UpdateGroupRequest(Group));

                NavigateToUrl($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{Group.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling valid submit: {ex.Message}");
            }
        }

        private async Task HandleInvalidSubmitAsync()
        {
            // Handle invalid form submission
            Console.WriteLine("Form submission is invalid.");
        }

        private async Task OnDeleteAsync(GridDataItemDeletingEventArgs e)
        {
            try
            {
                SetLoading(true);

                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteGroupRequest(((GroupDto)e.DataItem).Id));
                }
                else
                {
                    var ids = SelectedDataItems.Adapt<List<GroupDto>>().Select(x => x.Id).ToList();
                    await Mediator.Send(new DeleteGroupRequest(Ids: ids));
                }

                SelectedDataItems = new List<object>();
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async Task OnDeleteGroupMenuAsync(GridDataItemDeletingEventArgs e)
        {
            try
            {
                SetLoading(true);

                if (SelectedDataItemsGroupMenu == null || !SelectedDataItemsGroupMenu.Any())
                {
                    await Mediator.Send(new DeleteGroupMenuRequest(((GroupMenuDto)e.DataItem).Id));
                }
                else
                {
                    var ids = SelectedDataItemsGroupMenu.Adapt<List<GroupMenuDto>>().Select(x => x.Id).ToList();
                    await Mediator.Send(new DeleteGroupMenuRequest(Ids: ids));
                }

                SelectedDataItemsGroupMenu = [];
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting item: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async Task OnSaveGroupMenuAsync(GridEditModelSavingEventArgs e)
        {
            try
            {
                SetLoading(true);
                var editModel = (GroupMenuDto)e.EditModel;
                editModel.GroupId = Group.Id;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateGroupMenuRequest(editModel));
                else
                    await Mediator.Send(new UpdateGroupMenuRequest(editModel));

                await LoadDataGroupMenuAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving group menu: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void SetLoading(bool isLoading)
        {
            PanelVisible = isLoading;
        }

        private void CancelItemGroupMenuGrid_Click()
        {
            // Implement logic to cancel group menu grid operation
        }

        private async Task NewItemGroup_ClickAsync()
        {
            GroupMenu = new GroupMenuDto();
            await GridGroupMenu.StartEditNewRowAsync();
        }

        private async Task BackButtonAsync()
        {
            try
            {
                SetLoading(true);
                NavigateToUrl(Url);
                Groups = await Mediator.Send(new GetGroupQuery());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error navigating back: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        #endregion Default Methods

        #region Variables

        [Parameter] public Guid? Id { get; set; }
        [Parameter] public string? PageMode { get; set; }
        private List<GroupDto> Groups { get; set; } = new List<GroupDto>();
        private List<GroupMenuDto> GroupMenus { get; set; } = new List<GroupMenuDto>();
        private List<MenuDto> Menus { get; set; } = new List<MenuDto>();
        private GroupDto Group { get; set; } = new GroupDto();
        private GroupMenuDto GroupMenu { get; set; } = new GroupMenuDto();

        #endregion Variables

        #region On Import

        protected override async Task OnParametersSetAsync()
        {
            // Periksa apakah PageMode diatur
            if (!string.IsNullOrWhiteSpace(PageMode))
            {
                // Cek apakah PageMode adalah Create
                if (PageMode == EnumPageMode.Create.GetDisplayName())
                {
                    // Periksa apakah URL saat ini tidak diakhiri dengan mode Create
                    if (!PageName.EndsWith($"/{EnumPageMode.Create.GetDisplayName()}", StringComparison.OrdinalIgnoreCase))
                    {
                        // Redirect ke URL dengan mode Create
                        NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Create.GetDisplayName()}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                    else
                    {
                        InitializeNew(true);
                    }
                }
                // Cek apakah PageMode adalah Update
                else if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    // Logika untuk update
                    if (Id.HasValue)
                    {
                        await LoadDataByIdAsync(Id.GetValueOrDefault());
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"{Url}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                }
            }
        }

        private List<ExportFileData> ExportFileDatasGroupMenus =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            }
        ];

        private List<ExportFileData> ExportFileDatasGroup =
        [
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            }
        ];

        public async Task ImportExcelFileGroupMenu(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = ExportFileDatasGroupMenus.Select(x => x.Column).ToList();

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var gg = new List<GroupMenuDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        bool IsValid = true;
                        var aa = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var menuId = Menus.FirstOrDefault(x => (x.ParentId != null && x.ParentId != Guid.Empty) && x.Name == aa)?.Id ?? Guid.Empty;

                        if (menuId == Guid.Empty)
                        {
                            ToastService.ShowErrorImport(row, 1, ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty);
                            IsValid = false;
                        }

                        if (!IsValid)
                            continue;

                        var g = new GroupMenuDto
                        {
                            GroupId = Group.Id,
                            MenuId = menuId
                        };

                        if (!GroupMenus.Any(x => x.GroupId == g.GroupId && x.MenuId == g.MenuId))
                            gg.Add(g);
                    }

                    await Mediator.Send(new CreateListGroupMenuRequest(gg));

                    await LoadDataGroupMenuAsync();

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        public async Task ImportExcelFileGroup(InputFileChangeEventArgs e)
        {
            PanelVisible = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = ExportFileDatasGroup.Select(x => x.Column).ToList();

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var gg = new List<GroupDto>();

                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var g = new GroupDto
                        {
                            Name = ws.Cells[row, 1].Value?.ToString()?.Trim() ?? string.Empty
                        };

                        if (!Groups.Any(x => x.Name.Trim().ToLower() == g?.Name?.Trim().ToLower()))
                            gg.Add(g);
                    }

                    await Mediator.Send(new CreateListGroupRequest(gg));

                    await LoadDataAsync();

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }
            PanelVisible = false;
        }

        #endregion On Import
    }
}