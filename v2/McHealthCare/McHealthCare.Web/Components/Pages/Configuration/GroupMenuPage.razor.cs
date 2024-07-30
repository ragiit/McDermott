using McHealthCare.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Internal;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.GroupCommand;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.GroupMenuCommand;
using static McHealthCare.Application.Features.CommandsQueries.Configuration.MenuCommand;
using static McHealthCare.Extentions.EnumHelper;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class GroupMenuPage
    {
        #region Default Variables 
        private IGrid Grid { get; set; }
        private IGrid GridGroupMenu { get; set; }
        private bool PanelVisible { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndex2 { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsGroupMenu { get; set; } = [];

        #endregion

        #region Default Methods

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        private async Task LoadDataGroupMenu()
        {
            GroupMenus = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == Group.Id));
        }
        private async Task LoadData()
        {
            PanelVisible = true;
            Groups = await Mediator.Send(new GetGroupQuery());
            Menus = await Mediator.Send(new GetMenuQuery());
            PanelVisible = false;
        }
        private async Task LoadDataById(Guid id)
        {
            Group = (await Mediator.Send(new GetGroupQuery(x => x.Id == id))).FirstOrDefault() ?? new();
            await LoadDataGroupMenu();

            if (Group.Id == Guid.Empty)
            {
                NavigationManager.NavigateTo(Url);
            }
            else
            {
            }
        }
        private void InitializeNew(bool isParam = false)
        {
            Group = new();
            GroupMenus = [];

            if (!isParam)
                NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Create.GetDisplayName()}");
        }
        private async Task InitializeEdit()
        {
            NavigationManager.NavigateTo(Url);
            await LoadDataById(SelectedDataItems[0].Adapt<GroupDto>().Id);
        } 
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteGroupRequest(((GroupDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GroupDto>>();
                    await Mediator.Send(new DeleteGroupRequest(Ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }
        private async Task HandleValidSubmit()
        {
            if (Group.Id == Guid.Empty)
                Group = await Mediator.Send(new CreateGroupRequest(Group));
            else
                Group = await Mediator.Send(new UpdateGroupRequest(Group));

            NavigationManager.NavigateTo($"{Url}/{EnumPageMode.Update.GetDisplayName()}/{Group.Id}");
        }
        private async Task HandleInvalidSubmit() { }
        private async Task OnSaveGroupMenu(GridEditModelSavingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                var editModel = (GroupMenuDto)e.EditModel;

                editModel.GroupId = Group.Id;

                if (editModel.Id == Guid.Empty)
                    await Mediator.Send(new CreateGroupMenuRequest(editModel));
                else
                    await Mediator.Send(new UpdateGroupMenuRequest(editModel));

                await LoadDataGroupMenu();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }
        private async Task OnDeleteGroupMenu(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteGroupMenuRequest(((GroupMenuDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GroupMenuDto>>();
                    await Mediator.Send(new DeleteGroupMenuRequest(Ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                PanelVisible = false;
            }
        }
        private void CancelItemGroupMenuGrid_Click()
        {

        }
        private async Task NewItemGroup_Click()
        {
            GroupMenu = new();
            await GridGroupMenu.StartEditNewRowAsync();
        }
        #endregion

        #region Variables
        private string Url => Helper.URLS.FirstOrDefault(x => x == "configuration/groups") ?? "";
        [Parameter]
        public Guid? Id { get; set; }
        [Parameter]
        public string? PageMode { get; set; }
        public bool IsLoading { get; set; } = false;
        private List<GroupDto> Groups { get; set; } = [];
        private List<GroupMenuDto> GroupMenus { get; set; } = [];
        private List<MenuDto> Menus { get; set; } = [];
        private GroupDto Group { get; set; } = new();
        private GroupMenuDto GroupMenu { get; set; } = new();
        private string PageName => new Uri(NavigationManager.Uri).PathAndQuery.Replace(NavigationManager.BaseUri, "/");
        #endregion

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
                        await LoadDataById(Id.GetValueOrDefault());
                    }
                    else
                    {
                        NavigationManager.NavigateTo($"{Url}", true);
                        return; // Pastikan kode berikutnya tidak dieksekusi
                    }
                }
            }
        }

    }
}
