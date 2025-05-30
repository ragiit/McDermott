﻿using static McDermott.Application.Features.Commands.Patient.DiseaseHistoryCommand;

namespace McDermott.Web.Components.Pages.Patient
{
    public partial class DiseasePage
    {
        private List<GeneralConsultanCPPTDto> getGeneralConsultanCPPT = [];
        private List<GeneralConsultanServiceDto> getGeneralConsultan = [];
        private List<DiagnosisDto> getDiagnosis = [];
        private List<DiseaseDto> getDisease = [];

        #region variable

        private bool PanelVisible { get; set; } = false;
        private bool IsPopUpForm { get; set; } = false;
        private bool ShowForm { get; set; } = false;
        private bool IsActive { get; set; } = false;
        private IGrid Grid { get; set; }

        #endregion variable

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
                    StateHasChanged();
                }
                catch { }

                await LoadData();
                StateHasChanged();

                try
                {
                    if (Grid is not null)
                    {
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(1);
                        await Grid.WaitForDataLoadAsync();
                        Grid.ExpandGroupRow(2);
                        StateHasChanged();
                    }
                }
                catch { }

                StateHasChanged();
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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await LoadData();
            PanelVisible = false;
        }

        //private async Task LoadData()
        //{
        //    PanelVisible = true;

        //    // Load data asynchronously
        //    getGeneralConsultan = (await Mediator.Send(new GetGeneralConsultanServiceQuery())).Item1;
        //    getGeneralConsultanCPPT = await Mediator.Send(new GetGeneralConsultanCPPTQuery());
        //    getDiagnosis = (await Mediator.Send(new GetDiagnosisQuery())).Item1;

        //    // Clear the getDisease list before populating it
        //    getDisease.Clear();

        //    // Iterate through each GeneralConsultanServiceDto
        //    foreach (var Gc in getGeneralConsultan)
        //    {
        //        // Find all CPPT records associated with the current GeneralConsultanServiceDto
        //        var relatedCppts = getGeneralConsultanCPPT
        //            .Where(x => x.GeneralConsultanServiceId == Gc.Id && x.Title == "Diagnosis")
        //            .OrderBy(x => x.CreatedDate)
        //            .ToList();

        //        // Iterate through each related CPPT record
        //        foreach (var cppt in relatedCppts)
        //        {
        //            // Find the corresponding disease based on the CPPT body and diagnosis name
        //            var getDataDisease = getDiagnosis
        //                .Where(x => x.Name == cppt.Body && x.CronisCategoryId is not null)
        //                .FirstOrDefault();

        //            // Create a new DiseaseDto based on the current data
        //            var diseases = new DiseaseDto()
        //            {
        //                DiseaseName = getDataDisease?.Name ?? "",
        //                PatientName = Gc.Patient?.Name ?? "Unknown",
        //                PhycisianName = Gc.Pratitioner?.Name ?? "Unknown",
        //                DateDisease = cppt.DateTime // Assuming DateTime is a property in CPPT
        //            };

        //            // Add the DiseaseDto to the list
        //            getDisease.Add(diseases);
        //        }
        //    }

        //    // Reload the grid to reflect the updated data
        //    Grid.Reload();

        //    // Hide the panel
        //    PanelVisible = false;
        //}

        private List<CountryDto> Countries = new();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }

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

        private List<DiseaseHistoryTemp> DiseaseHistories { get; set; } = [];

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetDiseaseHistoryQuery
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm ?? ""
                });
                DiseaseHistories = result.Item1;
                totalCount = result.PageCount;
                activePageIndex = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }
    }
}