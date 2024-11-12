using Blazored.TextEditor;
using DocumentFormat.OpenXml.Spreadsheet;
using McDermott.Application.Dtos.AwarenessEvent;
using McDermott.Application.Dtos.Medical;
using Microsoft.AspNetCore.Components.Web;
using static McDermott.Application.Features.Commands.AwarenessEvent.AwarenessEduCategoryCommand;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramAttendanceCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramDetailCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramParticipantCommand;
using static McDermott.Application.Features.Commands.Transaction.WellnessProgramSessionCommand;

namespace McDermott.Web.Components.Pages.Inventory.InventoryAdjusments
{
    public partial class CreateUpdateInventoryAdjusmentsPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

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

        public IGrid Grid { get; set; }
        public IGrid GridDetail { get; set; }

        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDetailDataItems { get; set; } = [];

        private EnumStatusInventoryAdjustment StagingText { get; set; } = EnumStatusInventoryAdjustment.InProgress;
        private bool PanelVisible { get; set; } = true;
        private bool ShowConfirmation { get; set; } = false;
        private bool FormValidationState { get; set; } = true;
        private bool ActiveButton { get; set; } = false;
        private bool ShowForm { get; set; } = false;

        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowDetailVisibleIndex { get; set; }

        private InventoryAdjusmentDto InventoryAdjusment { get; set; } = new();

        private List<InventoryAdjusmentDto> InventoryAdjusments { get; set; } = [];
        private List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetails { get; set; } = [];
        private List<InventoryAdjustmentLogDto> InventoryAdjusmentLogs { get; set; } = [];
        private InventoryAdjustmentLogDto postInventoryAdjusmentLog { get; set; } = new();
        private List<ProductDto> AllProducts { get; set; } = [];
        private List<ProductDto> Products { get; set; } = [];
        private List<UomDto> Uoms { get; set; } = [];

