using DevExpress.Data.Helpers;
using McDermott.Extentions;
using Microsoft.AspNetCore.Components;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class PrintDocumentMedicalPage
    {
        private List<GeneralConsultanServiceDto> getGeneralConsultanServices = [];
        private List<UserDto> getPatient = [];
        private GenerateMedicalcertificatesDto postGenerateMedicalcertificates = new();
        #region Variabel Static
        [Parameter]
        public string IdEncrypt { get; set; } = string.Empty;
        private long? Ids { get; set; }
        public bool PanelVisible {  get; set; }=false;

        #endregion
        // Metode untuk mendekripsi ID
        private async Task Decrypt()
        {
            if (!string.IsNullOrEmpty(IdEncrypt))
            {
                Ids = SecureHelper.DecryptIdFromBase64(IdEncrypt);
            }
        }

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


        // Override OnParametersSetAsync untuk mendekripsi setelah parameter diatur
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }

            await LoadData();
            await Decrypt();
        }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                getGeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == Ids));
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
    }
}
