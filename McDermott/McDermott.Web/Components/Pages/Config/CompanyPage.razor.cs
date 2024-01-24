using static McDermott.Application.Features.Commands.CityCommand;
using static McDermott.Application.Features.Commands.CountryCommand;
using static McDermott.Application.Features.Commands.ProvinceCommand;
using static McDermott.Application.Features.Commands.CompanyCommand;
using Blazored.LocalStorage;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CompanyPage
    {
        public IGrid Grid { get; set; }
        private List<CompanyDto> Companys = new();
        private CompanyDto CompanyForm = new();
        private IReadOnlyList<object>? SelectedDataItems { get; set; }
        private bool EditItemsEnabled { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool OnVacation { get; set; } = true;
        private bool ShowForm { get; set; } = false;
        public List<CountryDto> Countries { get; set; }
        public List<ProvinceDto> Provinces { get; set; }
        public List<CityDto> Cities { get; set; }
        // public List<CurrencyDto> Currencys {get; private set;}

        private async Task LoadData()
        {
            ShowForm = false;
            Companys = await Mediator.Send(new GetCompanyQuery());
        }

        protected override async Task OnInitializedAsync()
        {
            Countries = await Mediator.Send(new GetCountryQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());

            await LoadData();
        }

        private async Task OnSave()
        {
            var a = CompanyForm;

            if (CompanyForm.Id == 0)
                await Mediator.Send(new CreateCompanyRequest(CompanyForm));
            else
                await Mediator.Send(new CreateCompanyRequest(CompanyForm));

            await LoadData();
        }

        private void OnCancel()
        {
            CompanyForm = new();
            ShowForm = false;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private async Task onChangeCountry(ChangedEventArgs args)
        {
            if (args.NewValue != null)
            {
                CountryDto selectedCountry = (CountryDto)args.NewValue;
                // Update the list of cities based on the selected province
                //Cities = await Mediator.Send(new GetProvinceByCountry());
            }

        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteCompanyRequest(((CompanyDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<CompanyDto>>();
                await Mediator.Send(new DeleteListCompanyRequest(a.Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

        private void OnItemUpdating(string fieldName, object newValue)
        {
            if (fieldName == nameof(CompanyForm.Name))
            {
                CompanyForm.Name = newValue.ToString();
            }
        }

        private async Task NewItem_Click()
        {
            ShowForm = true;
        }

        private async Task EditItem_Click()
        {
            var a = FocusedRowVisibleIndex;
            //await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
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

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }
    }
}