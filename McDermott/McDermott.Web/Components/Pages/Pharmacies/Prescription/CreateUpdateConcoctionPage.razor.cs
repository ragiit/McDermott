using McDermott.Application.Dtos.AwarenessEvent;
using static McDermott.Application.Features.Commands.AwarenessEvent.EducationProgramCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PharmacyCommand;
using static McDermott.Application.Features.Commands.Pharmacies.PrescriptionCommand;

namespace McDermott.Web.Components.Pages.Pharmacies.Prescription
{
    public partial class CreateUpdateConcoctionPage
    {
        #region Relation Data
        private List<ConcoctionDto> GetConcoctions { get; set; } = [];
        private List<ConcoctionLineDto> GetConcoctionLines { get; set; } = [];
        private List<StockOutLinesDto> GetStockOutLines { get; set; } = [];
        private List<PharmacyDto> GetPharmacy { get; set; } = [];

        private ConcoctionDto PostConcoction { get; set; } = new();
        private ConcoctionLineDto PostConcoctionLines { get; set; } = new();
        private StockOutLinesDto PostStockOutLines { get; set; } = new();
        private PharmacyDto PostPharmacy { get; set; } = new();
        #endregion

        #region Static Variabel
        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();
        [SupplyParameterFromQuery]
        public long? PId { get; set; }

        private IGrid Grid { get; set; }
        private bool PanelVisible { get; set; } = false;
        private bool PanelVisibleCl { get; set; } = false;
        public bool FormValidationState { get; set; } = false;
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataItemsCl { get; set; } = [];
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowVisibleIndexCl { get; set; }
        #endregion

        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

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

        private int pageSizeConcoctionLine { get; set; } = 10;
        private int totalCountConcoctionLine = 0;
        private int activePageIndexConcoctionLine { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataConcoctionLine(0, pageSizeConcoctionLine);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSizeConcoctionLine = newPageSize;
            await LoadDataConcoctionLine(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataConcoctionLine(newPageIndex, pageSizeConcoctionLine);
        }

        #endregion Searching


        #region Load Data
        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            //await LoadProduct();
            //await LoadDataTransaction();
            //await LoadDataActiveComponent();
            //await LoadDataDrugDosage();
            //await LoadDataDrugRoute();
            //await LoadDataSigna();
            //await LoadDataDrugForm();
            await LoadData();
            //await LoadDataPatient();
            //await LoadDataPractitioner();

            PanelVisible = false;
        }
        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                SelectedDataItemsCl = [];
                if (PId.HasValue)
                {

                    var cekPharmacy = await Mediator.Send(new GetSinglePharmacyQuery
                    {
                        Predicate = x => x.Id == PId
                    });
                    PostPharmacy = cekPharmacy ?? new();

                    PostConcoction.PrescribingDoctorId = PostPharmacy.PractitionerId;

                }
                else
                {
                    if (PageMode == EnumPageMode.Update.GetDisplayName())
                    {
                        var result = await Mediator.Send(new GetSingleConcoctionQuery
                        {
                            Predicate = x => x.Id == Id,
                        });
                        if (result is null || !Id.HasValue)
                        {
                            NavigationManager.NavigateTo("pharmacy/prescriptions");
                            return;
                        }

                        PostConcoction = result ?? new();

                        await LoadDataConcoctionLine();
                    }
                }
                PanelVisible = false;
            }
            catch (Exception ex) { ex.HandleException(ToastService); }
        }
        private async Task LoadDataConcoctionLine(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisibleCl = true;

                var result = await Mediator.Send(new GetConcoctionLineQuery
                {
                    Predicate = x => x.ConcoctionId == PostConcoction.Id,
                });
                GetConcoctionLines = result.Item1;
                totalCountConcoctionLine = result.PageCount;
                activePageIndexConcoctionLine = result.PageIndex;
                PanelVisibleCl = false;
            }
            catch { }

        }
        #endregion

        #region Grid
        private void GridConcoctionLine_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexCl = args.VisibleIndex;
        }
        #endregion

        #region New Edit Delete Click

        private async Task NewItemCl_Click()
        {
            await Grid.StartEditNewRowAsync();
        }

        private async Task EditItemCl_Click()
        {
            await Grid.StartEditRowAsync(FocusedRowVisibleIndexCl);
        }

        private async Task DeleteItemCl_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndexCl);
        }
        #endregion

        #region Delete Function
        private async Task OnDelete()
        {
            PanelVisibleCl = true;
            var data = SelectedDataItemsCl[0].Adapt<ConcoctionLineDto>();
            if (data is not null)
            {
                if (SelectedDataItems == null || !SelectedDataItems.Any())
                {
                    await Mediator.Send(new DeleteConcoctionLineRequest(((ConcoctionLineDto)e.DataItem).Id));
                }
                else
                {
                    var ClToDelete = SelectedDataItemsCl.Adapt<List<ConcoctionLineDto>>();
                    await Mediator.Send(new DeleteConcoctionLineRequest(ids: ClToDelete.Select(x => x.Id).ToList()));

                }
            }
            PanelVisibleCl = false;
        }
        #endregion

        #region Function Save
        private async Task OnSave()
        {
            try
            {
                if (PostConcoction.Id == 0)
                {
                    PostConcoction.PharmacyId = PostPharmacy.Id;
                    await Mediator.Send(new CreateConcoctionRequest(PostConcoction));
                    ToastService.ShowSuccess("Add Data Concoction Success");
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionRequest(PostConcoction));
                    ToastService.ShowSuccess("Add Data Concoction Success");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        private async Task OnSaveCl()
        {
            try
            {
                if (PostConcoctionLines.Id == 0)
                {
                    PostConcoctionLines.ConcoctionId = PostConcoction.Id;
                    await Mediator.Send(new CreateConcoctionLineRequest(PostConcoctionLines));
                    ToastService.ShowSuccess("Add ConcoctionLine Success");
                }
                else
                {
                    await Mediator.Send(new UpdateConcoctionLineRequest(PostConcoctionLines));
                    ToastService.ShowSuccess("Update ConcoctionLine Success");
                }

                await LoadDataConcoctionLine();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }
        #endregion

    }
}