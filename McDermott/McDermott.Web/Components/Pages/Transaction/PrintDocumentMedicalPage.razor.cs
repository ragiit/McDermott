using DevExpress.Blazor.RichEdit;
using McDermott.Extentions;

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
        public bool PanelVisible { get; set; } = false;
        public bool ViewVisible { get; set; } = false;
        private DxRichEdit richEdit;
        private DevExpress.Blazor.RichEdit.Document documentAPI;
        public byte[]? DocumentContent;

        #endregion Variabel Static

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

        private List<string> CertifiedMedicalCategroy =
        [
            "AUTHORIZATION FOR RELEASE OF MEDICAL",
            "INFORMED CONSENT MEDICAL ACTIONS",
            "REFERENCE ANSWER",
            "REFUSAL OF MEDICAL TREATMENT",
            "ACCIDENT REFERRAL",
            "GENERAL REFERRAL",
            "CERTIFICATE OF FITNESS"
        ];

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
                getGeneralConsultanServices = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == Ids))).Item1;
            }
            catch { }
            finally
            {
                PanelVisible = false;
            }
        }

        private async Task SelectCategory()
        {
            switch (postGenerateMedicalcertificates.JenisSurat)
            {
                case "AUTHORIZATION FOR RELEASE OF MEDICAL":
                    await AuthorizationForReleaseOfMedical();
                    break;

                case "INFORMED CONSENT MEDICAL ACTIONS":
                    await InformedConsentMedicalActions();
                    break;

                case "REFERENCE ANSWER":
                    await ReferenceAnswer();
                    break;

                case "REFUSAL OF MEDICAL TREATMENT":
                    await RefusalOfMedicalTreatment();
                    break;

                case "ACCIDENT REFERRAL":
                    await AccidentReferral();
                    break;

                case "GENERAL REFERRAL":
                    await GeneralReferral();
                    break;

                case "CERTIFICATE OF FITNESS":
                    await CertificateOfFitness();
                    break;

                default: break;
            }
        }

        private async Task AuthorizationForReleaseOfMedical()
        {
            ViewVisible = true;
        }

        private async Task InformedConsentMedicalActions()
        {
        }

        private async Task ReferenceAnswer()
        { }

        private async Task RefusalOfMedicalTreatment()
        { }

        private async Task AccidentReferral()
        { }

        private async Task GeneralReferral()
        { }

        private async Task CertificateOfFitness()
        {
        }
    }
}