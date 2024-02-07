using static McDermott.Application.Features.Commands.Config.CityCommand;
using static McDermott.Application.Features.Commands.Config.CountryCommand;
using static McDermott.Application.Features.Commands.Config.ProvinceCommand;
using static McDermott.Application.Features.Commands.Config.CompanyCommand;
using Blazored.LocalStorage;
using System.ComponentModel.DataAnnotations;
using DevExpress.Data.XtraReports.Native;
using McDermott.Web.Components.Layout;
using McDermott.Domain.Entities;
using McDermott.Application.Dtos.Config;
using McDermott.Application.Dtos.Employee;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class CompanyPage
    {
        private CompanyDto CompanyForm = new();

        public List<CompanyDto> Companys = new();
        public CompanyDto formCompanies = new();
        public CompanyDto DetailCompanies = new();
        public List<CountryDto> Countries { get; set; }
        public List<ProvinceDto> Provinces { get; set; }
        public List<CityDto> Cities { get; set; }
        private GroupMenuDto UserAccessCRUID = new();

        #region Default Grid Components

        private bool showForm { get; set; } = false;
        private bool isDetail { get; set; } = false;
        private string textPopUp = "";
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private int FocusedRowVisibleIndex { get; set; }
        private bool PanelVisible { get; set; } = true;

        private async Task LoadData()
        {
            showForm = false;
            PanelVisible = true;
            Companys = await Mediator.Send(new GetCompanyQuery());
            //DetailCompanies = [.. Companys.ToList()];
            PanelVisible = false;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }

            SelectedDataItems = new ObservableRangeCollection<object>();
            Countries = await Mediator.Send(new GetCountryQuery());
            Provinces = await Mediator.Send(new GetProvinceQuery());
            Cities = await Mediator.Send(new GetCityQuery());

            await LoadData();
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

        private void OnCancel()
        {
            formCompanies = new();
            showForm = false;
            isDetail = false;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            showForm = false;
            isDetail = true;
            var company = SelectedDataItems[0].Adapt<CompanyDto>();
            DetailCompanies = company;
        }

        private async Task OnSave()
        {
            try
            {
                var editModel = formCompanies;

                if (editModel.Id == 0)
                    await Mediator.Send(new CreateCompanyRequest(editModel));
                else
                    await Mediator.Send(new UpdateCompanyRequest(editModel));

                await LoadData();
            }
            catch { }
        }

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
                    await Mediator.Send(new DeleteCompanyRequest(((CompanyDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<CompanyDto>>();
                    await Mediator.Send(new DeleteListCompanyRequest(a.Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch
            {
            }
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
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
            isDetail = false;
            showForm = true;
            textPopUp = "Add Data Companies";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            var company = SelectedDataItems[0].Adapt<CompanyDto>();
            formCompanies = company;
            showForm = true;
            isDetail = false;
            textPopUp = "Edit Data Companies";
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

        #endregion Default Grid Components
    }
}