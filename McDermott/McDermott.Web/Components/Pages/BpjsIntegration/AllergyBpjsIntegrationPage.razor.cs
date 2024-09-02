namespace McDermott.Web.Components.Pages.BpjsIntegration
{
    public partial class AllergyBpjsIntegrationPage
    {
        private string SelectedCodeAllergyType { get; set; } = "01";

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private List<AllergyDto> _allergies { get; set; } = new();
        private int parameter1 = 1; // Row data awal yang akan ditampilkan
        private int parameter2 = 10; // Limit jumlah data yang akan ditampilkan
        private int _pageIndex = 0;

        private int FocusedRowVisibleIndex { get; set; }
        private IGrid Grid { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool PanelVisible { get; set; } = false;

        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task EditItem_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is not null && SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteAllergyRequest(((AllergyDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<AllergyDto>>();
                    await Mediator.Send(new DeleteAllergyRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
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
                var editModel = (AllergyDto)e.EditModel;

                if (string.IsNullOrWhiteSpace(editModel.NmAllergy))
                    return;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateAllergyRequest(editModel));
                else
                    await Mediator.Send(new UpdateAllergyRequest(editModel));

                await LoadData();
            }
            catch (Exception ee)
            {
                ee.HandleException(ToastService);
            }
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            SelectedCodeAllergyType = Helper._allergyTypes[0].Name;
            await LoadData();
            IsLoading = false;
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

        private async Task SelectChangeAllergyType(Helper.AllergyType req)
        {
            SelectedCodeAllergyType = req.Code;
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;

            //_allergies = await Mediator.Send(new GetAllergyQuery(x => x.Type == SelectedCodeAllergyType));

            SelectedCodeAllergyType = SelectedCodeAllergyType == "Makanan" ? "01" : SelectedCodeAllergyType?.ToString() ?? string.Empty;

            var response = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"alergi/jenis/{SelectedCodeAllergyType}", HttpMethod.Get);

            if (response.Item2 != 200)
            {
                PanelVisible = false;

                if (response.Item2 == 404)
                {
                    ToastService.ClearErrorToasts();
                    ToastService.ShowError(Convert.ToString(response.Item1));
                }

                _allergies.Clear();

                return;
            }

            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Item1);

            var dynamicList = (IEnumerable<dynamic>)data.list;

            var AllergyList = dynamicList.Select(item => new AllergyDto
            {
                KdAllergy = item.kdAlergi,
                NmAllergy = item.nmAlergi,
                //MaxTarif = item.maxTarif,
                //WithValue = item.withValue
            }).ToList();

            var a = await Mediator.Send(new GetAllergyQuery(x => x.Type == SelectedCodeAllergyType && AllergyList.Select(z => z.KdAllergy).Contains(x.KdAllergy)));

            var existingKdAllergy = a.Select(x => x.KdAllergy).ToHashSet();

            var newAllergies = AllergyList.Where(item => !existingKdAllergy.Contains(item.KdAllergy)).ToList();
            newAllergies.ForEach(x => x.Type = SelectedCodeAllergyType);

            await Mediator.Send(new CreateListAllergyRequest(newAllergies));

            //var combinedAllergyList = a.Concat(newAllergies).ToList();

            _allergies = AllergyList;

            PanelVisible = false;
        }

        private int CalculateParameter1(int pageIndex, int pageSize)
        {
            return (pageIndex - 1) * pageSize + 1;
        }

        private async Task HandlePageIndexChanged(int newIndex)
        {
            parameter1 = newIndex++;
            await LoadData();
        }

        private async Task HandlePageSizeChanged(int newSize)
        {
            parameter2 = newSize;
            await LoadData();
        }
    }
}