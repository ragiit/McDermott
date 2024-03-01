using Blazored.TextEditor;
using DevExpress.Data.XtraReports.Native;
using MailKit.Net.Smtp;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MimeKit;
using MimeKit.Text;
using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;
using static McDermott.Application.Features.Commands.Config.EmailTemplateCommand;

namespace McDermott.Web.Components.Pages.Config
{
    public partial class EmailTemplatePage
    {
        #region relation Data

        private List<EmailTemplateDto> EmailTemplates = new();
        private List<EmailSettingDto> EmailSettings = new();
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
        private MarkupString preview;

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

            //EmailTemplates = await Mediator.Send(new GetEmailTemplateQuery());
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
            showForm = false;
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
        }

        private async Task EditItem_Click()
        {
            await LoadUser();
            showForm = true;
            var general = SelectedDataItems[0].Adapt<EmailTemplateDto>();
            EmailFormTemplate = general;
            textPopUp = "Edit Form Template Email";
        }

        private async Task OnCancle()
        {
            showForm = false;
            await LoadData();
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

        private async Task OnSave()
        {
            try
            {
                if (EmailFormTemplate.Status == "")
                {
                    EmailFormTemplate.Status = "draf";
                }
                if (!string.IsNullOrWhiteSpace(await QuillHtml.GetHTML()))
                {
                    preview = (MarkupString)await QuillHtml.GetHTML();
                }
                EmailFormTemplate.Message = preview.ToString();
                var a = EmailFormTemplate;
                if (EmailFormTemplate.Id == 0)
                {
                    await Mediator.Send(new CreateEmailTemplateRequest(EmailFormTemplate));
                }
                else
                {
                    await Mediator.Send(new UpdateEmailTemplateRequest(EmailFormTemplate));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task SendEmail()
        {
            try
            {
                EmailSettings = await Mediator.Send(new GetEmailSettingQuery());
                var cek = EmailSettings.Where(x => x.Smtp_User == EmailFormTemplate.From).FirstOrDefault();
                var host = cek.Smtp_Host;
                var port = int.Parse(cek.Smtp_Port);
                var pass = cek.Smtp_Pass;
                var user = EmailFormTemplate.From;

                //if (!string.IsNullOrWhiteSpace(await QuillHtml.GetHTML()))
                //{
                preview = (MarkupString)await QuillHtml.GetHTML();
                //}
                EmailFormTemplate.Message = preview.ToString();
                EmailFormTemplate.Status = "sending";

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(EmailFormTemplate.From));
                message.To.Add(MailboxAddress.Parse(EmailFormTemplate.To));
                message.Subject = EmailFormTemplate.Subject;
                message.Body = new TextPart(TextFormat.Html) { Text = EmailFormTemplate.Message };
                //message.Cc.Add(MailboxAddress.Parse(EmailFormTemplate.Cc));
                //message.Body = new TextPart(TextFormat.Html) { Text = EmailFormTemplate.Message };

                using var smtp = new SmtpClient();
                if (cek.Smtp_Encryption == "SSL/TLS")
                {
                    smtp.Connect(host, port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                    smtp.Authenticate(user, pass);
                    smtp.Send(message);
                    smtp.Disconnect(true);
                    ToastService.ShowSuccess("Success Send Email!");
                }
                else if (cek.Smtp_Encryption == "TLS (STARTTLS)")
                {
                    smtp.Connect(host, port, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate(user, pass);
                    smtp.Send(message);
                    smtp.Disconnect(true);
                    ToastService.ShowSuccess("Success Send Email!");
                }
                else
                {
                    smtp.Connect(host, port, MailKit.Security.SecureSocketOptions.None);
                    smtp.Authenticate(user, pass);
                    smtp.Send(message);
                    smtp.Disconnect(true);
                    EmailFormTemplate.Status = "Send";
                    ToastService.ShowSuccess("Success Send Email!");
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }
    }
}