using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Dtos.ClaimUserManagement;
using McDermott.Domain.Entities;
using MediatR;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.BenefitConfigurationCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimHistoryCommand;
using static McDermott.Application.Features.Commands.ClaimUserManagement.ClaimRequestCommand;

namespace McDermott.Web.Components.Pages.ClaimUserManagement
{
    public partial class ClaimRequestPage
    {
        #region UserLoginAndAccessRole

        [Parameter]
        public long Id { get; set; }

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

        #region Relation Data
        private List<BenefitConfigurationDto> GetBenefitConfigurations { get; set; } = [];
        private List<ClaimRequestDto> GetClaimRequests { get; set; } = [];
        private List<UserDto> GetPatient { get; set; } = [];
        private List<UserDto> GetPhycisian { get; set; } = [];
        private ClaimRequestDto PostClaimRequests = new();
        #endregion

        #region Variable Static
        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool VisibleButton { get; set; } = true;
        private bool FormValidationState { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #region Enum Status
        public MarkupString GetIssueStatusIconHtml(EnumClaimRequestStatus? status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case EnumClaimRequestStatus.Draft:
                    priorityClass = "info";
                    title = "Draft";
                    break;

                case EnumClaimRequestStatus.Done:
                    priorityClass = "success";
                    title = "Done";
                    break;

                //case EnumClaimRequestStatus.Active:
                //    priorityClass = "success";
                //    title = "Active";
                //    break;

                //case EnumClaimRequestStatus.InActive:
                //    priorityClass = "danger";
                //    title = "InActive";
                //    break;

                default:
                    return new MarkupString("");
            }
            string html = $"<div class='row '><div class='col-3'>" +
                         $"<span class='badge text-white bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }
        #endregion
        #endregion

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

        #region Async Load
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadDataBenefit();
            await LoadDataPatient();
            await LoadDataPhycisian();
            await LoadData();
            PanelVisible = false;
        }
        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            var result = await Mediator.Send(new GetClaimRequestQuery
            {
                OrderByList = [
                    (x=>x.ClaimDate, true)
                    ],
                SearchTerm = searchTerm,
                PageSize = pageSize,
                PageIndex = pageIndex,
            });
            GetClaimRequests = result.Item1;
            totalCount = result.PageCount;
            activePageIndex = pageIndex;

            PostClaimRequests = result.Item1.FirstOrDefault();

            PanelVisible = false;
        }
        #endregion

        #region ComboBox Patient

        private DxComboBox<UserDto, long?> refPatientComboBox { get; set; }
        private int PatientComboBoxIndex { get; set; } = 0;
        private int totalCountPatient = 0;

        private async Task OnSearchPatient()
        {
            await LoadDataPatient();
        }

        private async Task OnSearchPatientIndexIncrement()
        {
            if (PatientComboBoxIndex < (totalCountPatient - 1))
            {
                PatientComboBoxIndex++;
                await LoadDataPatient(PatientComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPatientIndexDecrement()
        {
            if (PatientComboBoxIndex > 0)
            {
                PatientComboBoxIndex--;
                await LoadDataPatient(PatientComboBoxIndex, 10);
            }
        }

        private async Task OnInputPatientChanged(string e)
        {
            PatientComboBoxIndex = 0;
            await LoadDataPatient(0, 10);
        }

        private async Task LoadDataPatient(int pageIndex = 0, int pageSize = 10)
        {

            var result = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.IsPatient == true,
                SearchTerm = refPatientComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Select = x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth
                }
            });
            GetPatient = result.Item1;
            totalCountPatient = result.PageCount;
        }

        #endregion ComboBox Patient

        #region ComboBox Benefit

        private DxComboBox<BenefitConfigurationDto, long?> refBenefitComboBox { get; set; }
        private int BenefitComboBoxIndex { get; set; } = 0;
        private int totalCountBenefit = 0;

        private async Task OnSearchBenefit()
        {
            await LoadDataBenefit(0, 10);
        }

        private async Task OnSearchBenefitIndexIncrement()
        {
            if (BenefitComboBoxIndex < (totalCountBenefit - 1))
            {
                BenefitComboBoxIndex++;
                await LoadDataBenefit(BenefitComboBoxIndex, 10);
            }
        }

        private async Task OnSearchBenefitIndexDecrement()
        {
            if (BenefitComboBoxIndex > 0)
            {
                BenefitComboBoxIndex--;
                await LoadDataBenefit(BenefitComboBoxIndex, 10);
            }
        }

