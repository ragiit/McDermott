using DevExpress.Data.XtraReports.Native;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.Config.EmailTemplateCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class EmailTemplatePage
    {
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private bool IsAccess { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool PopupVisible { get; set; } = false;
        private string textPopUp = "";
        private DateTime DateTimeValue { get; set; } = DateTime.Now;
        public IGrid Grid { get; set; }
        private List<EmailTemplateDto> EmailTemplates = new();
        private EmailTemplateDto EmailFormTemplate = new();
        private GroupMenuDto UserAccessCRUID = new();

        private User? User = new();
        private string? userBy;
        private List<UserDto> ToPartner;
        private List<string>? Cc;
        private IEnumerable<UserDto> CcBy = [];

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await NavigationManager.CheckAccessUser(oLocal);
                IsAccess = result.Item1;
                UserAccessCRUID = result.Item2;
            }
            catch { }
            //var by =

            EmailTemplates = await Mediator.Send(new GetEmailTemplateQuery());
            await LoadData();
        }

        private async Task LoadUser()
        {
            try
            {
                EmailFormTemplate.Schendule = DateTime.Now;
                var Partner = await Mediator.Send(new GetUserQuery());
                Cc = [.. Partner.Select(x => x.Email)];
                ToPartner = [.. Partner.Where(x => x.IsPatient == true).ToList()];
                User = await oLocal.GetUserInfo();
                userBy = User.Name;
                EmailFormTemplate.ById = User.Id;
                //_isInitComplete = true;
            }
            catch
            {
            }
        }

        private async Task LoadData()
        {
            PopupVisible = false;
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            EmailTemplates = await Mediator.Send(new GetEmailTemplateQuery());
            PanelVisible = false;
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
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

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        private async Task NewItem_Click()
        {
            await LoadUser();
            EmailFormTemplate = new();
            PopupVisible = true;
            textPopUp = "Form Template Email";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await LoadUser();
            EmailFormTemplate = new();
            PopupVisible = true;
            textPopUp = "Edit Form Template Email";
            await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
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

        private async Task OnDelete()
        {
            try
            {
                await LoadData();
            }
            catch { }
        }
    }
}