using Blazored.TextEditor;
using DevExpress.Data.XtraReports.Native;
using MailKit.Net.Smtp;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MimeKit;
using MimeKit.Text;
using static McDermott.Application.Features.Commands.Config.EmailTemplateCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class EmailTemplatePage
    {
        #region relation Data

        private List<EmailTemplateDto> EmailTemplates = new();
        private EmailTemplateDto EmailFormTemplate = new();
        private IEnumerable<UserDto> CcBy = [];
        private List<UserDto> ToPartner;
        private User? User = new();

        #endregion relation Data

        #region grid configuration

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private BaseAuthorizationLayout AuthorizationLayout = new();
        private GroupMenuDto UserAccessCRUID = new();
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }

        #endregion grid configuration

        #region Variable

        private bool EditItemsEnabled { get; set; }
        private bool IsAccess { get; set; } = false;
        private bool showForm { get; set; } = false;
        private bool PanelVisible { get; set; } = true;
        private bool PopupVisible { get; set; } = false;
        private string textPopUp = "";
        private DateTime DateTimeValue { get; set; } = DateTime.Now;

        private bool[] DocumentContent;

        private string? userBy;

        private List<string>? Cc;

        #endregion Variable

        #region BlazoredTextEditor

        private BlazoredTextEditor richEditor = default!;
        private string toolbar = """"...markup here..."""";
        private string body = """"...markup here..."""";
        private BlazoredTextEditor QuillHtml;
        private BlazoredTextEditor QuillNative;
        private BlazoredTextEditor QuillReadOnly;
        private MarkupString priview;

        private string QuillHTMLContent;
        private string QuillContent;

        private string QuillReadOnlyContent =
            @"<span><b>Read Only</b> <u>Content</u></span>";

        private bool mode = false;

        #endregion BlazoredTextEditor

        #region Async Data

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

        #endregion Async Data

        #region config Grid

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

        #endregion config Grid

        #region function button

        private async Task NewItem_Click()
        {
            await LoadUser();
            EmailFormTemplate = new();
            showForm = true;
            textPopUp = "Form Template Email";
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            await LoadUser();
            EmailFormTemplate = new();
            showForm = true;
            textPopUp = "Edit Form Template Email";
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

        #endregion function button

        private async Task OnDelete()
        {
            try
            {
                await LoadData();
            }
            catch { }
        }

        private async Task SendEmail()
        {
            try
            {
                var host = "smtp.gmail.com";
                var port = int.Parse("587");
                var username = "nuralimajids@gmail.com";
                var pass = "fjzyzqrteviltcub";

                if (!string.IsNullOrWhiteSpace(await QuillHtml.GetText()))
                {
                    priview = (MarkupString)await QuillHtml.GetHTML();
                }
                EmailFormTemplate.Message = priview.ToString();
                EmailFormTemplate.Status = "sending";

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(EmailFormTemplate.From));
                message.To.Add(MailboxAddress.Parse(EmailFormTemplate.To));
                message.Subject = EmailFormTemplate.Subject;
                message.Body = new TextPart(TextFormat.Html) { Text = EmailFormTemplate.Message };

                using var smtp = new SmtpClient();
                smtp.Connect(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(username, pass);
                smtp.Send(message);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }
    }
}