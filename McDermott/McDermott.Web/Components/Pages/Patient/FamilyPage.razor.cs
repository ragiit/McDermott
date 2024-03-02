namespace McDermott.Web.Components.Pages.Patient
{
    public partial class FamilyPage
    {
        private bool PanelVisible { get; set; } = true;

        public IGrid Grid { get; set; }
        private List<FamilyDto> Familys = new();
        private List<FamilyDto> relations = new();
        private string? relation { get; set; } = string.Empty;
        private string? name { get; set; } = string.Empty;

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            Familys = await Mediator.Send(new GetFamilyQuery());
            relations = [.. Familys.Where(x => x.Relation == null || x.Relation == "").ToList()];
            PanelVisible = false;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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
            relation = string.Empty;
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            relation = string.Empty;
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

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteFamilyRequest(((FamilyDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<FamilyDto>>();
                await Mediator.Send(new DeleteListFamilyRequest(a.Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (FamilyDto)e.EditModel;
            name = editModel.Name;

            //var invers = Familys.Where(x => x.Name == relation).Select(x => x.Id).FirstOrDefault();

            if (relation == null || relation == "")
            {
                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                editModel.ParentRelation = editModel.Name;

                if (editModel.Id == 0)
                {
                    await Mediator.Send(new CreateFamilyRequest(editModel));
                }
                else
                    await Mediator.Send(new UpdateFamilyRequest(editModel));
            }
            else
            {
                editModel.Relation = relation + "-" + name;
                if (string.IsNullOrWhiteSpace(editModel.Name))
                    return;

                if (editModel.Id == 0)
                {
                    editModel.ParentRelation = name;
                    editModel.ChildRelation = relation;
                    await Mediator.Send(new CreateFamilyRequest(editModel));

                    var invers = Familys.Where(x => x.Name == relation).Select(x => x.Id).FirstOrDefault();

                    editModel.Id = invers;
                    editModel.Name = relation;
                    editModel.Relation = name + "-" + relation;
                    editModel.ChildRelation = name;
                    editModel.ParentRelation = relation;
                    await Mediator.Send(new UpdateFamilyRequest(editModel));
                }
                else
                {
                    await Mediator.Send(new UpdateFamilyRequest(editModel));
                }
            }
            relations.Clear();
            await LoadData();
        }
    }
}