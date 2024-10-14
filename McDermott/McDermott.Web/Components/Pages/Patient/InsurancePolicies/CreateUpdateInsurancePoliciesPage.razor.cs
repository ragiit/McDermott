using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.AllQueries.CountModelCommand;

namespace McDermott.Web.Components.Pages.Patient.InsurancePolicies
{
    public partial class CreateUpdateInsurancePoliciesPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await base.OnAfterRenderAsync(firstRender);

            //if (firstRender)
            //{
            //}
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

        private string FormUrl = "patient/insurance-policies";
        private bool PanelVisible { get; set; } = true;

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private InsurancePolicyDto InsurancePolicy { get; set; } = new();
        private List<InsuranceDto> Insurances { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            await LoadDataInsurance();
            await LoadDataUser();

            IsBPJS = Insurances.Any(x => x.Id == InsurancePolicy.InsuranceId && x.IsBPJS == true);
            PanelVisible = false;
        }

        #region ComboboxInsurance

        private DxComboBox<InsuranceDto, long> refInsuranceComboBox { get; set; }
        private int InsuranceComboBoxIndex { get; set; } = 0;
        private int totalCountInsurance = 0;

        private async Task OnSearchInsurance()
        {
            await LoadDataInsurance();
        }

        private async Task OnSearchInsuranceIndexIncrement()
        {
            if (InsuranceComboBoxIndex < (totalCountInsurance - 1))
            {
                InsuranceComboBoxIndex++;
                await LoadDataInsurance(InsuranceComboBoxIndex, 10);
            }
        }

        private async Task OnSearchInsuranceIndexDecrement()
        {
            if (InsuranceComboBoxIndex > 0)
            {
                InsuranceComboBoxIndex--;
                await LoadDataInsurance(InsuranceComboBoxIndex, 10);
            }
        }

        private async Task SelectedItemInsuranceChanged(InsuranceDto e)
        {
            IsBPJS = false;
            if (e is null)
                return;

            IsBPJS = e.IsBPJS;
        }

        private async Task OnInputInsuranceChanged(string e)
        {
            InsuranceComboBoxIndex = 0;
            await LoadDataInsurance();
        }

        private async Task LoadDataInsurance(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetInsuranceQuery
                {
                    SearchTerm = refInsuranceComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Insurances = result.Item1;
                totalCountInsurance = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxInsurance

        #region ComboboxUser

        private DxComboBox<UserDto, long> refUserComboBox { get; set; }
        private int UserComboBoxIndex { get; set; } = 0;
        private int totalCountUser = 0;

        private async Task OnSearchUser()
        {
            await LoadDataUser();
        }

        private async Task OnSearchUserIndexIncrement()
        {
            if (UserComboBoxIndex < (totalCountUser - 1))
            {
                UserComboBoxIndex++;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnSearchUserndexDecrement()
        {
            if (UserComboBoxIndex > 0)
            {
                UserComboBoxIndex--;
                await LoadDataUser(UserComboBoxIndex, 10);
            }
        }

        private async Task OnInputUserChanged(string e)
        {
            UserComboBoxIndex = 0;
            await LoadDataUser();
        }

        private List<UserDto> Users = [];

        private async Task LoadDataUser(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetUserQuery2(
                 predicate: x => x.Id == InsurancePolicy.UserId,
                 pageIndex: pageIndex,
                 pageSize: pageSize,
                 searchTerm: refUserComboBox?.Text ?? "",
                 select: x => new User
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Email = x.Email
                 }

            ));
            Users = result.Item1;
            totalCountUser = result.pageCount;
            PanelVisible = false;
        }

        #endregion ComboboxUser

        private async Task LoadData()
        {
            var result = await Mediator.Send(new GetSingleInsurancePolicyQuery
            {
                Predicate = x => x.Id == Id
            });

            InsurancePolicy = new();

            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo(FormUrl);
                    return;
                }

                InsurancePolicy = result ?? new();
            }
        }