        private async Task OnInputBenefitChanged(string e)
        {
            BenefitComboBoxIndex = 0;
            await LoadDataBenefit(0, 10);
        }

        private async Task LoadDataBenefit(int pageIndex = 0, int pageSize = 10)
        {
            var result = await Mediator.Send(new GetBenefitConfigurationQuery
            {
                Predicate = x=>x.Status == EnumBenefitStatus.Active,
                SearchTerm = refBenefitComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
            GetBenefitConfigurations = result.Item1;
            totalCountBenefit = result.PageCount;
        }

        #endregion

        #region ComboBox Phycisian

        private DxComboBox<UserDto, long?> refPhycisianComboBox { get; set; }
        private int PhycisianComboBoxIndex { get; set; } = 0;
        private int totalCountPhycisian = 0;

        private async Task OnSearchPhycisian()
        {
            await LoadDataPhycisian(0, 10);
        }

        private async Task OnSearchPhycisianIndexIncrement()
        {
            if (PhycisianComboBoxIndex < (totalCountPhycisian - 1))
            {
                PhycisianComboBoxIndex++;
                await LoadDataPhycisian(PhycisianComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPhycisianIndexDecrement()
        {
            if (PhycisianComboBoxIndex > 0)
            {
                PhycisianComboBoxIndex--;
                await LoadDataPhycisian(PhycisianComboBoxIndex, 10);
            }
        }

        private async Task OnInputPhycisianChanged(string e)
        {
            PhycisianComboBoxIndex = 0;
            await LoadDataPhycisian(0, 10);
        }

        private async Task LoadDataPhycisian(int pageIndex = 0, int pageSize = 10)
        {
            var result = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.IsPhysicion == true && x.IsDoctor == true,
                SearchTerm = refPhycisianComboBox?.Text ?? "",
                PageIndex = pageIndex,
                PageSize = pageSize,
                Select = x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth
                }
            });
            GetPhycisian = result.Item1;
            totalCountPhycisian = result.PageCount;
        }

        #endregion

        #region Click
        private async Task NewItem_Click()
        {
            await Grid.StartEditNewRowAsync();
        }
        private async Task EditItem_Click()
        {
            try
            {
                PanelVisible = true;
                await Grid.StartEditRowAsync(FocusedRowVisibleIndex);
                
                PanelVisible = false;
            }
            catch(Exception e)
            {
                e.HandleException(ToastService);
            }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private bool isActiveButton { get; set; } = true;
        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            try
            {
                if ((ClaimRequestDto)args.DataItem is null)
                    return;

                isActiveButton = ((ClaimRequestDto)args.DataItem)!.Status!.Equals(EnumClaimRequestStatus.Draft);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private ClaimHistoryDto postClaimhistory = new();
        private async Task OnDone(ClaimRequestDto Data)
        {
            // Lakukan validasi klaim terlebih dahulu
            await ValidateClaimRequest(Data.PatientId, Data.BenefitId);

            // Jika tombol tidak terlihat setelah validasi, artinya klaim tidak bisa diajukan
            if (!VisibleButton)
            {
                ToastService.ClearAll();
                ToastService.ShowInfo($"Patient benefits can only be claimed again after {nextEligibleDate.ToString("dd MMMM yyyy")}");
                return;
            }

            var item = new ClaimRequestDto();
            if (Data.Id != 0)
            {
                Data.Status = EnumClaimRequestStatus.Done;
                item = await Mediator.Send(new UpdateClaimRequestRequest(Data));

                ToastService.ShowSuccess("Update Status Success");

                postClaimhistory.PatientId = item.PatientId;
                postClaimhistory.BenefitId = item.BenefitId;
                postClaimhistory.PhycisianId = item.PhycisianId;
                postClaimhistory.ClaimDate = item.ClaimDate;
                postClaimhistory.ClaimedValue = 1;

                await Mediator.Send(new CreateClaimHistoryRequest(postClaimhistory));
            }

            await LoadData();
            StateHasChanged();
        }


        #endregion

        #region Delete
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteClaimRequestRequest(((ClaimRequestDto)e.DataItem).Id));
                }
                else
                {
                    var countriesToDelete = SelectedDataItems.Adapt<List<ClaimRequestDto>>();
                    await Mediator.Send(new DeleteClaimRequestRequest(ids: countriesToDelete.Select(x => x.Id).ToList()));
                }

                SelectedDataItems = [];
                await LoadData(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }
        #endregion

        #region save
        private async Task OnSave()
        {
            try
            {
                var item = new ClaimRequestDto();
                if (PostClaimRequests.Id == 0)
                {
                    PostClaimRequests.Status = EnumClaimRequestStatus.Draft;
                    item = await Mediator.Send(new CreateClaimRequestRequest(PostClaimRequests));
                    ToastService.ClearAll();
                    ToastService.ShowSuccess($"Add Data Claim request Success");
                }
                else
                {
                    item = await Mediator.Send(new UpdateClaimRequestRequest(PostClaimRequests));
                    ToastService.ClearAll();
                    ToastService.ShowSuccess($"Update Data Claim Request Success");
                }
                await LoadData();
            }
            catch (Exception Ex)
            {
                Ex.HandleException(ToastService);
            }
        }
        #endregion

        #region Validasi
        private async Task cekValidasi(BenefitConfigurationDto data)
        {
            if (data is not null)
            {
                if (PostClaimRequests.PatientId is null)
                {
                    PostClaimRequests.BenefitId = null;
                    ToastService.ClearAll();
                    ToastService.ShowInfo("Patient is not null");
                    return;
                }
                PostClaimRequests.BenefitId = data.Id;

                await ValidateClaimRequest(PostClaimRequests.PatientId, PostClaimRequests.BenefitId);
            }
            else
            {
                PostClaimRequests.BenefitId = null;
            }

        }

        private async Task CekPatient(UserDto data)
        {
            VisibleButton = true;
        }
        private DateTime nextEligibleDate { get; set; }
        public async Task ValidateClaimRequest(long? patientId, long? benefitId)
        {
            // Dapatkan konfigurasi benefit
            var benefitConfig = await Mediator.Send(new GetSingleBenefitConfigurationQuery
            {
                Predicate = x => x.Id == benefitId,
            });
            if (benefitConfig == null)
            {
                ToastService.ClearAll();
                ToastService.ShowInfo("Benefit configuration not found.");
                return;
            }

            // Dapatkan riwayat klaim pasien untuk benefit yang sama
            var lastClaims = await Mediator.Send(new GetSingleClaimHistoryQuery
            {
                Predicate = c => c.PatientId == patientId && c.BenefitId == benefitId,
                OrderByList = [
                    (x=>x.ClaimDate, true)
                    ]
            });
            
            var lastClaimCount = await Mediator.Send(new GetClaimHistoryQuery
            {
                Predicate = c => c.PatientId == patientId && c.BenefitId == benefitId,              
                
            });
            int counts = 0;
            if (benefitConfig.TypeOfBenefit == "Amount")
            {
                 counts = lastClaimCount.Item1.Select(x => x.ClaimedValue).Count();
            }
            else if(benefitConfig.TypeOfBenefit == "Qty")
            {
                counts = lastClaimCount.Item1.Select(x => x.ClaimedValue).Count();
            }
            

            // Cek apakah klaim melebihi durasi yang diizinkan
            if (lastClaims != null)
            {
                 nextEligibleDate = CalculateNextEligibleDate(lastClaims.ClaimDate, benefitConfig);

                bool isEligibleForClaim = DateTime.Now <= nextEligibleDate && counts >= benefitConfig.BenefitValue ;

                if (isEligibleForClaim)
                {
                    ToastService.ClearAll();
                    ToastService.ShowInfo($"Patient benefits can only be claimed again after {nextEligibleDate.ToString("dd MMMM yyyy")}");
                    VisibleButton = false;
                    return;
                }

            }

            // Jika validasi berhasil
            ToastService.ClearAll();
            ToastService.ShowSuccess("Validation was successful. Claims can be filed.");
            VisibleButton = true;
        }

        private DateTime CalculateNextEligibleDate(DateTime lastClaimDate, BenefitConfigurationDto benefitConfig)
        {
            int duration = benefitConfig.BenefitDuration.GetValueOrDefault();

            return benefitConfig.DurationOfBenefit switch
            {
                "Days" => lastClaimDate.AddDays(duration),
                "Months" => lastClaimDate.AddMonths(duration),
                "Years" => lastClaimDate.AddYears(duration),
                _ => lastClaimDate
            };
        }

        #endregion


    }
}
