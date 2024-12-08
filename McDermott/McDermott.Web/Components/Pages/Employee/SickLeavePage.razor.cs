using McDermott.Extentions;
using MimeKit;
using static McDermott.Application.Features.Commands.Config.EmailEmailTemplateCommand;
using static McDermott.Application.Features.Commands.Config.EmailSettingCommand;
using static McDermott.Application.Features.Commands.Employee.SickLeaveCommand;

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
        private SickLeaveDto fSickLeave = new();

        #endregion Relation Data

        #region Static Variabel

        [Parameter]
        public long Id { get; set; }

        public byte[]? DocumentContent;
        private IGrid Grid { get; set; }
        private Timer _timer;
        private bool IsLoading { get; set; } = false;
        private bool PanelVisible { get; set; }
        private bool isPrint { get; set; } = false;
        private bool isShow { get; set; } = false;
        private bool Employee { get; set; } = false;
        private string? TypeLeaves { get; set; }
        private string? YesOrNoEmployee { get; set; } = "No";
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
                var user = await UserInfoService.GetUserInfo(ToastService);
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadData(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadData(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadData(newPageIndex, pageSize);
        }

        #endregion Searching

        #region async Data

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            await GetUserInfo();
            PanelVisible = false;

            return;

            try
            {
                _timer = new Timer(async (_) => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

                await GetUserInfo();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoading = true;

                SickLeaves = await Mediator.Send(new GetSickLeaveQuery());
                Users = await Mediator.Send(new GetUserQuery());
                CPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery());
                var Diagnoses = (await Mediator.Send(new GetDiagnosisQuery())).Item1;
                this.Diagnoses = Diagnoses;

                foreach (var item in SickLeaves)
                {
                    var diagnosis = "";
                    TypeLeaves = item.TypeLeave;
                    var generalConsultans = (await Mediator.Send(new GetGeneralConsultanServiceQuery())).Item1;
                    var BodyCPPT = CPPTs.Where(x => x.GeneralConsultanServiceId == item.GeneralConsultans.Id && x.Title == "Diagnosis").Select(x => x.Body).FirstOrDefault();

                    if (BodyCPPT != null)
                    {
                        diagnosis = Diagnoses.Where(x => BodyCPPT!.Contains(x.Name)).Select(x => x.Name).FirstOrDefault();
                        item.Diagnosis = diagnosis;
                    }

                    var cek = generalConsultans.Where(x => x.Id == item.GeneralConsultansId).FirstOrDefault();
                    item.PhycisianName = Users.Where(x => x.Id == cek.PratitionerId).Select(x => x.Name).FirstOrDefault();
                    item.isEmployee = Users.Where(x => x.Id == cek.PatientId).Select(x => x.IsEmployee).FirstOrDefault();

                    if (item.isEmployee == true)
                    {
                        item.YesOrNoEmployee = "Yes";
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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            try
            {
                if ((SickLeaveDto)args.DataItem is null)
                    return;

                Employee = ((SickLeaveDto)args.DataItem)!.isEmployee == true;
            }
            catch { }
        }

        //private void Grid_FocusedRowChangedDetail(GridFocusedRowChangedEventArgs args)
        //{
        //    FocusedRowVisibleIndex = args.VisibleIndex;
        //}

        private async Task ConfirmAndSendEmail()
        {
            isShow = true;
        }

        private void Cancel()
        {
            isShow = false;
        }

        #endregion Grid Configuration

        #region Click

        private async void PrintToLeave(long? grid)
        {
            try
            {
                IsLoading = true;
                var data = SickLeaves.Where(x => x.GeneralConsultans?.PatientId == grid).FirstOrDefault()!;
                var patienss = Users.Where(x => x.Id == grid).FirstOrDefault();
                var age = 0;
                if (data.GeneralConsultans?.Patient.DateOfBirth == null)
                {
                    age = 0;
                }
                else
                {
                    age = DateTime.Now.Year - data.GeneralConsultans.Patient.DateOfBirth.Value.Year;
                }
                if (data.GeneralConsultans.IsSickLeave)
                {
                    data.StartSickLeave = data.GeneralConsultans?.StartDateSickLeave;
                    data.EndSickLeave = data.GeneralConsultans?.EndDateSickLeave;
                }
                else if (data.GeneralConsultans.IsMaternityLeave)
                {
                    data.StartSickLeave = data.GeneralConsultans.StartMaternityLeave;
                    data.EndSickLeave = data.GeneralConsultans.EndMaternityLeave;
                }
                int TotalDays = data.EndSickLeave.Value.Day - data.StartSickLeave.Value.Day;

                string WordDays = ConvertNumberHelper.ConvertNumberToWord(TotalDays);
                isPrint = true;
                var mergeFields = new Dictionary<string, string>
                {
                    {"<<NamePatient>>", patienss?.Name},
                    {"<<startDate>>", data?.StartSickLeave?.ToString("dd MMMM yyyy") },
                    {"<<endDate>>", data?.EndSickLeave?.ToString("dd MMMM yyyy") },
                    {"<<NameDoctor>>", data?.PhycisianName },
                    {"<<SIPDoctor>>", data?.SIP },
                    {"<<AddressPatient>>", data?.Address },
                    {"<<AgePatient>>", age.ToString() },
                    {"<<WordDays>>", WordDays },
                    {"<<days>>", TotalDays.ToString() },
                    {"<<Date>>", DateTime.Now.ToString("dd MMMM yyyy")}
                };

                DocumentContent = await DocumentProvider.GetDocumentAsync("SuratIzin.docx", mergeFields);
                // Konversi byte array menjadi base64 string
                var base64String = Convert.ToBase64String(DocumentContent);

                // Panggil JavaScript untuk membuka dan mencetak dokumen
                await JsRuntime.InvokeVoidAsync("printDocument", base64String);
                IsLoading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SendToEmail()
        {
            try
            {
                IsLoading = true;
                // Adapt selected data items to SickLeaveDto list
                List<SickLeaveDto> sickLeavesDtoList = SelectedDataItems.Adapt<List<SickLeaveDto>>();

                foreach (var item in sickLeavesDtoList)
                {
                    // Find corresponding sick leave data
                    var data = SickLeaves.FirstOrDefault(x => x.Id == item.Id);
                    if (data == null) continue;

                    // Find Data Patient
                    var Patientss = Users.Where(x => x.Id == item.GeneralConsultans.PatientId).FirstOrDefault()!;
                    if (Patientss == null) continue;

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
                    int age = Patientss.DateOfBirth.HasValue
                        ? DateTime.Now.Year - Patientss.DateOfBirth.Value.Year
                        : 0;

                    // Calculate total days of sick leave
                    int totalDays = item.EndSickLeave.Value.Day - item.StartSickLeave.Value.Day;
                    string wordDays = ConvertNumberHelper.ConvertNumberToWord(totalDays);

                    data.PatientName = data.GeneralConsultans.Patient.Name;
                    if (data.GeneralConsultans.PratitionerId is not null)
                        data.PhycisianName = data.GeneralConsultans.Pratitioner.Name;
                    data.SIP = data.GeneralConsultans.Pratitioner.SipNo;
                    data.Address = data.GeneralConsultans.Patient.DomicileAddress1;

                    //Gender
                    string Gender = "";
                    string OppositeSex = "";
                    if (Patientss.Gender != null)
                    {
                        Gender = Patientss.Gender == "Male" ? "MAlE(L)" : "FEMALE(P)";
                        OppositeSex = Patientss.Gender == "Male" ? "<strike>F(P)</strike>" : "<strike>M(L)</strike>";
                    }

                    //Days
                    var culture = new System.Globalization.CultureInfo("id-ID");
                    string todays = item.GeneralConsultans.RegistrationDate.ToString("dddd", culture);
                    string GetDefaultValue(string value, string defaultValue = "-")
                    {
                        return value ?? defaultValue;
                    }

                    // Create merge fields for document generation
                    var mergeFields = new Dictionary<string, string>
                    {
                        {"%NamePatient%", data?.PatientName},
                    {"%NIP%", GetDefaultValue(Patientss?.NIP ?? string.Empty) },
                    {"%Departement%", GetDefaultValue(Patientss?.Department?.Name ?? string.Empty)},
                    {"%JobPosition%", GetDefaultValue(Patientss?.JobPosition?.Name ?? string.Empty)},
                        {"%startDate%", item?.StartSickLeave?.ToString("dd MMMM yyyy") },
                        {"%endDate%", item?.EndSickLeave?.ToString("dd MMMM yyyy") },
                        {"%NameDoctor%", data?.PhycisianName },
                        {"%SIPDoctor%", data?.SIP },
                        {"%AddressPatient%", data?.Address },
                        {"%AgePatient%", age.ToString() },
                        {"%Totaldays%", totalDays.ToString() },
                        {"%KataNumber%", wordDays },
                        {"%Days%", todays},
                        {"%Dates%", item.GeneralConsultans.RegistrationDate.ToString("dd MMMM yyyy")},
                        {"%Times%", item.GeneralConsultans.RegistrationDate.ToString("H:MM")},
                        {"%Date%", DateTime.Now.ToString("dd MMMM yyyy")},
                        {"%Genders%", Gender},
                        //{"%oppositeSexs%", OppositeSex},
                    };

                    // Generate document content
                    DocumentContent = await DocumentProvider.GetDocumentAsync("Employee.docx", mergeFields);
                    if (DocumentContent == null) continue;

                    string fileName = $"SickLeave_{data.GeneralConsultans.Patient.Name}_{DateTime.Now:yyyyMMddHHmmss}.docx";

                    var cekEmailTemplate = (await Mediator.Send(new GetEmailTemplateQuery(x => x.TypeEmail == 1))).FirstOrDefault()!;

                    string subject = cekEmailTemplate.Subject;
                    string body = cekEmailTemplate.Message;
                    foreach (var field in mergeFields)
                    {
                        body = body.Replace(field.Key, field.Value);
                    }

                    // Get email settings
                    EmailSettings = await Mediator.Send(new GetEmailSettingQuery());
                    var smtpSettings = EmailSettings.FirstOrDefault(x => x.Id == cekEmailTemplate.EmailFromId);
                    if (smtpSettings == null) continue;

                    // Create email message
                    var message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(smtpSettings.Smtp_User));
                    message.To.Add(MailboxAddress.Parse(data.GeneralConsultans.Patient.Email));
                    // Add Cc recipients
                    if (cekEmailTemplate.Cc is not null)
                    {
                        foreach (var cc in cekEmailTemplate.Cc)
                        {
                            message.Cc.Add(MailboxAddress.Parse(cc.Trim()));
                        }
                    }

                    message.Subject = subject.Replace("%NamePatient%", mergeFields["%NamePatient%"]);
                    var bodyBuilder = new BodyBuilder { HtmlBody = body };
                    bodyBuilder.Attachments.Add(fileName, DocumentContent);
                    message.Body = bodyBuilder.ToMessageBody();

                    // Send email
                    using var smtpClient = new MailKit.Net.Smtp.SmtpClient();

                    var secureSocketOptions = smtpSettings.Smtp_Encryption switch
                    {
                        "SSL/TLS" => MailKit.Security.SecureSocketOptions.SslOnConnect,
                        "TLS (STARTTLS)" => MailKit.Security.SecureSocketOptions.StartTls,
                        _ => MailKit.Security.SecureSocketOptions.None
                    };

                    smtpClient.Connect(smtpSettings.Smtp_Host, int.Parse(smtpSettings.Smtp_Port), secureSocketOptions);
                    smtpClient.Authenticate(smtpSettings.Smtp_User, smtpSettings.Smtp_Pass);
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);

                    item.Status = Application.Extentions.EnumHelper.EnumStatusSickLeave.Send;
                    fSickLeave = item;
                    var aso = await Mediator.Send(new UpdateSickLeaveRequest(fSickLeave));
                    isShow = false;
                    // Show success message
                    await LoadData();
                    ToastService.ClearAll();
                    ToastService.ShowSuccess("Success Send Email!");
                    IsLoading = false;
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