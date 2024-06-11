using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;
using System.Net.Mail;
using static McDermott.Application.Features.Commands.Employee.SickLeaveCommand;
using DocumentFormat.OpenXml.Bibliography;

namespace McDermott.Web.Components.Pages.Employee
{
    public partial class SickLeavePage
    {
        #region Relation Data


        private List<GeneralConsultanServiceDto> generalConsultans = [];
        private List<GeneralConsultanCPPTDto> CPPTs = [];
        private List<DiagnosisDto> Diagnoses = [];
        private List<SickLeaveDto> SickLeaves = [];
        private List<EmailSettingDto> EmailSettings = [];
        private List<UserDto> Users = [];

        #endregion Relation Data

        #region Static Variabel

        [Parameter]
        public long Id { get; set; }

        public byte[] DocumentContent;
        private IGrid Grid { get; set; }
        private bool IsLoading { get; set; } = false;
        private bool isPrint { get; set; } = false;
        private string? TypeLeaves { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Static Variabel

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
                var user = await UserInfoService.GetUserInfo();
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                IsLoading = true;

                SickLeaves = await Mediator.Send(new GetSickLeaveQuery());
                Users = await Mediator.Send(new GetUserQuery());
                CPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery());
                Diagnoses = await Mediator.Send(new GetDiagnosisQuery());

                foreach (var item in SickLeaves)
                {
                    var diagnosis = "";
                    TypeLeaves = item.TypeLeave;
                    var BodyCPPT = CPPTs.Where(x => x.GeneralConsultanServiceId == item.GeneralConsultans.Id && x.Title == "Diagnosis").Select(x => x.Body).FirstOrDefault();
                    if (BodyCPPT != null)
                    {
                        diagnosis = Diagnoses.Where(x => BodyCPPT!.Contains(x.Name)).Select(x => x.Name).FirstOrDefault();

                        item.Diagnosis = diagnosis;
                    }

                }
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion async Data

        #region Grid Configuration

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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
            //await EditItem_Click(null);
        }

        private async Task SendToEmail_click()
        {
            Grid!.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
        #endregion Grid Configuration

        #region Click

        private async void PrintToLeave(long? grid)
        {
            try
            {
                var data = SickLeaves.Where(x => x.PatientId == grid).FirstOrDefault()!;
                var age = 0;
                if (data.brithday == null)
                {
                    age = 0;
                }
                else
                {
                    age = DateTime.Now.Year - data.brithday.Value.Year;
                }
                var days = data.StartSickLeave.Value.Day + data.EndSickLeave.Value.Day;
                isPrint = true;
                var mergeFields = new Dictionary<string, string>
                {
                    {"%NamePatient%", data?.PatientName},
                {"%startDate%", data?.StartSickLeave?.ToString("dd MMMM yyyy") },
                {"%endDate%", data?.EndSickLeave?.ToString("dd MMMM yyyy") },
                {"%NameDoctor%", data?.PhycisianName },
                {"%SIPDoctor%", data?.SIP },
                {"%AddressPatient%", data?.Address },
                {"%AgePatient%", age.ToString() },
                {"%days%", days.ToString() },
                {"%Date%", DateTime.Now.ToString("dd MMMM yyyy")}
                };

                DocumentContent = await DocumentProvider.GetDocumentAsync("SuratIzin.docx", mergeFields);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendToEmail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                // Adapt selected data items to SickLeaveDto list
                List<SickLeaveDto> sickLeavesDtoList = SelectedDataItems.Adapt<List<SickLeaveDto>>();

                foreach (var item in sickLeavesDtoList)
                {
                    // Find corresponding sick leave data
                    var data = SickLeaves.FirstOrDefault(x => x.Id == item.Id);
                    if (data == null) continue;

                    // Update start and end dates based on leave type
                    if (TypeLeaves == "SickLeave")
                    {
                        item.StartSickLeave = item.GeneralConsultans.StartDateSickLeave;
                        item.EndSickLeave = item.GeneralConsultans.EndDateSickLeave;
                    }
                    else if (TypeLeaves == "Maternity")
                    {
                        item.StartSickLeave = item.GeneralConsultans.StartMaternityLeave;
                        item.EndSickLeave = item.GeneralConsultans.EndMaternityLeave;
                    }

                    // Calculate patient's age
                    int age = data.brithday.HasValue
                        ? DateTime.Now.Year - data.brithday.Value.Year
                        : 0;

                    // Calculate total days of sick leave
                    int totalDays = (item.StartSickLeave?.Day ?? 0) + (item.EndSickLeave?.Day ?? 0);

                    // Create merge fields for document generation
                    var mergeFields = new Dictionary<string, string>
        {
            {"%NamePatient%", data?.GeneralConsultans.Patient.Name},
            {"%startDate%", data?.StartSickLeave?.ToString("dd MMMM yyyy") },
            {"%endDate%", data?.EndSickLeave?.ToString("dd MMMM yyyy") },
            {"%NameDoctor%", data?.GeneralConsultans.Pratitioner.Name },
            {"%SIPDoctor%", data?.SIP },
            {"%AddressPatient%", data?.Address },
            {"%AgePatient%", age.ToString() },
            {"%days%", totalDays.ToString() },
            {"%Date%", DateTime.Now.ToString("dd MMMM yyyy")}
        };

                    // Generate document content
                    byte[] documentContent = await DocumentProvider.GetDocumentAsync("SuratIzin.docx", mergeFields);
                    if (documentContent == null) continue;

                    string fileName = $"SickLeave_{DateTime.Now:yyyyMMddHHmmss}.docx";
                    string subject = $"Sick Leave {data.GeneralConsultans.Patient.Name}";
                    string body = $"Dear {data.GeneralConsultans.Patient.Name},<br/><br/>Please find attached your document.<br/><br/>Best regards,<br/>Your Company";

                    // Get email settings
                    EmailSettings = await Mediator.Send(new GetEmailSettingQuery());
                    var smtpSettings = EmailSettings.FirstOrDefault(x => x.Smtp_User == "nuralimajid@matrica.co.id");
                    if (smtpSettings == null) continue;

                    // Create email message
                    var message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(smtpSettings.Smtp_User));
                    message.To.Add(MailboxAddress.Parse(data.GeneralConsultans.Patient.Email));
                    message.Subject = subject;

                    var bodyBuilder = new BodyBuilder { HtmlBody = body };
                    bodyBuilder.Attachments.Add(fileName, documentContent);
                    message.Body = bodyBuilder.ToMessageBody();

                    // Send email
                    using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
                    if (smtpSettings.Smtp_Encryption == "SSL/TLS")
                    {
                        smtpClient.Connect(smtpSettings.Smtp_Host, int.Parse(smtpSettings.Smtp_Port), MailKit.Security.SecureSocketOptions.SslOnConnect);
                        smtpClient.Authenticate(smtpSettings.Smtp_User, smtpSettings.Smtp_Pass);
                        smtpClient.Send(message);
                        smtpClient.Disconnect(true);
                    }

                    // Show success message
                    ToastService.ShowSuccess("Success Send Email!");
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Click
    }
}