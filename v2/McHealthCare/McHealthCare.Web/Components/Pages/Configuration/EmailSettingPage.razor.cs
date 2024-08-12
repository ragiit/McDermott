using Blazored.Toast.Services;
using McHealthCare.Application.Extentions;
using MediatR;
using Microsoft.AspNetCore.SignalR.Client;

namespace McHealthCare.Web.Components.Pages.Configuration
{
    public partial class EmailSettingPage
    {
        #region Variables
        private bool PanelVisible { get; set; } = true;
        private (bool, GroupMenuDto) UserAccess { get; set; } = new();
        private bool IsLoading { get; set; } = true;
        private HubConnection? hubConnection;
        private List<EmailSettingDto> EmailSettings = [];

        private List<ExportFileData> ExportFileDatas =
        [
            new()
            {
                Column = "Code",
                Notes = "Mandatory"
            },
            new()
            {
                Column = "Name",
                Notes = "Mandatory"
            }
        ];

        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Variables

        private bool shouldAddW100Class = true;

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            try
            {
                UserAccess = await UserService.GetUserInfo(ToastService);

                var aa = NavigationManager.ToAbsoluteUri("/notificationHub");
                hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/notificationHub"))
                .Build();

                hubConnection.On<ReceiveDataDto>("ReceiveNotification", async message =>
                {
                    await LoadData();
                });

                await hubConnection.StartAsync();

                await LoadData();

                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    Grid?.SelectRow(0, true);
                    StateHasChanged();
                }
                catch { }
            }
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                EmailSettings.Clear();
                EmailSettings = await Mediator.Send(new GetEmailSettingQuery());
                //SelectedDataItems = [];
                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch { }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            PanelVisible = false;
        }

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            PanelVisible = true;
            try
            {
                if (SelectedDataItems is null)
                {
                    await Mediator.Send(new DeleteEmailSettingRequest(((EmailSettingDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<EmailSettingDto>>();
                    await Mediator.Send(new DeleteEmailSettingRequest(Ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataItems = [];
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task OnSave(GridEditModelSavingEventArgs e)
        {
            try
            {
                var FormEmails = e.EditModel.Adapt<EmailSettingDto>();
                if (FormEmails.Smtp_Pass == null || FormEmails.Smtp_User == null || FormEmails.Smtp_Host == null || FormEmails.Smtp_Encryption == null || FormEmails.Smtp_Port == null)
                {
                    return;
                }

                if (FormEmails.Status == "")
                {
                    FormEmails.Status = "No Testing";
                }

                if (FormEmails.Id == Guid.Empty)
                    await Mediator.Send(new CreateEmailSettingRequest(FormEmails));
                else
                    await Mediator.Send(new UpdateEmailSettingRequest(FormEmails));

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private List<string> Stts_Ecrypt = new List<string>
        {
            "none",
            "TLS (STARTTLS)",
            "SSL/TLS"
        }; private bool showPassword = false;
        private string showPasswordIcon = "fa-solid fa-eye-slash";

        private bool? IsConnected { get; set; }
        private bool isLoading { get; set; }
        private void TogglePasswordVisibility()
        {
            showPassword = !showPassword;

            if (showPassword == true)
            {
                showPasswordIcon = "fa-solid fa-eye";
            }
            else
            {
                showPasswordIcon = "fa-solid fa-eye-slash";
            }
        }
        
        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}