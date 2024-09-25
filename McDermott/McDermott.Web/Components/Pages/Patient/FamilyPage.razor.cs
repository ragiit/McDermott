using Microsoft.AspNetCore.HttpLogging;
using System.ComponentModel.DataAnnotations;

namespace McDermott.Web.Components.Pages.Patient
{
    public partial class FamilyPage
    {
        private bool PanelVisible { get; set; } = true;

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

        public IGrid Grid { get; set; }
        private List<FamilyDto> Familys = new();
        private List<FamilyDto> InverseRelations = new();
        private string? relation { get; set; } = string.Empty;
        private string? name { get; set; } = string.Empty;

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetUserInfo();
            await LoadData();
            await LoadDataFamily();
        }

        #region ComboboxFamily

        private DxComboBox<FamilyDto, long?> refFamilyComboBox { get; set; }
        private int FamilyComboBoxIndex { get; set; } = 0;
        private int totalCountFamily = 0;

        private async Task OnSearchFamily()
        {
            await LoadDataFamily();
        }

        private async Task OnSearchFamilyIndexIncrement()
        {
            if (FamilyComboBoxIndex < (totalCountFamily - 1))
            {
                FamilyComboBoxIndex++;
                await LoadDataFamily(FamilyComboBoxIndex, 10);
            }
        }

        private async Task OnSearchFamilyndexDecrement()
        {
            if (FamilyComboBoxIndex > 0)
            {
                FamilyComboBoxIndex--;
                await LoadDataFamily(FamilyComboBoxIndex, 10);
            }
        }

        private async Task OnInputFamilyChanged(string e)
        {
            FamilyComboBoxIndex = 0;
            await LoadDataFamily();
        }

        private async Task LoadDataFamily(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetFamilyQuery(x => x.InverseRelationId == null, pageIndex: pageIndex, pageSize: pageSize, searchTerm: refFamilyComboBox?.Text ?? ""));
            InverseRelations = result.Item1;
            totalCountFamily = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxFamily

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

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.Send(new GetFamilyQuery(searchTerm: searchTerm, pageSize: pageSize, pageIndex: pageIndex));
                Familys = a.Item1;
                totalCount = a.pageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task NewItem_Click()
        {
            relation = string.Empty;
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);

            var a = (Grid.GetDataItem(FocusedRowVisibleIndex) as FamilyDto ?? new());
            PanelVisible = true;

            var resultz = await Mediator.Send(new GetFamilyQuery(x => x.Id == a.InverseRelationId));
            InverseRelations = resultz.Item1;
            totalCountFamily = resultz.pageCount;

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
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    var editModel = ((FamilyDto)e.DataItem);
                    if (editModel.InverseRelationId is not null)
                    {
                        var f = (await Mediator.Send(new GetFamilyQuery(x => x.Id == editModel.InverseRelationId))).Item1.FirstOrDefault();
                        if (f is not null)
                        {
                            f.InverseRelationId = null;
                            await Mediator.Send(new UpdateFamilyRequest(f));
                        }
                    }

                    await Mediator.Send(new DeleteFamilyRequest(editModel.Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<FamilyDto>>();

                    foreach (var editModel in a)
                    {
                        if (editModel.InverseRelationId is not null)
                        {
                            var f = (await Mediator.Send(new GetFamilyQuery(x => x.Id == editModel.InverseRelationId))).Item1.FirstOrDefault();
                            if (f is not null)
                            {
                                f.InverseRelationId = null;
                                await Mediator.Send(new UpdateFamilyRequest(f));
                            }
                        }
                    }

                    await Mediator.Send(new DeleteFamilyRequest(ids: a.Select(x => x.Id).ToList()));
                }

                await LoadData();
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
                //var editModel = (FamilyDto)e.EditModel;
                //name = editModel.Name;

                ////var invers = Familys.Where(x => x.Name == relation).Select(x => x.Id).FirstOrDefault();

                //if (relation == null || relation == "")
                //{
                //    if (string.IsNullOrWhiteSpace(editModel.Name))
                //        return;

                //    editModel.ParentRelation = editModel.Name;

                //    if (editModel.Id == 0)
                //    {
                //        await Mediator.Send(new CreateFamilyRequest(editModel));
                //    }
                //    else
                //        await Mediator.Send(new UpdateFamilyRequest(editModel));
                //}
                //else
                //{
                //    editModel.Relation = relation + "-" + name;
                //    if (string.IsNullOrWhiteSpace(editModel.Name))
                //        return;

                //    if (editModel.Id == 0)
                //    {
                //        editModel.ParentRelation = name;
                //        editModel.ChildRelation = relation;
                //        await Mediator.Send(new CreateFamilyRequest(editModel));

                //        var invers = Familys.Where(x => x.Name == relation).Select(x => x.Id).FirstOrDefault();

                //        editModel.Id = invers;
                //        editModel.Name = relation;
                //        editModel.Relation = name + "-" + relation;
                //        editModel.ChildRelation = name;
                //        editModel.ParentRelation = relation;
                //        await Mediator.Send(new UpdateFamilyRequest(editModel));
                //    }
                //    else
                //    {
                //        await Mediator.Send(new UpdateFamilyRequest(editModel));
                //    }
                //}
                //relations.Clear();

                var editModel = (FamilyDto)e.EditModel;

                bool validate = await Mediator.Send(new ValidateFamilyQuery(x => x.Id != editModel.Id && x.Name == editModel.Name));
                if (validate)
                {
                    ToastService.ShowInfo($"Family Relation with name '{editModel.Name}' is already exists");
                    e.Cancel = true;
                    return;
                }

                if (editModel.Id == 0)
                {
                    editModel = await Mediator.Send(new CreateFamilyRequest(editModel));
                }
                else
                {
                    editModel = await Mediator.Send(new UpdateFamilyRequest(editModel));
                }

                if (editModel.InverseRelationId is not null)
                {
                    var f = (await Mediator.Send(new GetFamilyQuery(x => x.Id == editModel.InverseRelationId))).Item1.FirstOrDefault();
                    if (f is not null)
                    {
                        f.InverseRelationId = editModel.Id;
                        await Mediator.Send(new UpdateFamilyRequest(f));
                    }
                }

                await LoadDataFamily();
                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }
    }
}