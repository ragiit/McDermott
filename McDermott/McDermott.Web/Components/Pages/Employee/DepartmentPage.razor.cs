namespace McDermott.Web.Components.Pages.Employee
{
    public partial class DepartmentPage
    {
        public List<DepartmentDto> Departments = [];
        public List<DepartmentDto> AllParentDepartments = [];
        public List<DepartmentDto> ParentDepartments = [];
        public List<CompanyDto> Companies = [];
        private List<UserDto> AllUsers = [];
        private List<UserDto> Users = [];

        private long UpdateUserId { get; set; }
        private long? SelectedUserId { get; set; }

        private List<string> DepartmentCategories = new()
        {
            "Department",
            "Unit",
        };

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            PanelVisible = true;

            SelectedDataItems = new ObservableRangeCollection<object>();
            Companies = await Mediator.Send(new GetCompanyQuery());

            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            Users = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true));
            Departments = await Mediator.Send(new GetDepartmentQuery());
            ParentDepartments = await Mediator.Send(new GetDepartmentQuery(x => x.ParentDepartmentId == null));
            AllParentDepartments = [.. ParentDepartments];

            //Departments.ForEach(x =>
            //{
            //    var n = AllUsers.FirstOrDefault(z => z.DepartmentId == x.Id);
            //    if (n is not null)
            //        x.Manager = n.Name;
            //});

            PanelVisible = false;
        }

        private bool IsAccess = false;

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

        #region Default Grid

        private GroupMenuDto UserAccessCRUID = new();
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteDepartmentRequest(((DepartmentDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<DepartmentDto>>();
                    await Mediator.Send(new DeleteDepartmentRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var editModel = (DepartmentDto)e.EditModel;

                //if (editModel.ParentId is not null)
                //    editModel.ParentName = Departments.FirstOrDefault(x => x.Id == editModel.ParentId).Name;

                if (editModel.Id == 0)
                {
                    var result = await Mediator.Send(new CreateDepartmentRequest(editModel));
                    editModel.Id = result.Id;
                }
                else
                {
                    var user = await Mediator.Send(new GetUserQuery(x => x.Id == UpdateUserId));

                    if (user.Count > 0)
                    {
                        user[0].DepartmentId = null;
                        await Mediator.Send(new UpdateUserRequest(user[0]));
                    }

                    await Mediator.Send(new UpdateDepartmentRequest(editModel));
                }

                //if (SelectedUserId != 0)
                //{
                //    var user = await Mediator.Send(new GetUserQuery(x => x.Id == UpdateUserId));
                //    if (user.Count > 0)
                //    {
                //        user[0].DepartmentId = editModel.Id;
                //        await Mediator.Send(new UpdateUserRequest(user[0]));
                //    }
                //}

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

        private async Task NewItem_Click()
        {
            ParentDepartments = AllParentDepartments.ToList();
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            ParentDepartments = AllParentDepartments.Where(x => x.Id != SelectedDataItems[0].Adapt<DepartmentDto>().Id).ToList();
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);

            //try
            //{
            //    long departmentId = SelectedDataItems[0].Adapt<DepartmentDto>().Id;
            //    var user = AllUsers.FirstOrDefault(x => x.DepartmentId == departmentId);

            //    Users = AllUsers
            //        .Where(x => x.Id == user?.Id && departmentId == x.DepartmentId || x.DepartmentId == null)
            //        .OrderBy(x => x.Name)
            //        .ToList();

            //    UpdateUserId = user?.Id ?? 0;

            //    SelectedUserId = user?.Id ?? 0;
            //}
            //catch { }

            return;

            try
            {
                long departmentId = SelectedDataItems[0].Adapt<DepartmentDto>().Id;
                var user = AllUsers.FirstOrDefault(x => x.DepartmentId == departmentId);

                if (user is null)
                {
                    Users = [.. Users.Where(x => x.DepartmentId == null).OrderBy(x => x.Name)];
                    SelectedUserId = 0;
                    return;
                }

                var a = AllUsers;

                var users = new List<UserDto>();
                foreach (var item in a)
                {
                    if (item.Id == user.Id && departmentId == item.DepartmentId)
                        users.Add(item);

                    if (item.DepartmentId == null)
                        users.Add(item);
                }

                Users = users;

                SelectedUserId = user.Id;
            }
            catch { }
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

        #endregion Default Grid
    }
}