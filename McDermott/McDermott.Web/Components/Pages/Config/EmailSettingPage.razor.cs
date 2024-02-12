using DevExpress.Data.XtraReports.Native;
using McDermott.Application.Dtos.Config;
using McDermott.Web.Extentions;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;
using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class EmailSettingPage
    {
        #region Relatio Data

        private List<EmailSettingDto> EmailSettings = new();
        private EmailSettingDto FormEmails = new();

        #endregion Relatio Data

        #region Auth

        private bool IsAccess = false;
        private GroupMenuDto UserAccessCRUID = new();

        #endregion Auth

        private readonly SmtpClient? _smtpClient;

        #region Grid Setting

        private bool PanelVisible { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;

        private string TextPopUp = "";
        private bool ShowForm { get; set; } = false;

        public IGrid Grid { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();

        private int FocusedRowVisibleIndex { get; set; }
        private bool EditItemsEnabled { get; set; }

        #endregion Grid Setting

        #region Data static

        private List<string> Stts_Ecrypt = new List<string>
        {
            "none",
            "TLS (STARTTLS)",
            "SSL/TLS"
        };

        #endregion Data static

        #region Async Data And Auth Render

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
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            SelectedDataItems = new ObservableRangeCollection<object>();
            EmailSettings = await Mediator.Send(new GetEmailSettingQuery());
            PanelVisible = false;
        }

        #endregion Async Data And Auth Render

        #region Config Grid

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

        #endregion Config Grid

        #region Button Setting

        private async Task NewItem_Click()
        {
            PopUpVisible = true;
            TextPopUp = "Tambah Data";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            PopUpVisible = false;
            TextPopUp = "Edit Data";
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

        #endregion Button Setting

        #region function Delete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            if (SelectedDataItems is null)
            {
                await Mediator.Send(new DeleteEmailSettingRequest(((EmailSettingDto)e.DataItem).Id));
            }
            else
            {
                var a = SelectedDataItems.Adapt<List<EmailSettingDto>>();
                await Mediator.Send(new DeleteListEmailSettingRequest(a.Select(x => x.Id).ToList()));
            }
            await LoadData();
        }

        #endregion function Delete

        #region Save Function

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            var editModel = (EmailSettingDto)e.EditModel;

            if (string.IsNullOrWhiteSpace(editModel.Description))
                return;

            if (editModel.Id == 0)
                await Mediator.Send(new CreateEmailSettingRequest(editModel));
            else
                await Mediator.Send(new UpdateEmailSettingRequest(editModel));

            await LoadData();
        }

        #endregion Save Function

        private async Task TestConnect()
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient(FormEmails.Smpt_Host, FormEmails.Smpt_Port))
                {
                    smtp.Credentials = new NetworkCredential(FormEmails.Smpt_User, FormEmails.Smpt_Pass);
                    smtp.EnableSsl = false;
                    //smtp.Send();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}