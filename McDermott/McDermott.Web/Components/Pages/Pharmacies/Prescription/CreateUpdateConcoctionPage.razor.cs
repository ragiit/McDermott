using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Domain.Entities;
using static McDermott.Application.Features.Commands.AwarenessEvent.EducationProgramCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionCommand;
using static McDermott.Application.Features.Commands.Pharmacies.ConcoctionLineCommand;
using static McDermott.Application.Features.Commands.Pharmacies.MedicamentGroupCommand;
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
        private List<UserDto> GetPractitioner { get; set; } = [];
        private List<ActiveComponentDto> ActiveComponents { get; set; } = [];
        private List<TransactionStockDto> TransactionStocks { get; set; } = [];
        private List<StockOutLinesDto> StockOutLines { get; set; } = [];
        private List<ProductDto> GetProducts { get; set; } = [];
        private List<MedicamentGroupDto> GetMedicamentGroup { get; set; } = [];
        private List<MedicamentDto> GetMedicament { get; set; } = [];

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

                    PostConcoction.PharmacyId = PostPharmacy.Id;
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

        private async Task LoadDataPractitioner()
        {
            var getPractitioner = await Mediator.Send(new GetUserQueryNew
            {
                Predicate = x => x.Id == PostPharmacy.PractitionerId,

            });

            GetPractitioner = getPractitioner.Item1;

        }

        #endregion

        #region Selected Change
        private async Task SelectedMedicamentGroupConcoction(MedicamentGroupDto value)
        {
            try
            {
                if (value is null)
                    return;

                var medicamentGroup = GetMedicamentGroup.Where(x => x.Id == value.Id).FirstOrDefault();
                PostConcoction.MedicamenName = medicamentGroup?.Name;
                PostConcoction.DrugFormId = medicamentGroup?.FormDrugId;

                var a = await Mediator.Send(new GetMedicamentGroupDetailQuery());
                var data = a.Where(x => x.MedicamentGroupId == value?.Id).ToList();
                List<ConcoctionLineDto> concoctionLinesList = new();

                foreach (var item in data)
                {
                    var checkProduct = GetProducts.FirstOrDefault(x => x.Id == item.MedicamentId);
                    if (checkProduct == null)
                        return;

                    var stockProduct = TransactionStocks
                                    .Where(x => x.ProductId == checkProduct.Id && x.LocationId is not null && x.LocationId == PostPharmacy.LocationId && x.Validate == true)
                                    .Sum(x => x.Quantity);

                    var medicamentData = GetMedicament.FirstOrDefault(x => x.ProductId == checkProduct.Id);
                    if (medicamentData is null)
                        return;

                    var newConcoctionLine = new ConcoctionLineDto
                    {
                        ProductId = checkProduct.Id,
                        ProductName = checkProduct.Name,
                    };
                    float? QtyPerDay = 0;
                    if (medicamentData.FrequencyId != null)
                    {
                        QtyPerDay = medicamentData?.Frequency?.TotalQtyPerDay;
                    }
                    else
                    {
                        QtyPerDay = 0;
                    }
                    newConcoctionLine.Id = Helper.RandomNumber;
                    newConcoctionLine.MedicamentDosage = medicamentData!.Dosage;
                    newConcoctionLine.ActiveComponentId = medicamentData?.ActiveComponentId;
                    newConcoctionLine.Dosage = medicamentData!.Dosage;
                    newConcoctionLine.UomId = medicamentData?.UomId;
                    newConcoctionLine.UomName = medicamentData?.Uom?.Name;

                    if (newConcoctionLine.Dosage <= newConcoctionLine.MedicamentDosage)
                    {
                        newConcoctionLine.TotalQty = 1;
                    }
                    else
                    {
                        var temp = (newConcoctionLine.Dosage / newConcoctionLine.MedicamentDosage) + (newConcoctionLine.Dosage % newConcoctionLine.MedicamentDosage != 0 ? 1 : 0);
                        newConcoctionLine.TotalQty = temp * PostConcoction.ConcoctionQty;
                    }
                    if (stockProduct == 0)
                    {
                        newConcoctionLine.AvaliableQty = 0;
                    }
                    else
                    {
                        newConcoctionLine.AvaliableQty = stockProduct;
                    }
                    newConcoctionLine.ActiveComponentName = string.Join(",", ActiveComponents.Where(a => medicamentData?.ActiveComponentId is not null && medicamentData.ActiveComponentId.Contains(a.Id)).Select(n => n.Name));
                    concoctionLinesList.Add(newConcoctionLine);
                }

                GetConcoctionLines = concoctionLinesList;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
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
        private async Task OnDelete(GridDataItemDeletingEventArgs e)
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

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;

            if (FormValidationState)
                await OnSave();
        }

        private async Task HandleInvalidSubmit()
        {
            FormValidationState = true;
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

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