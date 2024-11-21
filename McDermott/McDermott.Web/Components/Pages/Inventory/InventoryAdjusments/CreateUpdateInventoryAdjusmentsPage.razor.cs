using DevExpress.Pdf.Native.BouncyCastle.Utilities;
using DevExpress.XtraRichEdit.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;

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

        private EnumStatusInventoryAdjustment StagingText { get; set; } = EnumStatusInventoryAdjustment.InProgress;
        private bool PanelVisible { get; set; } = true;
        private bool ShowConfirmation { get; set; } = false;
        private bool FormValidationState { get; set; } = true;
        private bool ActiveButton { get; set; } = false;
        private bool ShowForm { get; set; } = false;

        private int FocusedRowVisibleIndex { get; set; }

        private InventoryAdjusmentDto InventoryAdjusment { get; set; } = new();

        private List<InventoryAdjusmentDto> InventoryAdjusments { get; set; } = [];
        private List<InventoryAdjustmentLogDto> InventoryAdjusmentLogs { get; set; } = [];
        private InventoryAdjustmentLogDto postInventoryAdjusmentLog { get; set; } = new();
        private List<ProductDto> AllProducts { get; set; } = [];
        private List<ProductDto> Products { get; set; } = [];

        #region ComboBox Uom

        private List<UomDto> Uoms { get; set; } = [];

        private CancellationTokenSource? _ctsUom;

        private async Task OnInputUom(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _ctsUom?.Cancel();
                _ctsUom?.Dispose();
                _ctsUom = new CancellationTokenSource();

                await Task.Delay(700, _ctsUom.Token);

                await LoadUom(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _ctsUom?.Dispose();
                _ctsUom = null;
            }
        }

        private async Task LoadUom(string? e = "", Expression<Func<Uom, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Uoms = await Mediator.QueryGetComboBox<Uom, UomDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Uom

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
            try
            {
                PanelVisible = true;
                InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Cancel;
                StagingText = EnumStatusInventoryAdjustment.Cancel;
                await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));
                await RefreshInventoryAdjsument();
                PanelVisible = false;
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

        #region ComboBox Location

        private LocationDto SelectedLocation { get; set; } = new();

        private async Task SelectedItemChanged(LocationDto e)
        {
            if (e is null)
            {
                SelectedLocation = new();
                await LoadLocation();
            }
            else
                SelectedLocation = e;
        }

        private CancellationTokenSource? _cts;

        private async Task OnInputLocation(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _cts?.Cancel();
                _cts?.Dispose();
                _cts = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _cts.Token);

                await LoadLocation(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _cts?.Dispose();
                _cts = null;
            }
        }

        private List<LocationDto> Locations { get; set; } = [];

        private async Task LoadLocation(string? e = "", Expression<Func<Locations, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Locations = await Mediator.QueryGetComboBox<Locations, LocationDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Location

        #region ComboBox Company

        private List<CompanyDto> Companies { get; set; } = [];
        private CompanyDto SelectedCompany { get; set; } = new();

        private async Task SelectedItemChanged(CompanyDto e)
        {
            if (e is null)
            {
                SelectedCompany = new();
                await LoadCompany();
            }
            else
                SelectedCompany = e;
        }

        private CancellationTokenSource? _ctsCompany;

        private async Task OnInputCompany(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _ctsCompany?.Cancel();
                _ctsCompany?.Dispose();
                _ctsCompany = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsCompany.Token);

                await LoadCompany(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _ctsCompany?.Dispose();
                _ctsCompany = null;
            }
        }

        private async Task LoadCompany(string? e = "", Expression<Func<Company, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Companies = await Mediator.QueryGetComboBox<Company, CompanyDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Company

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

                    await LoadUom();
                    await LoadCompany(predicate: x => x.Id == InventoryAdjusment.CompanyId);
                    await LoadLocation(predicate: x => x.Id == InventoryAdjusment.LocationId);

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
                else
                {
                    await LoadLocation();
                    await LoadProduct();
                    await LoadCompany();

                    if (Companies.Count == 1)
                        InventoryAdjusment.CompanyId = Companies.FirstOrDefault()?.Id;
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
                    switch (InventoryAdjusment.Status)
                    {
                        case EnumStatusInventoryAdjustment.Draft:
                            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.InProgress;
                            StagingText = EnumStatusInventoryAdjustment.Invalidate;

                            if (InventoryAdjusment is not null)
                            {
                                postInventoryAdjusmentLog.InventoryAdjusmentId = InventoryAdjusment.Id;
                                postInventoryAdjusmentLog.UserById = UserLogin.Id;
                                postInventoryAdjusmentLog.Status = EnumStatusInventoryAdjustment.Draft;

                                await Mediator.Send(new CreateInventoryAdjusmentLogRequest(postInventoryAdjusmentLog));
                            }

                            await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            var temps = new List<InventoryAdjusmentDetailDto>();

                            var p = (await Mediator.Send(new GetTransactionStockQueryNew
                            {
                                Predicate = x => x.LocationId == InventoryAdjusment.LocationId && x.Validate == true,
                                Select = x => new TransactionStock
                                {
                                    ProductId = x.ProductId,
                                },
                                IsGetAll = true,
                            })).Item1;

                            var ps = (await Mediator.Send(new GetProductQueryNew
                            {
                                Predicate = x => p.Select(s => s.ProductId).Contains(x.Id),
                                Select = x => new Product
                                {
                                    UomId = x.UomId,
                                    TraceAbility = x.TraceAbility,
                                    Id = x.Id
                                },
                                IsGetAll = true
                            })).Item1;

                            foreach (var o in ps)
                            {
                                var sp = (await Mediator.Send(new GetTransactionStockQueryNew
                                {
                                    Predicate = s => s.ProductId == o.Id && s.LocationId == InventoryAdjusment.LocationId && s.Validate == true,
                                    Select = x => new TransactionStock
                                    {
                                        ProductId = x.ProductId,
                                        Batch = x.Batch,
                                        ExpiredDate = x.ExpiredDate,
                                        Quantity = x.Quantity,
                                        UomId = x.UomId
                                    },
                                    IsGetAll = true
                                })).Item1;

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
                                    UomId = ps.FirstOrDefault(x => item.ProductId == x.Id)?.UomId,
                                    Validate = false
                                });
                            }

                            list = await Mediator.Send(new CreateListTransactionStockRequest(list));

                            foreach (var item in temps)
                            {
                                item.TransactionStockId = list.FirstOrDefault(x => x.SourceTable == nameof(InventoryAdjusment) && x.SourcTableId == item.Id)?.Id ?? null;
                            }

                            await Mediator.Send(new UpdateListInventoryAdjusmentDetailRequest(temps));

                            Id = InventoryAdjusment.Id;

                            NavigationManager.NavigateTo($"inventory/inventory-adjusments/{EnumPageMode.Update.GetDisplayName()}?Id={InventoryAdjusment.Id}", true);

                            break;

                        case EnumStatusInventoryAdjustment.InProgress:
                            InventoryAdjusment.Status = EnumStatusInventoryAdjustment.Invalidate;
                            StagingText = EnumStatusInventoryAdjustment.Invalidate;

                            var i = await Mediator.Send(new UpdateInventoryAdjusmentRequest(InventoryAdjusment));

                            if (i is not null)
                            {
                                postInventoryAdjusmentLog.InventoryAdjusmentId = i.Id;
                                postInventoryAdjusmentLog.UserById = UserLogin.Id;
                                postInventoryAdjusmentLog.Status = EnumStatusInventoryAdjustment.InProgress;
                                await Mediator.Send(new CreateInventoryAdjusmentLogRequest(postInventoryAdjusmentLog));
                            }

                            if (StagingText == EnumStatusInventoryAdjustment.Invalidate)
                            {
                                var update = await Mediator.Send(new GetTransactionStockQuery(x => x.Reference == InventoryAdjusment.Reference));
                                foreach (var item in update)
                                {
                                    item.Validate = true;
                                }

                                await Mediator.Send(new UpdateListTransactionStockRequest(update));
                            }
                            break;
                    }

                    await RefreshInventoryAdjsument();
                    await LoadDataInventoryAdjusmentDetail();
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

        #region Inventory Adjusment Detail

        public IGrid GridDetail { get; set; }
        private IReadOnlyList<object> SelectedDetailDataItems { get; set; } = [];
        private List<InventoryAdjusmentDetailDto> InventoryAdjusmentDetails { get; set; } = [];

        private void GridDetail_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnDeleteInventoryAdjumentDetail(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDetailDataItems == null || !SelectedDetailDataItems.Any())
                {
                    var id = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourcTableId == ((InventoryAdjusmentDetailDto)e.DataItem).Id && x.SourceTable == nameof(InventoryAdjusment)))).FirstOrDefault() ?? new();
                    await Mediator.Send(new DeleteTransactionStockRequest(id.Id));
                    await Mediator.Send(new DeleteInventoryAdjusmentDetailRequest(((InventoryAdjusmentDetailDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDetailDataItems.Adapt<List<InventoryAdjusmentDetailDto>>();

                    foreach (var id in a)
                    {
                        var idd = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourcTableId == ((InventoryAdjusmentDetailDto)e.DataItem).Id && x.SourceTable == nameof(InventoryAdjusment)))).FirstOrDefault() ?? new();
                        await Mediator.Send(new DeleteTransactionStockRequest(idd.Id));
                    }

                    await Mediator.Send(new DeleteInventoryAdjusmentDetailRequest(ids: a.Select(x => x.Id).ToList()));
                }

                SelectedDetailDataItems = [];
                await LoadDataInventoryAdjusmentDetail(activePageIndex, pageSize);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private InventoryAdjusmentDetailDto FormInventoryAdjusmentDetail { get; set; } = new();

        private async Task OnSaveInventoryAdjumentDetail(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                var IsBatch = Products.FirstOrDefault(x => x.Id == FormInventoryAdjusmentDetail.ProductId)?.TraceAbility ?? false;

                if (IsBatch && string.IsNullOrWhiteSpace(FormInventoryAdjusmentDetail.Batch))
                {
                    ToastService.ShowInfo("Please Select or Insert Batch Number");
                    e.Cancel = true;
                    return;
                }

                if (FormInventoryAdjusmentDetail.ProductId is null || string.IsNullOrWhiteSpace(FormInventoryAdjusmentDetail.Batch) && IsBatch || FormInventoryAdjusmentDetail.ExpiredDate is null || FormInventoryAdjusmentDetail.UomId is null)
                {
                    e.Cancel = true;
                    return;
                }

                var editModel = (InventoryAdjusmentDetailDto)e.EditModel;

                var check = await Mediator.Send(new ValidateInventoryAdjusmentDetail(x => x.InventoryAdjusmentId == InventoryAdjusment.Id && x.Id != editModel.Id && x.ProductId == editModel.ProductId && x.Batch == editModel.Batch));
                if (check)
                {
                    ToastService.ShowInfo($"The Product with name '{editModel.Product.Name}' and Batch '{editModel.Batch}' is already exist");
                    e.Cancel = true;
                    return;
                }

                editModel.InventoryAdjusmentId = InventoryAdjusment.Id;

                if (editModel.Id == 0)
                {
                    var ez = await Mediator.Send(new CreateInventoryAdjusmentDetailRequest(editModel));
                    var temp = await Mediator.Send(new CreateTransactionStockRequest(new TransactionStockDto
                    {
                        Reference = InventoryAdjusment.Reference,
                        Batch = editModel.Batch,
                        ExpiredDate = editModel.ExpiredDate,
                        LocationId = InventoryAdjusment.LocationId,
                        Quantity = editModel.Difference,
                        UomId = editModel.UomId,
                        Validate = false,
                        SourceTable = nameof(InventoryAdjusment),
                        SourcTableId = ez.Id,
                        ProductId = editModel.ProductId,
                    }));

                    ez.TransactionStockId = temp.Id;
                    await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(ez));
                }
                else
                {
                    var updtStock = (await Mediator.Send(new GetTransactionStockQuery(x => x.SourceTable == nameof(InventoryAdjusment) && x.SourcTableId == editModel.Id))).FirstOrDefault() ?? new();
                    updtStock.Quantity = editModel.Difference;
                    updtStock.UomId = editModel.UomId;

                    await Mediator.Send(new UpdateTransactionStockRequest(updtStock));
                    await Mediator.Send(new UpdateInventoryAdjusmentDetailRequest(editModel));
                }

                SelectedDetailDataItems = [];
                await LoadDataInventoryAdjusmentDetail();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #region Searching

        private int pageSize { get; set; } = 10;
        private int totalCount = 0;
        private int activePageIndex { get; set; } = 0;
        private string searchTerm { get; set; } = string.Empty;

        private async Task OnSearchBoxChanged(string searchText)
        {
            searchTerm = searchText;
            await LoadDataInventoryAdjusmentDetail(0, pageSize);
        }

        private async Task OnPageSizeIndexChanged(int newPageSize)
        {
            pageSize = newPageSize;
            await LoadDataInventoryAdjusmentDetail(0, newPageSize);
        }

        private async Task OnPageIndexChanged(int newPageIndex)
        {
            await LoadDataInventoryAdjusmentDetail(newPageIndex, pageSize);
        }

        private async Task LoadDataInventoryAdjusmentDetail(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetInventoryAdjusmentDetailQuery
                {
                    Predicate = x => x.InventoryAdjusmentId == InventoryAdjusment.Id,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                InventoryAdjusmentDetails = result.Item1;
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

        private async Task NewItem_Click()
        {
            await LoadProduct();
            await GridDetail.StartEditNewRowAsync();
        }

        private async Task EditItem_Click()
        {
            try
            {
                if (!IsStatus(EnumStatusInventoryAdjustment.InProgress))
                    return;

                await GridDetail.StartEditRowAsync(FocusedRowVisibleIndex);
                var a = (GridDetail.GetDataItem(FocusedRowVisibleIndex) as InventoryAdjusmentDetailDto ?? new());
                FormInventoryAdjusmentDetail = await Mediator.Send(new GetSingleInventoryAdjusmentDetailQuery
                {
                    Predicate = x => x.Id == a.Id
                });
                await LoadProduct(predicate: x => x.Id == FormInventoryAdjusmentDetail.ProductId);
                var id = FormInventoryAdjusmentDetail.Product?.UomId;
                FormInventoryAdjusmentDetail.UomId = id;
                Uoms = (await Mediator.Send(new GetUomQuery
                {
                    Predicate = x => x.Id == id,
                    Select = x => new Uom
                    {
                        Id = x.Id,
                        Name = x.Name
                    },
                    IsGetAll = true
                })).Item1;

                var stocks = (await Mediator.Send(new GetTransactionStockQueryNew
                {
                    Predicate = s => s.ProductId == FormInventoryAdjusmentDetail.ProductId && s.LocationId == InventoryAdjusment.LocationId,
                    Select = x => new TransactionStock
                    {
                        Quantity = x.Quantity,
                        Batch = x.Batch,
                        Id = x.Id,
                        UomId = x.UomId,
                        ExpiredDate = x.ExpiredDate
                    },
                    IsGetAll = true
                })).Item1;

                Batch = stocks?.Select(x => x.Batch)?.ToList() ?? [];
                Batch = Batch.Distinct().ToList();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItem_Click()
        {
            GridDetail.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private List<string> Batch = [];

        private void ResetFormInventoryAdjustmentDetail()
        {
            FormInventoryAdjusmentDetail.ExpiredDate = null;
            FormInventoryAdjusmentDetail.ProductId = null;
            FormInventoryAdjusmentDetail.TransactionStockId = null;
            FormInventoryAdjusmentDetail.UomId = null;
            FormInventoryAdjusmentDetail.TeoriticalQty = 0;
        }

        private async Task UpdateFormInventoryAdjustmentDetail2(TransactionStockDto stockProduct, long qty)
        {
            if (stockProduct != null)
            {
                if (stockProduct.UomId is not null)
                {
                    Uoms = (await Mediator.Send(new GetUomQuery
                    {
                        Predicate = x => x.Id == stockProduct.UomId,
                        Select = x => new Uom
                        {
                            Id = x.Id,
                            Name = x.Name
                        },
                        IsGetAll = true
                    })).Item1;
                }

                FormInventoryAdjusmentDetail.UomId = stockProduct.UomId;
                FormInventoryAdjusmentDetail.TeoriticalQty = qty;
                FormInventoryAdjusmentDetail.ExpiredDate = stockProduct.ExpiredDate;
            }
        }

        #region Products Combobox

        private CancellationTokenSource? _ctsProduct;

        private async Task OnInputProduct(ChangeEventArgs e)
        {
            try
            {
                PanelVisible = true;

                _ctsProduct?.Cancel();
                _ctsProduct?.Dispose();
                _ctsProduct = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsProduct.Token);

                await LoadProduct(e.Value?.ToString() ?? "");
            }
            finally
            {
                PanelVisible = false;

                // Untuk menghindari kebocoran memori (memory leaks).
                _ctsProduct?.Dispose();
                _ctsProduct = null;
            }
        }

        private async Task LoadProduct(string? e = "", Expression<Func<Product, bool>>? predicate = null)
        {
            try
            {
                PanelVisible = true;
                Products = await Mediator.QueryGetComboBox<Product, ProductDto>(e, predicate);
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Products Combobox

        private async Task OnSelectProduct(ProductDto e)
        {
            try
            {
                Batch.Clear();
                ResetFormInventoryAdjustmentDetail();

                if (e == null)
                {
                    await LoadProduct();
                    return;
                }

                FormInventoryAdjusmentDetail.ProductId = e.Id;

                var stocks = (await Mediator.Send(new GetTransactionStockQueryNew
                {
                    Predicate = s => s.ProductId == e.Id && s.LocationId == InventoryAdjusment.LocationId,
                    Select = x => new TransactionStock
                    {
                        Quantity = x.Quantity,
                        Batch = x.Batch,
                        Id = x.Id,
                        UomId = x.UomId,
                        ExpiredDate = x.ExpiredDate
                    },
                    IsGetAll = true
                })).Item1;

                if (stocks is null || stocks.Count == 0)
                    return;

                if (e.TraceAbility)
                {
                    Batch = stocks?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();

                    var firstStockProduct = stocks.Where(x => x.Batch == FormInventoryAdjusmentDetail.Batch);

                    await UpdateFormInventoryAdjustmentDetail2(firstStockProduct.FirstOrDefault() ?? new(), firstStockProduct.Sum(x => x.Quantity));
                }
                else
                {
                    var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == e.Id && x.LocationId == InventoryAdjusment.LocationId)));
                    var firstStockProduct = stocks.FirstOrDefault();
                    FormInventoryAdjusmentDetail.TransactionStockId = firstStockProduct?.Id ?? null;
                    await UpdateFormInventoryAdjustmentDetail2(firstStockProduct ?? new(), s.Sum(x => x.Quantity));
                }

                return;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SelectedBatch(string stockProduct)
        {
            FormInventoryAdjusmentDetail.TransactionStockId = null;
            FormInventoryAdjusmentDetail.UomId = null;
            FormInventoryAdjusmentDetail.ExpiredDate = null;
            FormInventoryAdjusmentDetail.TeoriticalQty = 0;

            if (stockProduct is null)
            {
                return;
            }

            FormInventoryAdjusmentDetail.Batch = stockProduct;

            if (FormInventoryAdjusmentDetail.ProductId is not null)
            {
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                    s.LocationId == InventoryAdjusment.LocationId &&
                    s.Validate == true
                ));

                // Find the first matching product
                var matchedProduct = stockProducts.FirstOrDefault(x =>
                    x.LocationId == InventoryAdjusment.LocationId &&
                    x.ProductId == FormInventoryAdjusmentDetail.ProductId &&
                    x.Batch == FormInventoryAdjusmentDetail.Batch
                );

                // Set UomId and ExpiredDate from the matched product
                FormInventoryAdjusmentDetail.UomId = matchedProduct?.UomId;
                FormInventoryAdjusmentDetail.ExpiredDate = matchedProduct?.ExpiredDate;

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == FormInventoryAdjusmentDetail.ProductId
                && x.LocationId == InventoryAdjusment.LocationId && x.Batch == FormInventoryAdjusmentDetail.Batch));

                // Calculate the sum of quantities for batch products
                FormInventoryAdjusmentDetail.TeoriticalQty = aa.Sum(x => x.Quantity);
            }
        }

        #endregion Inventory Adjusment Detail
    }
}