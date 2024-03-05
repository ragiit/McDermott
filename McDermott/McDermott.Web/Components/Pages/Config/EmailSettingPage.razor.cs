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

        private void HandleValidSubmit()
        {
            FormValidationState = @"Form data is valid";
        }

        private void HandleInvalidSubmit()
        {
            FormValidationState = @"Form data is invalid";
        }

        #endregion Data static

        #region variabel

        private string FormValidationState = @"Press the ""Save"" button to validate the form.";
        private string resultMessage = "";
        private bool? IsConnected { get; set; }
        private bool isLoading { get; set; }

        #endregion variabel

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
            PopUpVisible = false;
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
            FormEmails = new();
            PopUpVisible = true;
            IsConnected = null;
            TextPopUp = "Tambah Data";
        }

        private async Task EditItem_Click()
        {
            try
            {
                var general = SelectedDataItems[0].Adapt<EmailSettingDto>();
                FormEmails = general;

                PopUpVisible = true;
                TextPopUp = "Edit Data";
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task OnCancel()
        {
            PopUpVisible = false;
            await LoadData();
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

        private async Task OnSave()
        {
            if (FormEmails.Status == "")
            {
                FormEmails.Status = "No Testing";
            }

            if (FormEmails.Id == 0)
                await Mediator.Send(new CreateEmailSettingRequest(FormEmails));
            else
                await Mediator.Send(new UpdateEmailSettingRequest(FormEmails));

            await LoadData();
        }

        #endregion Save Function

        private string currentSmtpEncryption = "";

        private void SelectedItemChanged(string e)
        {
            if (e is null)
            {
                return;
            }
            if (e.Equals("TLS (STARTTLS)"))
            {
                FormEmails.Smtp_Port = "25";
            }
            else if (e.Equals("SSL/TLS"))
            {
                FormEmails.Smtp_Port = "465";
            }
            else if (e.Equals("none"))
            {
                FormEmails.Smtp_Port = "";
            }
        }

        private string OnSmtpEncryptionChange
        {
            get => currentSmtpEncryption;
            set
            {
                currentSmtpEncryption = value;
                FormEmails.Smtp_Encryption = value;
                if (currentSmtpEncryption == "TLS (STARTTLS)")
                {
                    FormEmails.Smtp_Port = "25";
                }
                else if (currentSmtpEncryption == "SSL/TLS")
                {
                    FormEmails.Smtp_Port = "465";
                }
                else if (currentSmtpEncryption == "none")
                {
                    FormEmails.Smtp_Port = "";
                }
            }
        }

        private async Task TestConnect()
        {
            try
            {
                var Port = int.Parse(FormEmails.Smtp_Port);
                isLoading = true;
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(FormEmails.Smtp_Host, Port, MailKit.Security.SecureSocketOptions.Auto);

                    if (client.IsConnected)
                    {
                        await client.AuthenticateAsync(FormEmails.Smtp_User, FormEmails.Smtp_Pass);
                        IsConnected = true;
                        ToastService.ShowSuccess("Connection Success");
                        FormEmails.Status = "Connected!";
                    }
                }
                isLoading = false;
            }
            catch (Exception ex)
            {
                isLoading = true;
                Console.WriteLine(ex.Message);
                IsConnected = false;
                ToastService.ShowError("Connection Failed!!!");
                isLoading = false;
            }
        }
    }
}