        private bool IsLoading { get; set; } = false;

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }

        private async Task HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private bool IsBPJS = false;
        private ResponseAPIBPJSIntegrationGetPeserta ResponseAPIBPJSIntegrationGetPeserta { get; set; } = new();

        private async Task OnClickGetBPJS()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(InsurancePolicy.PolicyNumber))
                {
                    ToastService.ShowInfo("Please insert the Policy Number!");
                    return;
                }

                IsLoadingGetBPJS = true;

                var result = await PcareService.SendPCareService(nameof(SystemParameter.PCareBaseURL), $"peserta/{InsurancePolicy.PolicyNumber}", HttpMethod.Get);
                if (result.Item2 == 200)
                {
                    if (result.Item1 == null)
                    {
                        ResponseAPIBPJSIntegrationGetPeserta = new();
                        InsurancePolicy = new();
                        IsLoadingGetBPJS = false;
                        return;
                    }

                    var tempInsurancePolicy = InsurancePolicy;

                    ResponseAPIBPJSIntegrationGetPeserta = System.Text.Json.JsonSerializer.Deserialize<ResponseAPIBPJSIntegrationGetPeserta>(result.Item1);
                    InsurancePolicy = ResponseAPIBPJSIntegrationGetPeserta.Adapt<InsurancePolicyDto>();

                    InsurancePolicy.Id = tempInsurancePolicy.Id;
                    InsurancePolicy.PolicyNumber = tempInsurancePolicy.PolicyNumber;
                    InsurancePolicy.UserId = tempInsurancePolicy.UserId;
                    InsurancePolicy.InsuranceId = tempInsurancePolicy.InsuranceId;
                    InsurancePolicy.Active = tempInsurancePolicy.Active;

                    InsurancePolicy.AsuransiNoAsuransi = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.NoAsuransi;
                    InsurancePolicy.AsuransiNmAsuransi = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.NmAsuransi;
                    InsurancePolicy.AsuransiCob = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.Cob;
                    InsurancePolicy.AsuransiKdAsuransi = ResponseAPIBPJSIntegrationGetPeserta.Asuransii.KdAsuransi;

                    InsurancePolicy.JnsKelasNama = ResponseAPIBPJSIntegrationGetPeserta.JnsKelass.Nama;
                    InsurancePolicy.JnsKelasKode = ResponseAPIBPJSIntegrationGetPeserta.JnsKelass.Kode;

                    InsurancePolicy.JnsPesertaNama = ResponseAPIBPJSIntegrationGetPeserta.JnsPesertaa.Nama;
                    InsurancePolicy.JnsPesertaKode = ResponseAPIBPJSIntegrationGetPeserta.JnsPesertaa.Kode;

                    InsurancePolicy.KdProviderGigiKdProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderGigii.KdProvider;
                    InsurancePolicy.KdProviderGigiNmProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderGigii.NmProvider;

                    InsurancePolicy.KdProviderPstKdProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderPstt.KdProvider;
                    InsurancePolicy.KdProviderPstNmProvider = ResponseAPIBPJSIntegrationGetPeserta.KdProviderPstt.NmProvider;
                }
                else
                {
                    ResponseAPIBPJSIntegrationGetPeserta = new();
                    InsurancePolicy = new();

                    dynamic data = JsonConvert.DeserializeObject<dynamic>(result.Item1);

                    ToastService.ShowError($"{data.metaData.message}\n Code: {data.metaData.code}");
                }

                IsLoadingGetBPJS = false;
            }
            catch (Exception ex)
            {
                IsLoadingGetBPJS = false;
                ex.HandleException(ToastService);
            }
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                PanelVisible = true;
                if (IsBPJS)
                {
                    if (InsurancePolicy.NoKartu != InsurancePolicy.PolicyNumber)
                    {
                        ToastService.ShowInfo("Card Number & Policy Number must be same");
                        return;
                    }
                }

                if (InsurancePolicy.UserId == 0)
                {
                    ToastService.ShowInfo("Please select the Patient first.");
                    return;
                }

                var existNumber = await Mediator.Send(new ValidateInsurancePolicyQuery(x => x.Id != InsurancePolicy.Id && x.NoKartu == InsurancePolicy.NoKartu));

                if (existNumber)
                {
                    ToastService.ShowInfo("Card Number already exist.");
                    return;
                }

                if (!IsBPJS)
                {
                    InsurancePolicy.Nama = null;
                    InsurancePolicy.HubunganKeluarga = null;
                    InsurancePolicy.Sex = null;
                    InsurancePolicy.TglLahir = null;
                    InsurancePolicy.TglMulaiAktif = null;
                    InsurancePolicy.TglAkhirBerlaku = null;
                    InsurancePolicy.GolDarah = null;
                    InsurancePolicy.NoHP = null;
                    InsurancePolicy.NoKTP = null;
                    InsurancePolicy.PstProl = null;
                    InsurancePolicy.PstPrb = null;
                    InsurancePolicy.Aktif = false;
                    InsurancePolicy.KetAktif = null;
                    InsurancePolicy.Tunggakan = 0;

                    InsurancePolicy.KdProviderPstKdProvider = null;
                    InsurancePolicy.KdProviderPstNmProvider = null;
                    InsurancePolicy.KdProviderGigiKdProvider = null;
                    InsurancePolicy.KdProviderGigiNmProvider = null;
                    InsurancePolicy.JnsKelasNama = null;
                    InsurancePolicy.JnsKelasKode = null;
                    InsurancePolicy.JnsPesertaNama = null;
                    InsurancePolicy.JnsPesertaKode = null;

                    InsurancePolicy.AsuransiKdAsuransi = null;
                    InsurancePolicy.AsuransiNmAsuransi = null;
                    InsurancePolicy.AsuransiNoAsuransi = null;
                    InsurancePolicy.AsuransiCob = false;
                }

                if (InsurancePolicy.Id == 0)
                    InsurancePolicy = await Mediator.Send(new CreateInsurancePolicyRequest(InsurancePolicy));
                else
                    await Mediator.Send(new UpdateInsurancePolicyRequest(InsurancePolicy));

                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={InsurancePolicy.Id}");
            }
            catch (Exception ex)
            {
                PanelVisible = false;
                ex.HandleException(ToastService);
            }
            finally
            {
                PanelVisible = false;
            }
        }

        private bool IsLoadingGetBPJS { get; set; } = false;
    }
}