        private bool IsStatus(EnumStatusInventoryAdjustment status) => InventoryAdjusment.Status == status;

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                FormValidationState = false;
                return;
            }
        }

        private async Task HandleValidSubmit()
        {
            try
            {
                IsLoadingConfirm = true;
                if (!FormValidationState && InventoryAdjusmentDetails.Count == 0)
                {
                    ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
                    return;
                }

                var temp = new InventoryAdjusmentDto();

                if (InventoryAdjusment.Id == 0)
                {
                    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Draft;
                    StagingText = EnumStatusInventoryAdjustment.InProgress;

                    var trx = await Mediator.Send(new GetTransactionStockQuery(x => x.SourceTable == nameof(InventoryAdjusment) && x.SourcTableId == InventoryAdjusment.Id));
                    // Get the last reference code
                    var cekReference = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourceTable == nameof(InventoryAdjusment))))
                                        .OrderByDescending(x => x.SourcTableId)
                                        .Select(x => x.Reference)
                                        .FirstOrDefault();
                    int NextReferenceNumber = 1;
                    if (cekReference != null)
                    {
                        int.TryParse(cekReference?["ADJ#".Length..], out NextReferenceNumber);
                        NextReferenceNumber++;
                    }

                    InventoryAdjusment.Reference = $"ADJ#{NextReferenceNumber:D3}";
                    temp = await Mediator.Send(new CreateInventoryAdjusmentRequest(InventoryAdjusment));

                    if (temp is not null)
                    {
                        postInventoryAdjusmentLog.InventoryAdjusmentId = temp.Id;
                        postInventoryAdjusmentLog.UserById = UserLogin.Id;
                        postInventoryAdjusmentLog.Status = EnumStatusInventoryAdjustment.Draft;

                        await Mediator.Send(new CreateInventoryAdjusmentLogRequest(postInventoryAdjusmentLog));
                    }

                    NavigationManager.NavigateTo($"inventory/inventory-adjusments/{EnumPageMode.Update.GetDisplayName()}?Id={temp.Id}", true);
                }
                else
                {
                    temp = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                }

                Id = temp.Id;
                await RefreshInventoryAdjsument();

                //await LoadData();
            }
            catch (Exception e)
            {
                e.HandleException(ToastService);
            }
            finally
            {
                IsLoadingConfirm = false;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
        }

        private async Task CancelItem_Click()
        {
            await LoadData();
        }

        private async Task OnCancelStatus()
        {
            try
            {
                PanelVisible = true;
                InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Cancel;
                StagingText = EnumStatusInventoryAdjustment.Cancel;

                await RefreshInventoryAdjsument();
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

        [SupplyParameterFromQuery]
        private long? Id { get; set; }

        [Parameter]
        public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private async Task RefreshInventoryAdjsument()
        {
            InventoryAdjusment = await Mediator.Send(new GetSingleInventoryAdjusmentQuery
            {
                Predicate = x => x.Id == Id
            });
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        #region ComboboxLocations

        private DxComboBox<LocationDto, long?> refLocationsComboBox { get; set; }
        private List<LocationDto> Locations { get; set; } = [];
        private int LocationsComboBoxIndex { get; set; } = 0;
        private int totalCountLocations = 0;

        private async Task OnSearchLocations()
        {
            await LoadDataLocations();
        }

        private async Task OnSearchLocationsIndexIncrement()
        {
            if (LocationsComboBoxIndex < (totalCountLocations - 1))
            {
                LocationsComboBoxIndex++;
                await LoadDataLocations(LocationsComboBoxIndex, 10);
            }
        }

        private async Task OnSearchLocationsIndexDecrement()
        {
            if (LocationsComboBoxIndex > 0)
            {
                LocationsComboBoxIndex--;
                await LoadDataLocations(LocationsComboBoxIndex, 10);
            }
        }

        private async Task OnInputLocationsChanged(string e)
        {
            LocationsComboBoxIndex = 0;
            await LoadDataLocations();
        }

        private async Task LoadDataLocations(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;

                var a = refLocationsComboBox?.Text?.Split("/");

                var search = string.Empty;
                if (a is not null)
                    search = a.Length == 1 ? a[0] : a[1];

                var result = await Mediator.Send(new GetLocationQuery
                {
                    SearchTerm = search ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new Locations
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentLocation = new Locations
                        {
                            Name = x.ParentLocation == null ? "" : x.ParentLocation.Name
                        }
                    }
                });
                Locations = result.Item1;
                totalCountLocations = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxLocations

        #region ComboboxCompany

        private DxComboBox<CompanyDto, long?> refCompanyComboBox { get; set; }
        private List<CompanyDto> Companies { get; set; } = [];
        private int CompanyComboBoxIndex { get; set; } = 0;
        private int totalCountCompany = 0;

        private async Task OnSearchCompany()
        {
            await LoadDataCompany();
        }

        private async Task OnSearchCompanyIndexIncrement()
        {
            if (CompanyComboBoxIndex < (totalCountCompany - 1))
            {
                CompanyComboBoxIndex++;
                await LoadDataCompany(CompanyComboBoxIndex, 10);
            }
        }

        private async Task OnSearchCompanyIndexDecrement()
        {
            if (CompanyComboBoxIndex > 0)
            {
                CompanyComboBoxIndex--;
                await LoadDataCompany(CompanyComboBoxIndex, 10);
            }
        }

        private async Task OnInputCompanyChanged(string e)
        {
            CompanyComboBoxIndex = 0;
            await LoadDataCompany();
        }

        private async Task LoadDataCompany(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetCompanyQuery
                {
                    SearchTerm = refCompanyComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new Company
                    {
                        Id = x.Id,
                        Name = x.Name
                    }
                });
                Companies = result.Item1;
                totalCountCompany = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxCompany

        private async Task LoadData()
        {
            PanelVisible = true;
            try
            {
                var result = await Mediator.Send(new GetSingleInventoryAdjusmentQuery
                {
                    Predicate = x => x.Id == Id
                });

                if (PageMode == EnumPageMode.Update.GetDisplayName())
                {
                    if (result == null || !Id.HasValue)
                    {
                        NavigationManager.NavigateTo("inventory/inventory-adjusments");
                        return;
                    }

                    InventoryAdjusment = result ?? new();

                    Companies = (await Mediator.Send(new GetCompanyQuery
                    {
                        Predicate = x => x.Id == InventoryAdjusment.CompanyId,
                        Select = x => new Company
                        {
                            Id = x.Id,
                            Name = x.Name
                        }
                    })).Item1;

                    Locations = (await Mediator.Send(new GetLocationQuery
                    {
                        Predicate = x => x.Id == InventoryAdjusment.LocationId,
                        Select = x => new Locations
                        {
                            Id = x.Id,
                            Name = x.Name,
                            ParentLocation = new Locations
                            {
                                Name = x.ParentLocation == null ? "" : x.ParentLocation.Name
                            }
                        }
                    })).Item1;

                    StateHasChanged();

                    switch (InventoryAdjusment.Status)
                    {
                        case EnumStatusInventoryAdjustment.Draft:
                            StagingText = EnumStatusInventoryAdjustment.InProgress;
                            break;

                        case EnumStatusInventoryAdjustment.InProgress:
                            StagingText = EnumStatusInventoryAdjustment.Invalidate;
                            break;

                        case EnumStatusInventoryAdjustment.Invalidate:
                            StagingText = EnumStatusInventoryAdjustment.Invalidate;
                            break;

                        case EnumStatusInventoryAdjustment.Cancel:
                            StagingText = EnumStatusInventoryAdjustment.Cancel;
                            break;

                        default:
                            break;
                    }

                    //await LoadDataOnSearchBoxChangedWellnessProgramAttendance();
                    await LoadDataInventoryAdjusmentDetail();
                    InventoryAdjusmentLogs = await Mediator.Send(new GetInventoryAdjusmentLogQuery(x => x.InventoryAdjusmentId == InventoryAdjusment.Id));
                }
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

        private async Task LoadDataInventoryAdjusmentDetail()
        {
            //
        }

        private bool IsLoadingConfirm { get; set; } = false;
        private bool YesConfirm { get; set; } = false;

        private async Task OnConfirmYes()
        {
            YesConfirm = true;
            await OnClickConfirm(true);
        }

        private void OnConfirmNo()
        {
            ShowConfirmation = false;
        }

        private async Task OnClickConfirm(bool? confirm = false)
        {
            IsLoadingConfirm = true;

            try
            {
                if ((InventoryAdjusmentDetails == null || InventoryAdjusmentDetails.Count == 0) && IsStatus(EnumStatusInventoryAdjustment.InProgress))
                {
                    ToastService.ShowInfo("Please add at least one inventory adjustment detail before proceeding.");
                    return;
                }

                if (InventoryAdjusmentDetails.Count > 0 && InventoryAdjusmentDetails.Any(x => x.RealQty == 0))
                {
                    if (YesConfirm)
                        ShowConfirmation = false;
                    else
                        ShowConfirmation = true;
                }
                else
                    YesConfirm = true;

                if ((!ShowConfirmation && YesConfirm) || IsStatus(EnumStatusInventoryAdjustment.Draft))
                {
                    switch (StagingText)
                    {
                        case EnumStatusInventoryAdjustment.InProgress:
                            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.InProgress;
                            //StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();

                            //await SelectLocation(new LocationDto { Id = InventoryAdjusment.LocationId.GetValueOrDefault(), });

                            var a = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            if (a is not null)
                            {
                                postInventoryAdjusmentLog.InventoryAdjusmentId = a.Id;
                                postInventoryAdjusmentLog.UserById = UserLogin.Id;
                                postInventoryAdjusmentLog.Status = EnumStatusInventoryAdjustment.InProgress;
                                await Mediator.Send(new CreateInventoryAdjusmentLogRequest(postInventoryAdjusmentLog));
                            }

                            var temps = new List<InventoryAdjusmentDetailDto>();
                            foreach (var o in Products)
                            {
                                var sp = await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == o.Id && s.LocationId == InventoryAdjusment.LocationId && s.Validate == true));

                                if (o.TraceAbility)
                                {
                                    var allBatch = sp.Select(x => x.Batch);
                                    allBatch = allBatch.Distinct().ToList();
                                    foreach (var b in allBatch)
                                    {
                                        var spb = sp.Where(x => x.ProductId == o.Id && x.Batch != null && x.Batch == b).FirstOrDefault() ?? new();

                                        temps.Add(new InventoryAdjusmentDetailDto
                                        {
                                            InventoryAdjusmentId = InventoryAdjusment.Id,
                                            ProductId = o.Id,
                                            ExpiredDate = spb.ExpiredDate,
                                            UomId = o.UomId,
                                            TeoriticalQty = sp.Where(x => x.Batch == b).Sum(x => x.Quantity),
                                            Batch = b,
                                            RealQty = 0
                                        });
                                    }
                                }
                                else
                                {
                                    temps.Add(new InventoryAdjusmentDetailDto
                                    {
                                        InventoryAdjusmentId = InventoryAdjusment.Id,
                                        ProductId = o.Id,
                                        ExpiredDate = sp.FirstOrDefault()?.ExpiredDate,
                                        TeoriticalQty = sp.Sum(x => x.Quantity),
                                        Batch = null,
                                        RealQty = 0
                                    });
                                }
                            }

                            temps = await Mediator.Send(new CreateListInventoryAdjusmentDetailRequest(temps));

                            var list = new List<TransactionStockDto>();
                            foreach (var item in temps)
                            {
                                list.Add(new TransactionStockDto
                                {
                                    SourceTable = nameof(InventoryAdjusment),
                                    SourcTableId = item.Id,
                                    ProductId = item.ProductId,
                                    Reference = InventoryAdjusment.Reference,
                                    Batch = item.Batch,
                                    ExpiredDate = item.ExpiredDate,
                                    Quantity = item.Difference,
                                    LocationId = InventoryAdjusment.LocationId,
                                    UomId = Products.FirstOrDefault(x => item.ProductId == x.Id)?.UomId,
                                    Validate = false
                                });
                            }

                            await Mediator.Send(new CreateListTransactionStockRequest(list));
                            //await LoadInventoryAdjustmentDetails();
                            //if (inventoryAdjusmentDetail.Difference != 0)
                            //{
                            //    // Map InventoryAdjusmentDetail to TransactionStockDto using Mapster
                            //    var transactionStockDto = inventoryAdjusmentDetail.Adapt<TransactionStockDto>();
                            //    transactionStockDto.LocationId = InventoryAdjusment.LocationId;
                            //    transactionStockDto.Validate = false;
                            //    transactionStockDto.Quantity = inventoryAdjusmentDetail.Difference;
                            //    transactionStockDto.SourcTableId = inventoryAdjusmentDetail.InventoryAdjusmentId;
                            //    transactionStockDto.SourceTable = nameof(InventoryAdjusment);

                            //    await Mediator.Send(new CreateTransactionStockRequest(transactionStockDto));
                            //}

                            break;

                            //case "In-Progress":
                            //    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                            //    StagingText = EnumStatusInventoryAdjustment.Invalidate.GetDisplayName();

                            //    var i = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            //    if (i is not null)
                            //    {
                            //        postInventoryAdjusmentLog.InventoryAdjusmentId = i.Id;
                            //        postInventoryAdjusmentLog.UserById = UserLogin.Id;
                            //        postInventoryAdjusmentLog.Status = EnumStatusInventoryAdjustment.InProgress;
                            //        await Mediator.Send(new CreateInventoryAdjusmentLogRequest(postInventoryAdjusmentLog));
                            //    }

                            //    if (StagingText == EnumStatusInventoryAdjustment.Invalidate.GetDisplayName())
                            //    {
                            //        var update = await Mediator.Send(new GetTransactionStockQuery(x => x.Reference == InventoryAdjusment.Reference));
                            //        foreach (var item in update)
                            //        {
                            //            item.Validate = true;
                            //        }

                            //        await Mediator.Send(new UpdateListTransactionStockRequest(update));

                            //        return;
                            //        var stockProductsToUpdate = new List<TransactionStockDto>();
                            //        var adjustmentDetailsToUpdate = new List<InventoryAdjusmentDetailDto>();

                            //        foreach (var detail in InventoryAdjusmentDetails)
                            //        {
                            //            var stockProduct = (await Mediator.Send(new GetTransactionStockQuery(s => s.Id == detail.TransactionStockId))).FirstOrDefault();

                            //            if (stockProduct != null)
                            //            {
                            //                //detail.TeoriticalQty = detail.RealQty;
                            //                //stockProduct.Qty = detail.RealQty;
                            //                //stockProductsToUpdate.Add(stockProduct);
                            //            }

                            //            adjustmentDetailsToUpdate.Add(detail);
                            //        }

                            //        //foreach (var stockProduct in stockProductsToUpdate)
                            //        //{
                            //        //    await Mediator.Send(new UpdateStockProductRequest(stockProduct));
                            //        //}

                            //        //foreach (var detail in adjustmentDetailsToUpdate)
                            //        //{
                            //        //    await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(detail));
                            //        //}

                            //        //await Mediator.Send(new UpdateListStockProductRequest(stockProductsToUpdate));
                            //        //await Mediator.Send(new UpdateListTransactionStockRequest());
                            //        //await Mediator.Send(new UpdateListInventoryAdjusmentDetailRequest(adjustmentDetailsToUpdate));

                            //        //await LoadInventoryAdjustmentDetails();
                            //    }
                            //    break;

                            //case "Invalidate":
                            //    InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                            //    var x = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                            //    if (x is not null)
                            //    {
                            //        postInventoryAdjusmentLog.InventoryAdjusmentId = x.Id;
                            //        postInventoryAdjusmentLog.UserById = UserLogin.Id;
                            //        postInventoryAdjusmentLog.Status = EnumStatusInventoryAdjustment.InProgress;
                            //        await Mediator.Send(new CreateInventoryAdjusmentLogRequest(postInventoryAdjusmentLog));
                            //    }

                            //    if (StagingText == EnumStatusInventoryAdjustment.Invalidate.GetDisplayName())
                            //    {
                            //        var update = await Mediator.Send(new GetTransactionStockQuery(x => x.Reference == InventoryAdjusment.Reference));
                            //        foreach (var item in update)
                            //        {
                            //            item.Validate = true;
                            //        }

                            //        await Mediator.Send(new UpdateListTransactionStockRequest(update));
                            //    }
                            //    break;

                            //default:
                            //    return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                YesConfirm = false;
                IsLoadingConfirm = false;
            }
        }
    }
}