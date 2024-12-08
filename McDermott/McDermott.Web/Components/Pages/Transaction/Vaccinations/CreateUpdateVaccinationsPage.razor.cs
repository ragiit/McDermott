using DevExpress.Blazor.RichEdit;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation.Results;
using McDermott.Application.Features.Services;
using McDermott.Extentions;
using Microsoft.AspNetCore.Components.Web;
using QuestPDF.Fluent;
using System.Linq.Expressions;
using static McDermott.Application.Features.Commands.Inventory.TransactionStockCommand;
using static McDermott.Application.Features.Commands.Transaction.VaccinationPlanCommand;

namespace McDermott.Web.Components.Pages.Transaction.Vaccinations
{
    public partial class CreateUpdateVaccinationsPage
    {
        #region Vaccination Tabs

        #region Vaccination Given

        #region ComboboxVaccinationGiven

        private DxComboBox<ProductDto, long> refVaccinationGivenComboBox { get; set; }
        private int VaccinationGivenComboBoxIndex { get; set; } = 0;
        private int totalCountVaccinationGiven = 0;

        private async Task OnSearchVaccinationGiven()
        {
            await LoadDataVaccinationGiven();
        }

        private async Task OnSearchVaccinationGivenIndexIncrement()
        {
            if (VaccinationGivenComboBoxIndex < (totalCountVaccinationGiven - 1))
            {
                VaccinationGivenComboBoxIndex++;
                await LoadDataVaccinationGiven(VaccinationGivenComboBoxIndex, 10);
            }
        }

        private async Task OnSearchVaccinationGivenIndexDecrement()
        {
            if (VaccinationGivenComboBoxIndex > 0)
            {
                VaccinationGivenComboBoxIndex--;
                await LoadDataVaccinationGiven(VaccinationGivenComboBoxIndex, 10);
            }
        }

        private async Task OnInputVaccinationGivenChanged(string e)
        {
            VaccinationGivenComboBoxIndex = 0;
            await LoadDataVaccinationGiven();
        }

        private async Task LoadDataVaccinationGiven(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetProductQueryNew
                {
                    SearchTerm = refVaccinationGivenComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                Products = result.Item1;
                totalCountVaccinationGiven = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxVaccinationGiven

        #region ComboboxSalesPerson

        private DxComboBox<UserDto, long?> refSalesPersonComboBox { get; set; }
        private int SalesPersonComboBoxIndex { get; set; } = 0;
        private int totalCountSalesPerson = 0;

        private async Task OnSearchSalesPerson()
        {
            await LoadDataSalesPerson();
        }

        private async Task OnSearchSalesPersonIndexIncrement()
        {
            if (SalesPersonComboBoxIndex < (totalCountSalesPerson - 1))
            {
                SalesPersonComboBoxIndex++;
                await LoadDataSalesPerson(SalesPersonComboBoxIndex, 10);
            }
        }

        private async Task OnSearchSalesPersonIndexDecrement()
        {
            if (SalesPersonComboBoxIndex > 0)
            {
                SalesPersonComboBoxIndex--;
                await LoadDataSalesPerson(SalesPersonComboBoxIndex, 10);
            }
        }

        private async Task OnInputSalesPersonChanged(string e)
        {
            SalesPersonComboBoxIndex = 0;
            await LoadDataSalesPerson();
        }

        private List<UserDto> SalesPersons { get; set; } = [];

        private async Task LoadDataSalesPerson(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refSalesPersonComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Predicate = x => x.IsDoctor == true,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                });
                SalesPersons = result.Item1;
                totalCountSalesPerson = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxSalesPerson

        #region ComboboxEducator

        private DxComboBox<UserDto, long?> refEducatorComboBox { get; set; }
        private int EducatorComboBoxIndex { get; set; } = 0;
        private int totalCountEducator = 0;

        private async Task OnSearchEducator()
        {
            await LoadDataEducator();
        }

        private async Task OnSearchEducatorIndexIncrement()
        {
            if (EducatorComboBoxIndex < (totalCountEducator - 1))
            {
                EducatorComboBoxIndex++;
                await LoadDataEducator(EducatorComboBoxIndex, 10);
            }
        }

        private async Task OnSearchEducatorIndexDecrement()
        {
            if (EducatorComboBoxIndex > 0)
            {
                EducatorComboBoxIndex--;
                await LoadDataEducator(EducatorComboBoxIndex, 10);
            }
        }

        private async Task OnInputEducatorChanged(string e)
        {
            EducatorComboBoxIndex = 0;
            await LoadDataEducator();
        }

        private List<UserDto> Educators { get; set; } = [];

        private async Task LoadDataEducator(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refEducatorComboBox?.Text ?? "",
                    Predicate = x => x.IsDoctor == true,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                });
                Educators = result.Item1;
                totalCountEducator = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxEducator

        #region ComboboxVaccinationGivenPhysicion

        private DxComboBox<UserDto, long?> refVaccinationGivenPhysicionComboBox { get; set; }
        private int VaccinationGivenPhysicionComboBoxIndex { get; set; } = 0;
        private int totalCountVaccinationGivenPhysicion = 0;

        private async Task OnSearchVaccinationGivenPhysicion()
        {
            await LoadDataVaccinationGivenPhysicion();
        }

        private async Task OnSearchVaccinationGivenPhysicionIndexIncrement()
        {
            if (VaccinationGivenPhysicionComboBoxIndex < (totalCountVaccinationGivenPhysicion - 1))
            {
                VaccinationGivenPhysicionComboBoxIndex++;
                await LoadDataVaccinationGivenPhysicion(VaccinationGivenPhysicionComboBoxIndex, 10);
            }
        }

        private async Task OnSearchVaccinationGivenPhysicionIndexDecrement()
        {
            if (VaccinationGivenPhysicionComboBoxIndex > 0)
            {
                VaccinationGivenPhysicionComboBoxIndex--;
                await LoadDataVaccinationGivenPhysicion(VaccinationGivenPhysicionComboBoxIndex, 10);
            }
        }

        private async Task OnInputVaccinationGivenPhysicionChanged(string e)
        {
            VaccinationGivenPhysicionComboBoxIndex = 0;
            await LoadDataVaccinationGivenPhysicion();
        }

        private List<UserDto> VaccinationGivenPhysicions { get; set; } = [];

        private async Task LoadDataVaccinationGivenPhysicion(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUserQueryNew
                {
                    SearchTerm = refVaccinationGivenPhysicionComboBox?.Text ?? "",
                    Predicate = x => x.IsDoctor == true,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                });
                VaccinationGivenPhysicions = result.Item1;
                totalCountVaccinationGivenPhysicion = result.PageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxVaccinationGivenPhysicion

        #endregion Vaccination Given

        private async Task OnClickCancelGiveVaccine(VaccinationPlanDto e)
        {
            var s = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == e.Id))).FirstOrDefault();
            if (s is not null)
            {
                s.Status = EnumStatusVaccination.Scheduled;
                s.GeneralConsultanServiceId = null;
                await Mediator.Send(new UpdateVaccinationPlanRequest(s));
                await LoadVaccinationGivens();
            }
        }

        private async Task OnClickGiveVaccine(VaccinationPlanDto e)
        {
            var s = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == e.Id))).FirstOrDefault();
            if (s is not null)
            {
                s.Status = EnumStatusVaccination.InProgress;
                s.GeneralConsultanServiceId = GeneralConsultanService.Id;
                await Mediator.Send(new UpdateVaccinationPlanRequest(s));
                await LoadVaccinationPlans();
            }
        }

        private List<ProductDto> Products = [];
        private VaccinationPlanDto VaccinationPlan { get; set; } = new();
        private VaccinationPlanDto VaccinationGiven { get; set; } = new();
        private List<VaccinationPlanDto> VaccinationPlans { get; set; } = [];
        private List<VaccinationPlanDto> VaccinationGivens { get; set; } = [];
        private List<VaccinationPlanDto> VaccinationHistoryGivens { get; set; } = [];
        private IGrid GridVaccinationPlan { get; set; }
        private IGrid GridVaccinationGiven { get; set; }
        private IGrid GridVaccinationHistoryGiven { get; set; }
        private IReadOnlyList<object> SelectedDataVaccinationPlanItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataVaccinationGivenItems { get; set; } = [];
        private IReadOnlyList<object> SelectedDataVaccinationHistoryGivenItems { get; set; } = [];
        private List<string> Batch = [];

        private async Task SelectedBatchGiven(string stockProduct)
        {
            VaccinationGiven.TeoriticalQty = 0;

            if (stockProduct is null)
            {
                return;
            }

            VaccinationGiven.Batch = stockProduct;

            if (VaccinationGiven.ProductId != 0)
            {
                var aa = (await Mediator.Send(new GetTransactionStockQueryNew
                {
                    Select = x => new TransactionStock
                    {
                        Quantity = x.Quantity
                    },
                    Predicate = x => x.Validate == true && x.ProductId == VaccinationGiven.ProductId && x.LocationId == GeneralConsultanService.LocationId && x.Batch == VaccinationGiven.Batch
                })).Item1;

                VaccinationGiven.TeoriticalQty = aa.Sum(x => x.Quantity);
            }
        }

        private async Task SelectedBatchPlan(string stockProduct)
        {
            VaccinationPlan.TeoriticalQty = 0;

            if (stockProduct is null)
            {
                return;
            }

            VaccinationPlan.Batch = stockProduct;

            if (VaccinationPlan.ProductId != 0)
            {
                var stockProducts = await Mediator.Send(new GetTransactionStockQuery(s =>
                    s.ProductId == VaccinationPlan.ProductId &&
                    s.LocationId == GeneralConsultanService.LocationId &&
                    s.Validate == true
                ));

                var aa = await Mediator.Send(new GetTransactionStockQuery(x => x.Validate == true && x.ProductId == VaccinationPlan.ProductId
                && x.LocationId == GeneralConsultanService.LocationId && x.Batch == VaccinationPlan.Batch));

                VaccinationPlan.TeoriticalQty = aa.Sum(x => x.Quantity);
            }
        }

        private bool IsVaccinationGiven = false;

        private async Task OnSelectProduct(ProductDto e)
        {
            try
            {
                Batch.Clear();

                if (e == null) return;

                //var stockProducts2 = (await Mediator.Send(new GetTransactionStockQuery(s => s.ProductId == e.Id && s.LocationId == GeneralConsultanService.LocationId)) ?? []);

                var result = (await Mediator.Send(new GetTransactionStockQueryNew
                {
                    Select = x => new TransactionStock
                    {
                        Batch = x.Batch,
                        Quantity = x.Quantity
                    },
                    IsGetAll = true,
                    Predicate = s => s.ProductId == e.Id && s.LocationId == GeneralConsultanService.LocationId
                })).Item1;

                if (e.TraceAbility)
                {
                    Batch = result?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();
                }
                else
                {
                    if (IsVaccinationGiven)
                        VaccinationGiven.TeoriticalQty = result.Sum(x => x.Quantity);
                    else
                        VaccinationPlan.TeoriticalQty = result.Sum(x => x.Quantity);
                }

                return;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SaveVaccinationPlan(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (GeneralConsultanService.PatientId is null)
                {
                    ToastService.ShowInfo("Please select the Patient");
                    e.Cancel = true;
                    return;
                }

                if (GeneralConsultanService.LocationId is null)
                {
                    ToastService.ShowInfo("Please select the Location");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationPlan.TeoriticalQty < 0)
                {
                    ToastService.ShowInfo("The Theoretical quantity must be greater than zero. Please adjust the quantity and try again.");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationPlan.Quantity > VaccinationPlan.TeoriticalQty)
                {
                    ToastService.ShowInfo("The quantity cannot exceed the theoretical quantity. Please enter a valid amount and try again.");
                    e.Cancel = true;
                    return;
                }

                VaccinationPlan.PatientId = GeneralConsultanService.PatientId.GetValueOrDefault();

                if (VaccinationPlan.Id == 0)
                    await Mediator.Send(new CreateVaccinationPlanRequest(VaccinationPlan));
                else
                    await Mediator.Send(new UpdateVaccinationPlanRequest(VaccinationPlan));

                await LoadVaccinationPlans();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task SaveVaccinationGiven(GridEditModelSavingEventArgs e)
        {
            try
            {
                if (GeneralConsultanService.PatientId is null)
                {
                    ToastService.ShowInfo("Please select the Patient");
                    e.Cancel = true;
                    return;
                }

                if (GeneralConsultanService.LocationId is null)
                {
                    ToastService.ShowInfo("Please select the Location");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationGiven.TeoriticalQty <= 0)
                {
                    ToastService.ShowInfo("The Theoretical quantity must be greater than zero. Please adjust the quantity and try again.");
                    e.Cancel = true;
                    return;
                }

                if (VaccinationGiven.Quantity > VaccinationGiven.TeoriticalQty)
                {
                    ToastService.ShowInfo("The quantity cannot exceed the theoretical quantity. Please enter a valid amount and try again.");
                    e.Cancel = true;
                    return;
                }

                VaccinationGiven.PatientId = GeneralConsultanService.PatientId.GetValueOrDefault();
                VaccinationGiven.GeneralConsultanServiceId = GeneralConsultanService.Id;
                VaccinationGiven.Status = EnumStatusVaccination.InProgress;

                if (VaccinationGiven.Id == 0)
                    await Mediator.Send(new CreateVaccinationPlanRequest(VaccinationGiven));
                else
                    await Mediator.Send(new UpdateVaccinationPlanRequest(VaccinationGiven));

                await LoadVaccinationGivens();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task NewItemDetailVaccinationGiven_Click()
        {
            if (GeneralConsultanService.PatientId == null || GeneralConsultanService.PatientId == 0)
            {
                ToastService.ClearInfoToasts();
                ToastService.ShowInfo("Please select the Patient first");
                return;
            }

            await GridVaccinationGiven.StartEditNewRowAsync();
        }

        private async Task NewItemDetailVaccinationPlan_Click()
        {
            if (GeneralConsultanService.PatientId == null || GeneralConsultanService.PatientId == 0)
            {
                ToastService.ClearInfoToasts();
                ToastService.ShowInfo("Please select the Patient first");
                return;
            }

            await GridVaccinationPlan.StartEditNewRowAsync();
        }

        private async Task EditItemDetailVaccinationGiven_Click(IGrid context)
        {
            // Ensure the context is not null and has selected data item
            if (context.SelectedDataItem != null)
            {
                VaccinationGiven = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == context.SelectedDataItem.Adapt<VaccinationPlanDto>().Id))).FirstOrDefault() ?? new();
                try
                {
                    var resultx = await Mediator.Send(new GetProductQueryNew
                    {
                        Predicate = x => x.Id == VaccinationGiven.ProductId
                    });
                    Products = resultx.Item1;

                    SalesPersons = (await Mediator.Send(new GetUserQueryNew
                    {
                        Predicate = x => x.Id == VaccinationGiven.SalesPersonId,
                        Select = x => new User
                        {
                            Id = x.Id,
                            Name = x.Name,
                            NoRm = x.NoRm,
                            Email = x.Email,
                            MobilePhone = x.MobilePhone,
                            Gender = x.Gender,
                            DateOfBirth = x.DateOfBirth,
                            NoId = x.NoId,
                            CurrentMobile = x.CurrentMobile
                        }
                    })).Item1;

                    SalesPersons = (await Mediator.Send(new GetUserQueryNew
                    {
                        Predicate = x => x.Id == VaccinationGiven.SalesPersonId,
                        Select = x => new User
                        {
                            Id = x.Id,
                            Name = x.Name,
                            NoRm = x.NoRm,
                            Email = x.Email,
                            MobilePhone = x.MobilePhone,
                            Gender = x.Gender,
                            DateOfBirth = x.DateOfBirth,
                            NoId = x.NoId,
                            CurrentMobile = x.CurrentMobile
                        }
                    })).Item1;

                    Educators = (await Mediator.Send(new GetUserQueryNew
                    {
                        Predicate = x => x.Id == VaccinationGiven.EducatorId,
                        Select = x => new User
                        {
                            Id = x.Id,
                            Name = x.Name,
                            NoRm = x.NoRm,
                            Email = x.Email,
                            MobilePhone = x.MobilePhone,
                            Gender = x.Gender,
                            DateOfBirth = x.DateOfBirth,
                            NoId = x.NoId,
                            CurrentMobile = x.CurrentMobile
                        }
                    })).Item1;

                    VaccinationGivenPhysicions = (await Mediator.Send(new GetUserQueryNew
                    {
                        Predicate = x => x.Id == VaccinationGiven.PhysicianId,
                        Select = x => new User
                        {
                            Id = x.Id,
                            Name = x.Name,
                            NoRm = x.NoRm,
                            Email = x.Email,
                            MobilePhone = x.MobilePhone,
                            Gender = x.Gender,
                            DateOfBirth = x.DateOfBirth,
                            NoId = x.NoId,
                            CurrentMobile = x.CurrentMobile
                        }
                    })).Item1;

                    if (VaccinationGiven.Product.TraceAbility)
                    {
                        var result = (await Mediator.Send(new GetTransactionStockQueryNew
                        {
                            Select = x => new TransactionStock
                            {
                                Batch = x.Batch,
                            },
                            IsGetAll = true,
                            Predicate = s => s.ProductId == VaccinationGiven.ProductId && s.LocationId == GeneralConsultanService.LocationId
                        })).Item1;

                        Batch = result?.Select(x => x.Batch)?.ToList() ?? [];
                        Batch = Batch.Distinct().ToList();
                    }

                    await SelectedBatchGiven(VaccinationGiven.Batch);
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
                StateHasChanged();
            }

            await GridVaccinationGiven.StartEditRowAsync(FocusedRowVisibleIndexVaccinationGiven);
        }

        private async Task EditItemDetailVaccinationPlan_Click(IGrid context)
        {
            // Ensure the context is not null and has selected data item
            if (context.SelectedDataItem != null)
            {
                VaccinationPlan = (await Mediator.Send(new GetVaccinationPlanQuery(x => x.Id == context.SelectedDataItem.Adapt<VaccinationPlanDto>().Id))).FirstOrDefault() ?? new();

                var resultx = await Mediator.Send(new GetProductQueryNew
                {
                    Predicate = x => x.Id == VaccinationPlan.ProductId
                });
                Products = resultx.Item1;

                SalesPersons = (await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.Id == VaccinationPlan.SalesPersonId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                })).Item1;

                SalesPersons = (await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.Id == VaccinationPlan.SalesPersonId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                })).Item1;

                Educators = (await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.Id == VaccinationPlan.EducatorId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                })).Item1;

                VaccinationGivenPhysicions = (await Mediator.Send(new GetUserQueryNew
                {
                    Predicate = x => x.Id == VaccinationPlan.PhysicianId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        NoRm = x.NoRm,
                        Email = x.Email,
                        MobilePhone = x.MobilePhone,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth,
                        NoId = x.NoId,
                        CurrentMobile = x.CurrentMobile
                    }
                })).Item1;

                if (VaccinationPlan.Product.TraceAbility)
                {
                    var result = (await Mediator.Send(new GetTransactionStockQueryNew
                    {
                        Select = x => new TransactionStock
                        {
                            Batch = x.Batch,
                        },
                        IsGetAll = true,
                        Predicate = s => s.ProductId == VaccinationPlan.ProductId && s.LocationId == GeneralConsultanService.LocationId
                    })).Item1;

                    Batch = result?.Select(x => x.Batch)?.ToList() ?? [];
                    Batch = Batch.Distinct().ToList();
                }

                await SelectedBatchPlan(VaccinationPlan.Batch);

                StateHasChanged();
            }

            await GridVaccinationPlan.StartEditRowAsync(FocusedRowVisibleIndexVaccinationPlan);
        }

        private async Task NewItemDetailGiven_Click()
        {
            //if (InventoryAdjusment.LocationId is null || InventoryAdjusment.LocationId == 0)
            //{
            //    ToastService.ClearInfoToasts();
            //    ToastService.ShowInfo("Please select the Location first.");
            //    return;
            //}

            //Products = await Mediator.Send(new GetProductQuery());
            //AllProducts = Products.Select(x => x).ToList();

            //FormInventoryAdjusmentDetail = new();
            //TotalQty = 0;
            //LotSerialNumber = "-";
            //UomId = null;

            //await GridDetail.StartEditNewRowAsync();
        }

        private int FocusedRowVisibleIndexVaccinationPlan { get; set; }
        private int FocusedRowVisibleIndexVaccinationGiven { get; set; }

        private void GridDetailVaccinationPlan_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexVaccinationPlan = args.VisibleIndex;
        }

        private void GridDetailVaccinationGiven_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndexVaccinationGiven = args.VisibleIndex;
        }

        private bool IsLoadingVaccinationPlan = false;
        private bool IsLoadingVaccinationGiven = false;
        private bool IsLoadingVaccinationHistoryGiven = false;

        private async Task LoadVaccinationHistoryGivens()
        {
            IsLoadingVaccinationHistoryGiven = true;
            VaccinationHistoryGivens = await Mediator.Send(new GetVaccinationPlanQuery(x => x.PatientId == GeneralConsultanService.PatientId && x.Status == EnumStatusVaccination.Done && (x.GeneralConsultanServiceId != 0 || x.GeneralConsultanServiceId != null)));
            IsLoadingVaccinationHistoryGiven = false;
        }

        private async Task LoadVaccinationPlans()
        {
            IsLoadingVaccinationPlan = true;
            VaccinationPlans = await Mediator.Send(new GetVaccinationPlanQuery(x => x.PatientId == GeneralConsultanService.PatientId && (x.GeneralConsultanServiceId == 0 || x.GeneralConsultanServiceId == null)));
            IsVaccinationGiven = false;
            IsLoadingVaccinationPlan = false;
        }

        private async Task LoadVaccinationGivens()
        {
            IsLoadingVaccinationGiven = true;
            VaccinationGivens = await Mediator.Send(new GetVaccinationPlanQuery(x => x.PatientId == GeneralConsultanService.PatientId && x.GeneralConsultanServiceId != null && x.GeneralConsultanServiceId == GeneralConsultanService.Id));
            await LoadDataVaccinationGiven();
            IsVaccinationGiven = true;
            IsLoadingVaccinationGiven = false;
        }

        private async Task OnDeleteVaccinationGiven(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataVaccinationGivenItems is null || SelectedDataVaccinationGivenItems.Count == 1)
                {
                    await Mediator.Send(new DeleteVaccinationPlanRequest(((VaccinationPlanDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataVaccinationGivenItems.Adapt<List<VaccinationPlanDto>>();
                    await Mediator.Send(new DeleteVaccinationPlanRequest(ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataVaccinationGivenItems = [];
                await LoadVaccinationGivens();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task OnDeleteVaccinationPlan(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataVaccinationPlanItems is null || SelectedDataVaccinationPlanItems.Count == 1)
                {
                    await Mediator.Send(new DeleteVaccinationPlanRequest(((VaccinationPlanDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataVaccinationPlanItems.Adapt<List<VaccinationPlanDto>>();
                    await Mediator.Send(new DeleteVaccinationPlanRequest(ids: a.Select(x => x.Id).ToList()));
                }
                SelectedDataVaccinationPlanItems = [];
                await LoadVaccinationPlans();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion Vaccination Tabs

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

        #region Binding

        private List<UserDto> Physicions { get; set; } = [];
        private List<UserDto> Patients { get; set; } = [];
        private List<ServiceDto> Services { get; set; } = [];

        private List<LocationDto> Locations { get; set; } = [];
        private List<InsurancePolicyDto> InsurancePolicies { get; set; } = [];
        private List<InsurancePolicyDto> ReferToInsurancePolicies { get; set; } = [];
        private List<string> RiskOfFallingDetail = [];

        private List<AwarenessDto> Awareness { get; set; } = [];
        private List<AllergyDto> WeatherAllergies = [];
        private List<AllergyDto> FoodAllergies = [];
        private List<AllergyDto> PharmacologyAllergies = [];

        private IEnumerable<AllergyDto> SelectedWeatherAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedFoodAllergies { get; set; } = [];
        private IEnumerable<AllergyDto> SelectedPharmacologyAllergies { get; set; } = [];

        private string FormUrl = "clinic-service/vaccinations";
        private bool PanelVisible = false;
        private bool IsLoading = false;
        [Parameter] public string PageMode { get; set; } = EnumPageMode.Create.GetDisplayName();

        private bool IsStatus(EnumStatusGeneralConsultantService status) => GeneralConsultanService.Status == status;

        private EnumStatusGeneralConsultantService StagingText { get; set; } = EnumStatusGeneralConsultantService.Confirmed;
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();
        private UserDto UserForm { get; set; } = new();
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport { get; set; } = new();

        #endregion Binding

        #region CPPT

        private IGrid GridCppt { get; set; }
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = [];
        private int FocusedGridTabCPPTRowVisibleIndex { get; set; }
        private List<DiagnosisDto> Diagnoses = [];
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];
        private List<NursingDiagnosesDto> NursingDiagnoses = [];

        private async Task NewItemCPPT_Click()
        {
            await GridCppt.StartEditNewRowAsync();
        }

        private async Task RefreshCPPT_Click()
        {
            await LoadDataCPPT();
        }

        #region Searching

        private int pageSizeGridCPPT { get; set; } = 10;
        private int totalCountGridCPPT = 0;
        private int activePageIndexTotalCountGridCPPT { get; set; } = 0;
        private string searchTermGridCPPT { get; set; } = string.Empty;

        private async Task OnSearchBoxChangedGridCPPT(string searchText)
        {
            searchTermGridCPPT = searchText;
            await LoadDataCPPT(0, pageSizeGridCPPT);
        }

        private async Task OnpageSizeGridCPPTIndexChangedGridCPPT(int newpageSizeGridCPPT)
        {
            pageSizeGridCPPT = newpageSizeGridCPPT;
            await LoadDataCPPT(0, newpageSizeGridCPPT);
        }

        private async Task OnPageIndexChangedGridCPPT(int newPageIndex)
        {
            await LoadDataCPPT(newPageIndex, pageSizeGridCPPT);
        }

        #endregion Searching

        private async Task LoadDataCPPT(int pageIndex = 0, int pageSizeGridCPPT = 10)
        {
            try
            {
                PanelVisible = true;
                SelectedDataItemsCPPT = [];
                var ab = await Mediator.Send(new GetGeneralConsultanCPPTsQuery
                {
                    SearchTerm = searchTermGridCPPT ?? "",
                    Predicate = x => x.GeneralConsultanServiceId == GeneralConsultanService.Id
                });
                GeneralConsultanCPPTs = ab.Item1;
                totalCountGridCPPT = ab.PageCount;
                activePageIndexTotalCountGridCPPT = pageIndex;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task EditItemCPPT_Click()
        {
            try
            {
                PanelVisible = true;
                await GridCppt.StartEditRowAsync(FocusedGridTabCPPTRowVisibleIndex);

                var a = (GridCppt.GetDataItem(FocusedGridTabCPPTRowVisibleIndex) as GeneralConsultanCPPTDto ?? new());
                NursingDiagnoses = (await Mediator.Send(new GetNursingDiagnosesQuery
                {
                    Predicate = x => x.Id == a.NursingDiagnosesId
                })).Item1;
                Diagnoses = (await Mediator.Send(new GetDiagnosisQuery
                {
                    Predicate = x => x.Id == a.DiagnosisId
                })).Item1;

                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItemCPPT_Click()
        {
            GridCppt.ShowRowDeleteConfirmation(FocusedGridTabCPPTRowVisibleIndex);
        }

        private void GridCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        private void GridTabCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        private async Task OnSaveCPPT(GridEditModelSavingEventArgs e)
        {
            try
            {
                PanelVisible = true;

                var editModel = (GeneralConsultanCPPTDto)e.EditModel;

                editModel.GeneralConsultanServiceId = GeneralConsultanService.Id;

                if (editModel.Id == 0)
                {
                    await Mediator.Send(new CreateGeneralConsultanCPPTRequest(editModel));
                }
                else
                {
                    await Mediator.Send(new UpdateGeneralConsultanCPPTRequest(editModel));
                }

                await LoadDataCPPT(activePageIndexTotalCountGridCPPT, pageSizeGridCPPT);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnDeleteCPPT(GridDataItemDeletingEventArgs e)
        {
            try
            {
                PanelVisible = true;
                if (SelectedDataItemsCPPT.Count == 0)
                {
                    await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(((GeneralConsultanCPPTDto)e.DataItem).Id));
                }
                else
                {
                    var selectedGeneralConsultanCPPTs = SelectedDataItemsCPPT.Adapt<List<GeneralConsultanCPPTDto>>();
                    await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(ids: selectedGeneralConsultanCPPTs.Select(x => x.Id).ToList()));
                }

                await LoadDataCPPT(activePageIndexTotalCountGridCPPT, pageSizeGridCPPT);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion CPPT

        private void KeyPressHandler(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                return;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();
            await LoadData();
            PanelVisible = false;
        }

        [SupplyParameterFromQuery] public long? Id { get; set; }

        private bool ReadOnlyForm()
        {
            var a = ((
                GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned) ||
                GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.NurseStation) ||
                GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician)
                ));

            return !a;
        }

        private async Task<GeneralConsultanServiceDto> GetGeneralConsultanServiceById()
        {
            var result = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
            {
                Predicate = x => x.Id == this.Id,

                Select = x => new GeneralConsultanService
                {
                    Id = x.Id,
                    Status = x.Status,
                    PatientId = x.PatientId,
                    Patient = new User
                    {
                        Id = x.PatientId.GetValueOrDefault(),
                        Name = x.Patient == null ? string.Empty : x.Patient.Name,
                        NoRm = x.Patient == null ? string.Empty : x.Patient.NoRm,
                        NoId = x.Patient == null ? string.Empty : x.Patient.NoId,
                        CurrentMobile = x.Patient == null ? string.Empty : x.Patient.CurrentMobile,
                        DateOfBirth = x.Patient == null ? null : x.Patient.DateOfBirth,

                        IsWeatherPatientAllergyIds = x.Patient != null && x.Patient.IsWeatherPatientAllergyIds,
                        IsFoodPatientAllergyIds = x.Patient != null && x.Patient.IsFoodPatientAllergyIds,
                        IsPharmacologyPatientAllergyIds = x.Patient == null ? false : x.Patient.IsPharmacologyPatientAllergyIds,
                        WeatherPatientAllergyIds = x.Patient == null ? new() : x.Patient.WeatherPatientAllergyIds,
                        FoodPatientAllergyIds = x.Patient == null ? new() : x.Patient.FoodPatientAllergyIds,
                        PharmacologyPatientAllergyIds = x.Patient == null ? new() : x.Patient.PharmacologyPatientAllergyIds,

                        IsFamilyMedicalHistory = x.Patient == null ? string.Empty : x.Patient.IsFamilyMedicalHistory,
                        FamilyMedicalHistory = x.Patient == null ? string.Empty : x.Patient.FamilyMedicalHistory,
                        FamilyMedicalHistoryOther = x.Patient == null ? string.Empty : x.Patient.FamilyMedicalHistoryOther,

                        IsMedicationHistory = x.Patient == null ? string.Empty : x.Patient.IsMedicationHistory,
                        MedicationHistory = x.Patient == null ? string.Empty : x.Patient.MedicationHistory,
                        PastMedicalHistory = x.Patient == null ? string.Empty : x.Patient.PastMedicalHistory,

                        Gender = x.Patient == null ? string.Empty : x.Patient.Gender
                    },
                    PratitionerId = x.PratitionerId,
                    Pratitioner = new User
                    {
                        Name = x.Pratitioner == null ? string.Empty : x.Pratitioner.Name,
                    },
                    ServiceId = x.ServiceId,
                    Service = new Service
                    {
                        Name = x.Service == null ? string.Empty : x.Service.Name,
                    },
                    Payment = x.Payment,
                    LocationId = x.LocationId,
                    InsurancePolicyId = x.InsurancePolicyId,
                    AppointmentDate = x.AppointmentDate,
                    IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                    RegistrationDate = x.RegistrationDate,
                    ClassType = x.ClassType,
                    TypeRegistration = x.TypeRegistration,

                    InformationFrom = x.InformationFrom,
                    ClinicVisitTypes = x.ClinicVisitTypes,
                    AwarenessId = x.AwarenessId,
                    Weight = x.Weight,
                    Height = x.Height,
                    RR = x.RR,
                    SpO2 = x.SpO2,
                    WaistCircumference = x.WaistCircumference,
                    BMIIndex = x.BMIIndex,
                    BMIIndexString = x.BMIIndexString,
                    ScrinningTriageScale = x.ScrinningTriageScale,
                    E = x.E,
                    V = x.V,
                    M = x.M,
                    Temp = x.Temp,
                    HR = x.HR,
                    Systolic = x.Systolic,
                    DiastolicBP = x.DiastolicBP,
                    PainScale = x.PainScale,
                    BMIState = x.BMIState,
                    RiskOfFalling = x.RiskOfFalling,
                    RiskOfFallingDetail = x.RiskOfFallingDetail,
                    Reference = x.Reference,
                    HomeStatus = x.HomeStatus,
                    IsSickLeave = x.IsSickLeave,
                    StartDateSickLeave = x.StartDateSickLeave,
                    EndDateSickLeave = x.EndDateSickLeave,
                    IsMaternityLeave = x.IsMaternityLeave,
                    StartMaternityLeave = x.StartMaternityLeave,
                    EndMaternityLeave = x.EndMaternityLeave,

                    PPKRujukanCode = x.PPKRujukanCode,
                    PPKRujukanName = x.PPKRujukanName,
                    ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                    ReferVerticalSpesialisParentSubSpesialisName = x.ReferVerticalSpesialisParentSubSpesialisName,
                    ReferReason = x.ReferReason,
                }
            });

            if (result.Status == EnumStatusGeneralConsultantService.NurseStation || result.Status == EnumStatusGeneralConsultantService.NurseStation)
            {
                result = await GetClinicalAssesmentPatientHistory(result);
            }

            return result;
        }

        private async Task LoadData()
        {
            if (PageMode == EnumPageMode.Update.GetDisplayName())
            {
                var result = await GetGeneralConsultanServiceById();

                GeneralConsultanService = new();

                if (result is null || !Id.HasValue)
                {
                    NavigationManager.NavigateTo(FormUrl);
                    return;
                }

                GeneralConsultanService = result;
                UserForm = result.Patient ?? new();

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        StagingText = EnumStatusGeneralConsultantService.Confirmed;
                        break;

                    case EnumStatusGeneralConsultantService.Confirmed:
                        StagingText = EnumStatusGeneralConsultantService.NurseStation;
                        break;

                    case EnumStatusGeneralConsultantService.NurseStation:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        break;

                    case EnumStatusGeneralConsultantService.Waiting:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        break;

                    case EnumStatusGeneralConsultantService.Finished:
                        StagingText = EnumStatusGeneralConsultantService.Finished;
                        GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        break;

                    case EnumStatusGeneralConsultantService.Canceled:
                        StagingText = EnumStatusGeneralConsultantService.Canceled;
                        break;

                    default:
                        break;
                }
                await LoadPatient(predicate: x => x.IsPatient == true && x.Id == GeneralConsultanService.PatientId);
                await LoadService(predicate: x => x.Id == GeneralConsultanService.ServiceId);
                await LoadLocation(predicate: x => x.Id == GeneralConsultanService.LocationId);

                if (!IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    await LoadPhysicion(predicate: x => x.IsDoctor == true && x.Id == GeneralConsultanService.PratitionerId && x.IsPhysicion == true);
                }

                if (!string.IsNullOrWhiteSpace(GeneralConsultanService.Payment))
                {
                    await LoadDataInsurancePolicy(predicate: x => x.Id == GeneralConsultanService.InsurancePolicyId);
                }

                if (GeneralConsultanService.RiskOfFalling == "Humpty Dumpty")
                {
                    RiskOfFallingDetail = [.. Helper.HumptyDumpty];
                }
                else if (GeneralConsultanService.RiskOfFalling == "Morse")
                {
                    RiskOfFallingDetail = [.. Helper.Morse];
                }
                else
                {
                    RiskOfFallingDetail = [.. Helper.Geriati];
                }
            }
            else
            {
                await LoadPatient();
                await LoadService();
                await LoadPhysicion();
                await LoadLocation();
            }

            #region Get Patient Allergies

            var alergy = (await Mediator.Send(new GetAllergyQuery()));
            FoodAllergies = alergy.Where(x => x.Type == "01").ToList();
            WeatherAllergies = alergy.Where(x => x.Type == "02").ToList();
            PharmacologyAllergies = alergy.Where(x => x.Type == "03").ToList();

            SelectedFoodAllergies = FoodAllergies.Where(x => UserForm.FoodPatientAllergyIds.Contains(x.Id));
            SelectedWeatherAllergies = WeatherAllergies.Where(x => UserForm.WeatherPatientAllergyIds.Contains(x.Id));
            SelectedPharmacologyAllergies = PharmacologyAllergies.Where(x => UserForm.PharmacologyPatientAllergyIds.Contains(x.Id));

            #endregion Get Patient Allergies

            Awareness = await Mediator.Send(new GetAwarenessQuery());
        }

        private async Task OnClickTabCPPT()
        {
            await LoadDataCPPT();

            if (IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
            {
                await LoadDataNursingDiagnoses();
                await LoadDataDiagnoses();
            }
        }

        #region ComboBox NursingDiagnoses

        private NursingDiagnosesDto SelectedNursingDiagnoses { get; set; } = new();

        private async Task SelectedItemChanged(NursingDiagnosesDto e)
        {
            if (e is null)
            {
                SelectedNursingDiagnoses = new();
                await LoadDataNursingDiagnoses();
            }
            else
                SelectedNursingDiagnoses = e;
        }

        private CancellationTokenSource? _ctsNursingDiagnoses;

        private async Task OnInputNursingDiagnoses(ChangeEventArgs e)
        {
            try
            {
                _ctsNursingDiagnoses?.Cancel();
                _ctsNursingDiagnoses?.Dispose();
                _ctsNursingDiagnoses = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsNursingDiagnoses.Token);

                await LoadDataNursingDiagnoses(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsNursingDiagnoses?.Dispose();
                _ctsNursingDiagnoses = null;
            }
        }

        private async Task LoadDataNursingDiagnoses(string? e = "", Expression<Func<NursingDiagnoses, bool>>? predicate = null)
        {
            try
            {
                NursingDiagnoses = await Mediator.QueryGetComboBox<NursingDiagnoses, NursingDiagnosesDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox NursingDiagnoses

        #region ComboBox InsurancePolicy

        private InsurancePolicyDto SelectedInsurancePolicy { get; set; } = new();

        private async Task SelectedItemChanged(InsurancePolicyDto e)
        {
            if (e is null)
            {
                SelectedInsurancePolicy = new();
                await LoadDataInsurancePolicy();
            }
            else
                SelectedInsurancePolicy = e;
        }

        private CancellationTokenSource? _ctsInsurancePolicy;

        private async Task OnInputInsurancePolicy(ChangeEventArgs e)
        {
            try
            {
                _ctsInsurancePolicy?.Cancel();
                _ctsInsurancePolicy?.Dispose();
                _ctsInsurancePolicy = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsInsurancePolicy.Token);

                await LoadDataInsurancePolicy(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsInsurancePolicy?.Dispose();
                _ctsInsurancePolicy = null;
            }
        }

        private async Task LoadDataInsurancePolicy(string? e = "", Expression<Func<InsurancePolicy, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJS == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true;

                string input = e ?? "";
                string b = input.Split('-')[0].Trim();

                InsurancePolicies = await Mediator.QueryGetComboBox<InsurancePolicy, InsurancePolicyDto>(e, predicate, select: x => new InsurancePolicy
                {
                    Id = x.Id,
                    PolicyNumber = x.PolicyNumber,
                    Insurance = new Insurance
                    {
                        Name = x.Insurance == null ? "" : x.Insurance.Name,
                    },
                    NoKartu = x.NoKartu,
                    KdProviderPstKdProvider = x.KdProviderPstKdProvider,
                    PstPrb = x.PstPrb,
                    PstProl = x.PstProl
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox InsurancePolicy

        private void OnSelectRiskOfFalling(string e)
        {
            RiskOfFallingDetail.Clear();
            GeneralConsultanService.RiskOfFallingDetail = null;
            if (e is null)
            {
                return;
            }

            if (e == "Humpty Dumpty")
            {
                RiskOfFallingDetail = Helper.HumptyDumpty.ToList();
            }
            else if (e == "Morse")
            {
                RiskOfFallingDetail = Helper.Morse.ToList();
            }
            else
            {
                RiskOfFallingDetail = Helper.Geriati.ToList();
            }
        }

        #region ComboBox Diagnosis

        private DiagnosisDto SelectedDiagnosis { get; set; } = new();

        private async Task SelectedItemChanged(DiagnosisDto e)
        {
            if (e is null)
            {
                SelectedDiagnosis = new();
                await LoadDataDiagnoses();
            }
            else
                SelectedDiagnosis = e;
        }

        private CancellationTokenSource? _ctsDiagnosis;

        private async Task OnInputDiagnosis(ChangeEventArgs e)
        {
            try
            {
                _ctsDiagnosis?.Cancel();
                _ctsDiagnosis?.Dispose();
                _ctsDiagnosis = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsDiagnosis.Token);

                await LoadDataDiagnoses(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsDiagnosis?.Dispose();
                _ctsDiagnosis = null;
            }
        }

        private async Task LoadDataDiagnoses(string? e = "", Expression<Func<Diagnosis, bool>>? predicate = null)
        {
            try
            {
                Diagnoses = await Mediator.QueryGetComboBox<Diagnosis, DiagnosisDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Diagnosis

        #region ComboBox Patient

        private async Task SelectedItemChanged(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.InsurancePolicyId = null;
                GeneralConsultanService.PatientId = null;
                GeneralConsultanService.Patient = new();
                UserForm = new();

                InsurancePolicies = [];
                await LoadPatient();
            }
            else
            {
                GeneralConsultanService.PatientId = e.Id;
                GeneralConsultanService.Patient = e;
                UserForm = e;
                await LoadDataInsurancePolicy();
            }
        }

        private CancellationTokenSource? _ctsPatient;

        private async Task OnInputPatient(ChangeEventArgs e)
        {
            try
            {
                _ctsPatient?.Cancel();
                _ctsPatient?.Dispose();
                _ctsPatient = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsPatient.Token);

                await LoadPatient(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsPatient?.Dispose();
                _ctsPatient = null;
            }
        }

        private async Task LoadPatient(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsPatient == true;

                Patients = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate, select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    NoRm = x.NoRm,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    DateOfBirth = x.DateOfBirth,
                    NoId = x.NoId,
                    CurrentMobile = x.CurrentMobile,

                    IsWeatherPatientAllergyIds = x.IsWeatherPatientAllergyIds,
                    IsFoodPatientAllergyIds = x.IsFoodPatientAllergyIds,
                    IsPharmacologyPatientAllergyIds = x.IsPharmacologyPatientAllergyIds,
                    WeatherPatientAllergyIds = x.WeatherPatientAllergyIds,
                    FoodPatientAllergyIds = x.FoodPatientAllergyIds,
                    PharmacologyPatientAllergyIds = x.PharmacologyPatientAllergyIds,

                    IsFamilyMedicalHistory = x.IsFamilyMedicalHistory,
                    FamilyMedicalHistory = x.FamilyMedicalHistory,
                    FamilyMedicalHistoryOther = x.FamilyMedicalHistoryOther,

                    IsMedicationHistory = x.IsMedicationHistory,
                    MedicationHistory = x.MedicationHistory,
                    PastMedicalHistory = x.PastMedicalHistory
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Patient

        private void SelectedDateMaternityChanged(DateTime? e)
        {
            GeneralConsultanService.EndMaternityLeave = null;

            if (e is null)
                return;

            GeneralConsultanService.StartMaternityLeave = e;
            GeneralConsultanService.EndMaternityLeave = e.GetValueOrDefault().AddMonths(3).Date;
        }

        #region ComboBox Service

        private ServiceDto SelectedService { get; set; } = new();

        private async Task SelectedItemChanged(ServiceDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.ServiceId = null;
                await LoadService();
                Physicions = [];
            }
            else
            {
                GeneralConsultanService.ServiceId = e.Id;
                await LoadPhysicion();
            }
        }

        private CancellationTokenSource? _ctsService;

        private async Task OnInputService(ChangeEventArgs e)
        {
            try
            {
                _ctsService?.Cancel();
                _ctsService?.Dispose();
                _ctsService = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsService.Token);

                await LoadService(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsService?.Dispose();
                _ctsService = null;
            }
        }

        private async Task LoadService(string? e = "", Expression<Func<Service, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsVaccination == true;
                Services = await Mediator.QueryGetComboBox<Service, ServiceDto>(e, predicate);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Service

        #region ComboBox Physicion

        private UserDto SelectedPhysicion { get; set; } = new();

        private async Task SelectedItemChangedPhysicion(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.PratitionerId = null;
                await LoadPhysicion();
            }
            else
                GeneralConsultanService.PratitionerId = e.Id;
        }

        private CancellationTokenSource? _ctsPhysicion;

        private async Task OnInputPhysicion(ChangeEventArgs e)
        {
            try
            {
                _ctsPhysicion?.Cancel();
                _ctsPhysicion?.Dispose();
                _ctsPhysicion = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsPhysicion.Token);

                await LoadPhysicion(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsPhysicion?.Dispose();
                _ctsPhysicion = null;
            }
        }

        private async Task LoadPhysicion(string? e = "", Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                predicate ??= x => x.IsDoctor == true && x.IsPhysicion == true && x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(GeneralConsultanService.ServiceId.GetValueOrDefault());

                Physicions = await Mediator.QueryGetComboBox<User, UserDto>(e, predicate, select: x => new User
                {
                    Id = x.Id,
                    Name = x.Name,
                    NoRm = x.NoRm,
                    Email = x.Email,
                    MobilePhone = x.MobilePhone,
                    Gender = x.Gender,
                    PhysicanCode = x.PhysicanCode,
                    DateOfBirth = x.DateOfBirth,
                    NoId = x.NoId,
                    CurrentMobile = x.CurrentMobile
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Physicion

        private async Task SelectedItemPaymentChanged(string e)
        {
            GeneralConsultanService.Payment = null;
            GeneralConsultanService.InsurancePolicyId = null;

            if (e is null)
                return;

            InsurancePolicies = (await Mediator.Send(new GetInsurancePolicyQuery
            {
                Predicate = x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJS == e.Equals("BPJS") && x.Active == true,
                Select = x => new InsurancePolicy
                {
                    Id = x.Id,
                    Insurance = new Insurance
                    {
                        Name = x.Insurance == null ? "" : x.Insurance.Name,
                    },
                    PolicyNumber = x.PolicyNumber,
                    PstPrb = x.PstPrb,
                    PstProl = x.PstProl
                }
            })).Item1;
        }

        #region OnClick

        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private async Task OnCancelStatus()
        {
            try
            {
                IsLoading = true;

                if (GeneralConsultanService.Id != 0)
                {
                    if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0)
                    {
                        //var isSuccess = await SendPCareRequestUpdateStatusPanggilAntrean(2);
                        //if (!isSuccess)
                        //{
                        //    IsLoading = false;
                        //    return;
                        //}
                    }

                    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Canceled;
                    GeneralConsultanService = await Mediator.Send(new CancelGeneralConsultanServiceRequest(GeneralConsultanService));
                    StagingText = EnumStatusGeneralConsultantService.Canceled;

                    ToastService.ShowSuccess("The patient has been successfully canceled from the consultation.");
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task HandleValidSubmit()
        {
            IsLoading = true;

            try
            {
                // Execute the validator
                ValidationResult results = new GeneralConsultanServiceValidator().Validate(GeneralConsultanService);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                ToastService.ClearInfoToasts();
                if (!success)
                {
                    foreach (var f in failures)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                // Execute the validator
                ValidationResult results2 = new GCGUserFormValidator().Validate(UserForm);

                // Inspect any validation failures.
                bool success2 = results2.IsValid;
                List<ValidationFailure> failures2 = results2.Errors;

                if (!success2)
                {
                    foreach (var f in failures2)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                if (!success2 || !success)
                    return;

                GeneralConsultanService.IsVaccination = true;

                if (!GeneralConsultanService.Payment!.Equals("Personal") && (GeneralConsultanService.InsurancePolicyId is null || GeneralConsultanService.InsurancePolicyId == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                UserForm.WeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds
                    ? SelectedWeatherAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.PharmacologyPatientAllergyIds = UserForm.IsPharmacologyPatientAllergyIds
                    ? SelectedPharmacologyAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.FoodPatientAllergyIds = UserForm.IsFoodPatientAllergyIds
                    ? SelectedFoodAllergies.Select(x => x.Id).ToList()
                    : [];

                GeneralConsultanServiceDto res = new();

                switch (GeneralConsultanService.Status)
                {
                    case EnumStatusGeneralConsultantService.Planned:
                        var patient = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                        {
                            Includes = [x => x.Patient],
                            Select = x => new GeneralConsultanService
                            {
                                Patient = new User { Name = x.Patient.Name },
                            },
                            Predicate = x => x.Id != GeneralConsultanService.Id
                                          && x.IsGC == true
                                          && x.ServiceId == GeneralConsultanService.ServiceId
                                          && x.PatientId == GeneralConsultanService.PatientId
                                          && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned)
                                          && x.RegistrationDate != null
                                          && x.RegistrationDate.Value.Date <= DateTime.Now.Date
                        });

                        if (patient is not null)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient.Patient?.Name}\" still has a pending transaction.");
                            return;
                        }

                        break;

                    case EnumStatusGeneralConsultantService.Physician:
                        if (GeneralConsultanService.IsSickLeave == true || GeneralConsultanService.IsMaternityLeave == true)
                        {
                            //var checkDataSickLeave = await Mediator.Send(new GetSickLeaveQuery(x => x.GeneralConsultansId == GeneralConsultanService.Id));
                            //if (checkDataSickLeave is null || checkDataSickLeave.Count == 0)
                            //{
                            //    var leaveType = GeneralConsultanService.IsSickLeave == true ? "SickLeave" : "Maternity";
                            //    SickLeaves.TypeLeave = leaveType;
                            //    SickLeaves.GeneralConsultansId = GeneralConsultanService.Id;
                            //    await Mediator.Send(new CreateSickLeaveRequest(SickLeaves));
                            //}
                        }
                        break;

                    default:
                        break;
                }

                await HandleGeneralConsultationSaveAsync(GeneralConsultanService, UserForm);

                // Handle user login validation
                if (UserLogin.Id == GeneralConsultanService.PatientId)
                {
                    var user = await Mediator.Send(new GetSingleUserQuery
                    {
                        Predicate = x => x.Id == UserForm.Id,
                        Select = x => new User { Id = x.Id, Name = x.Name }
                    });

                    if (user != null)
                    {
                        await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);

                        var authProvider = (CustomAuthenticationStateProvider)CustomAuth;
                        await authProvider.UpdateAuthState(string.Empty);

                        await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(user)), 2);
                    }
                }

                // Refactored Save Logic
                async Task HandleGeneralConsultationSaveAsync(GeneralConsultanServiceDto service, UserDto userForm)
                {
                    if (GeneralConsultanService.Id == 0)
                    {
                        var createRequest = new CreateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = service,
                            Status = service.Status,
                            UserDto = new UserDto
                            {
                                Id = service.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = userForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = userForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = userForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = userForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = userForm.IsMedicationHistory,
                                FamilyMedicalHistory = userForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = userForm.FamilyMedicalHistoryOther,
                                MedicationHistory = userForm.MedicationHistory,
                                PastMedicalHistory = userForm.PastMedicalHistory,
                                CurrentMobile = userForm.CurrentMobile
                            }
                        };

                        res = await Mediator.Send(createRequest);
                    }
                    else
                    {
                        var updateRequest = new UpdateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = service,
                            Status = service.Status,
                            UserDto = new UserDto
                            {
                                Id = service.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = userForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = userForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = userForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = userForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = userForm.IsMedicationHistory,
                                FamilyMedicalHistory = userForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = userForm.FamilyMedicalHistoryOther,
                                MedicationHistory = userForm.MedicationHistory,
                                PastMedicalHistory = userForm.PastMedicalHistory,
                                CurrentMobile = userForm.CurrentMobile
                            }
                        };

                        res = await Mediator.Send(updateRequest);
                    }
                }

                ToastService.ClearSuccessToasts();
                ToastService.ShowSuccess("Saved Successfully");

                Id = res.Id;
                GeneralConsultanService = await GetGeneralConsultanServiceById();

                if (PageMode == EnumPageMode.Create.GetDisplayName())
                    NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");
            }
            catch (Exception x)
            {
                x.HandleException(ToastService);
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private void OnCancelBack()
        {
            NavigationManager.NavigateTo(FormUrl);
        }

        private bool PopUpConfirmation = false;
        private bool IsContinueCPPT = false;

        private async Task OnPopupConfirmed(bool confirmed)
        {
            PopUpConfirmation = false;

            if (confirmed)
            {
                IsContinueCPPT = true;

                await OnClickConfirm(true);
            }
            else
            {
                IsContinueCPPT = false;
            }
        }

        private async Task<GeneralConsultanServiceDto> GetClinicalAssesmentPatientHistory(GeneralConsultanServiceDto result)
        {
            try
            {
                if (result.Height == 0 && result.Weight == 0)
                {
                    //var prev = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x
                    //    => x.PatientId == result.PatientId && x.Id < result.Id && x.Status == EnumStatusGeneralConsultantService.Finished))).Item1
                    //    .OrderByDescending(x => x.CreatedDate)
                    //    .FirstOrDefault() ?? new();

                    var a = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                    {
                        Select = x => new GeneralConsultanService
                        {
                            Weight = x.Weight,
                            Height = x.Height
                        },
                        Predicate = x => x.PatientId == result.PatientId && x.Id < result.Id && x.Status == EnumStatusGeneralConsultantService.Finished,
                        OrderByList =
                        [
                            (x => x.CreatedDate, true),
                        ],
                        IsDescending = true
                    });

                    if (a is not null)
                    {
                        result.Height = a?.Height ?? 0;
                        result.Weight = a?.Weight ?? 0;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            return result;
        }

        private async Task OnClickConfirm(bool? clickConfirm = false, bool? isPopUpCPPT = false)
        {
            IsLoading = true;
            try
            {
                // Execute the validator
                ValidationResult results = new GeneralConsultanServiceValidator().Validate(GeneralConsultanService);

                // Inspect any validation failures.
                bool success = results.IsValid;
                List<ValidationFailure> failures = results.Errors;

                ToastService.ClearInfoToasts();
                if (!success)
                {
                    foreach (var f in failures)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                // Execute the validator
                ValidationResult results2 = new GCGUserFormValidator().Validate(UserForm);

                // Inspect any validation failures.
                bool success2 = results2.IsValid;
                List<ValidationFailure> failures2 = results2.Errors;

                if (!success2)
                {
                    foreach (var f in failures2)
                    {
                        ToastService.ShowInfo(f.ErrorMessage);
                    }
                }

                if (!success2 || !success)
                    return;

                GeneralConsultanService.IsVaccination = true;

                if (!GeneralConsultanService.Payment!.Equals("Personal") && (GeneralConsultanService.InsurancePolicyId is null || GeneralConsultanService.InsurancePolicyId == 0))
                {
                    IsLoading = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                UserForm.WeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds
                    ? SelectedWeatherAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.PharmacologyPatientAllergyIds = UserForm.IsPharmacologyPatientAllergyIds
                    ? SelectedPharmacologyAllergies.Select(x => x.Id).ToList()
                    : [];

                UserForm.FoodPatientAllergyIds = UserForm.IsFoodPatientAllergyIds
                    ? SelectedFoodAllergies.Select(x => x.Id).ToList()
                    : [];

                if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Planned))
                {
                    //var isSuccess = await SendPcareRequestRegistration();
                    //if (!isSuccess)
                    //{
                    //    IsLoading = false;
                    //    return;
                    //}
                    //else
                    //{
                    //    await SendPCareRequestUpdateStatusPanggilAntrean(1);
                    //}
                }

                if (GeneralConsultanService.InsurancePolicyId is not null && GeneralConsultanService.InsurancePolicyId != 0 && GeneralConsultanService.Status.Equals(EnumStatusGeneralConsultantService.Physician))
                {
                    //var isSuccessAddKunjungan = await SendPcareRequestKunjungan();

                    //if (!isSuccessAddKunjungan)
                    //{
                    //    IsLoading = false;
                    //    return;
                    //}
                }

                if (IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    var isExistingCPPTs = await Mediator.Send(new CheckExistingGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                    if (!isExistingCPPTs)
                    {
                        if (IsContinueCPPT)
                            PopUpConfirmation = false;
                        else
                        {
                            PopUpConfirmation = true;
                            return;
                        }
                    }
                    else
                        IsContinueCPPT = true;
                }
                else
                    IsContinueCPPT = true;

                if ((!PopUpConfirmation && IsContinueCPPT) || IsStatus(EnumStatusGeneralConsultantService.NurseStation) || IsStatus(EnumStatusGeneralConsultantService.Physician))
                {
                    // Fetch existing patient with planned status
                    if (GeneralConsultanService.Status == EnumStatusGeneralConsultantService.Planned)
                    {
                        var patient = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                        {
                            Includes = [x => x.Patient],
                            Select = x => new GeneralConsultanService { Patient = new User { Name = x.Patient.Name } },
                            Predicate = x => x.Id != GeneralConsultanService.Id
                                          && x.ServiceId == GeneralConsultanService.ServiceId
                                          && x.PatientId == GeneralConsultanService.PatientId
                                          && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned)
                                          && x.RegistrationDate <= DateTime.Now.Date
                        });

                        if (patient is not null)
                        {
                            IsLoading = false;
                            ToastService.ShowInfo($"Patient in the name of \"{patient.Patient?.Name}\" still has a pending transaction");
                            return;
                        }

                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Confirmed;
                    }

                    GeneralConsultanServiceDto newGC;

                    //StagingText = GeneralConsultanService.Status == EnumStatusGeneralConsultantService.Confirmed
                    //   ? EnumStatusGeneralConsultantService.NurseStation
                    //   : StagingText;

                    // Handle status changes
                    switch (StagingText)
                    {
                        case EnumStatusGeneralConsultantService.Confirmed:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Confirmed;
                            StagingText = EnumStatusGeneralConsultantService.NurseStation;
                            break;

                        case EnumStatusGeneralConsultantService.NurseStation:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.NurseStation;
                            StagingText = EnumStatusGeneralConsultantService.Finished;
                            break;

                        case EnumStatusGeneralConsultantService.Waiting:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                            StagingText = EnumStatusGeneralConsultantService.Finished;
                            break;

                        case EnumStatusGeneralConsultantService.Physician:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Physician;
                            StagingText = EnumStatusGeneralConsultantService.Finished;
                            break;

                        case EnumStatusGeneralConsultantService.Finished:
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                            StagingText = EnumStatusGeneralConsultantService.Finished;
                            break;

                        //case EnumStatusGeneralConsultantService.Physician:
                        //    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Finished;
                        //    if (GeneralConsultanService.IsSickLeave || GeneralConsultanService.IsMaternityLeave)
                        //    {
                        //        // Logic for sick leave can be re-enabled if needed
                        //    }
                        //    StagingText = EnumStatusGeneralConsultantService.Finished;
                        //    break;

                        default:
                            break;
                    }

                    if (GeneralConsultanService.Id == 0)
                    {
                        newGC = await Mediator.Send(new CreateFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = GeneralConsultanService,
                            Status = GeneralConsultanService.Status,
                            UserDto = new UserDto
                            {
                                Id = GeneralConsultanService.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = UserForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = UserForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = UserForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = UserForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = UserForm.IsMedicationHistory,
                                FamilyMedicalHistory = UserForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = UserForm.FamilyMedicalHistoryOther,
                                MedicationHistory = UserForm.MedicationHistory,
                                PastMedicalHistory = UserForm.PastMedicalHistory,
                                CurrentMobile = UserForm.CurrentMobile
                            }
                        });
                    }
                    else
                    {
                        newGC = await Mediator.Send(new UpdateConfirmFormGeneralConsultanServiceNewRequest
                        {
                            GeneralConsultanServiceDto = GeneralConsultanService,
                            Status = GeneralConsultanService.Status,
                            UserDto = new UserDto
                            {
                                Id = GeneralConsultanService.PatientId.GetValueOrDefault(),
                                IsWeatherPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsPharmacologyPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                IsFoodPatientAllergyIds = UserForm.IsWeatherPatientAllergyIds,
                                WeatherPatientAllergyIds = UserForm.WeatherPatientAllergyIds,
                                PharmacologyPatientAllergyIds = UserForm.PharmacologyPatientAllergyIds,
                                FoodPatientAllergyIds = UserForm.FoodPatientAllergyIds,
                                IsFamilyMedicalHistory = UserForm.IsFamilyMedicalHistory,
                                IsMedicationHistory = UserForm.IsMedicationHistory,
                                FamilyMedicalHistory = UserForm.FamilyMedicalHistory,
                                FamilyMedicalHistoryOther = UserForm.FamilyMedicalHistoryOther,
                                MedicationHistory = UserForm.MedicationHistory,
                                PastMedicalHistory = UserForm.PastMedicalHistory,
                                CurrentMobile = UserForm.CurrentMobile
                            }
                        });
                    }

                    // Handle user login state
                    if (UserLogin.Id == GeneralConsultanService.PatientId)
                    {
                        var usr = await Mediator.Send(new GetSingleUserQuery
                        {
                            Predicate = x => x.Id == UserForm.Id,
                            Select = x => new User { Id = x.Id, Name = x.Name, GroupId = x.GroupId }
                        });

                        if (usr != null)
                        {
                            await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);
                            var aa = (CustomAuthenticationStateProvider)CustomAuth;
                            await aa.UpdateAuthState(string.Empty);
                            await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_INFO, Helper.Encrypt(JsonConvert.SerializeObject(usr)), 2);
                        }
                    }

                    ToastService.ClearSuccessToasts();
                    IsContinueCPPT = false;
                    Id = newGC.Id;
                    GeneralConsultanService = await GetGeneralConsultanServiceById();

                    if (PageMode == EnumPageMode.Create.GetDisplayName())
                        NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");

                    if (IsStatus(EnumStatusGeneralConsultantService.Finished))
                    {
                        #region Cut Product Stock

                        var vaccinations = await Mediator.Send(new GetVaccinationPlanQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));
                        var stocks = new List<TransactionStockDto>();

                        foreach (var i in vaccinations)
                        {
                            i.Status = EnumStatusVaccination.Done;

                            var s = (await Mediator.Send(new GetTransactionStockQuery(x => x.ProductId == i.ProductId && x.Batch == i.Batch && x.LocationId == GeneralConsultanService.LocationId))).FirstOrDefault() ?? new();

                            stocks.Add(new TransactionStockDto
                            {
                                SourceTable = nameof(VaccinationPlan),
                                SourcTableId = i.Id,
                                ProductId = i.ProductId,
                                Reference = GeneralConsultanService.Reference,
                                Batch = i.Batch,
                                LocationId = GeneralConsultanService.LocationId,
                                Quantity = -i.Quantity,
                                Validate = true,
                                ExpiredDate = s.ExpiredDate,
                                UomId = s.UomId
                            });
                        }

                        await Mediator.Send(new UpdateListVaccinationPlanRequest(vaccinations));
                        await Mediator.Send(new CreateListTransactionStockRequest(stocks));

                        #endregion Cut Product Stock
                    }
                }

                // Method to map UserForm to UserDto
                UserDto MapUserDto(UserDto userForm)
                {
                    return new UserDto
                    {
                        Id = GeneralConsultanService.PatientId.GetValueOrDefault(),
                        IsWeatherPatientAllergyIds = userForm.IsWeatherPatientAllergyIds,
                        IsPharmacologyPatientAllergyIds = userForm.IsPharmacologyPatientAllergyIds,
                        IsFoodPatientAllergyIds = userForm.IsFoodPatientAllergyIds,
                        WeatherPatientAllergyIds = userForm.WeatherPatientAllergyIds,
                        PharmacologyPatientAllergyIds = userForm.PharmacologyPatientAllergyIds,
                        FoodPatientAllergyIds = userForm.FoodPatientAllergyIds,
                        IsFamilyMedicalHistory = userForm.IsFamilyMedicalHistory,
                        IsMedicationHistory = userForm.IsMedicationHistory,
                        FamilyMedicalHistory = userForm.FamilyMedicalHistory,
                        FamilyMedicalHistoryOther = userForm.FamilyMedicalHistoryOther,
                        MedicationHistory = userForm.MedicationHistory,
                        PastMedicalHistory = userForm.PastMedicalHistory,
                        CurrentMobile = userForm.CurrentMobile
                    };
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool IsLoadingHistoricalRecordPatient { get; set; } = false;

        private void OnClickPopUpHistoricalMedical()
        {
            IsHistoricalRecordPatient = true;
        }

        private bool IsFollowUp = false;
        private bool IsReferTo = false;
        private bool IsAppoimentPending = false;
        private bool IsHistoricalRecordPatient = false;

        private void OnReferToClick()
        {
            IsReferTo = true;
        }

        private void HandleClosePopupReferTo()
        {
            IsReferTo = false; // Tutup popup
        }

        private void OnAppoimentPopUpClick()
        {
            IsFollowUp = true;
        }

        private void HandleClosePopup()
        {
            IsFollowUp = false;
        }

        private void OnClickPopUpAppoimentPending()
        {
            IsAppoimentPending = true;
        }

        private void OnClickReferralPrescriptionConcoction()
        {
            NavigationManager.NavigateTo($"/pharmacy/presciptions/{GeneralConsultanService.Id}");
        }

        private void OnPrintDocumentMedical()
        {
            var IdEncrypt = SecureHelper.EncryptIdToBase64(GeneralConsultanService.Id);
            NavigationManager.NavigateTo($"transaction/print-document-medical/{IdEncrypt}");
        }

        private void OnClickPopUpPopUpProcedureRoom()
        {
            NavigationManager.NavigateTo($"clinic-service/procedure-rooms/{EnumPageMode.Update.GetDisplayName()}?GcId={GeneralConsultanService.Id}");
        }

        private bool isPrint { get; set; } = false;
        private DxRichEdit richEdit;
        private DevExpress.Blazor.RichEdit.Document documentAPI;
        public byte[]? DocumentContent;

        private async Task SendToPrint(long id)
        {
            try
            {
                PanelVisible = true;

                DateTime? startSickLeave = null;
                DateTime? endSickLeave = null;

                var culture = new System.Globalization.CultureInfo("id-ID");

                var data = (await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == id,
                    Includes =
                    [
                        x => x.Pratitioner,
                        x => x.Patient
                    ],
                    Select = x => new GeneralConsultanService
                    {
                        Id = x.Id,
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            DateOfBirth = x.Patient.DateOfBirth
                        },
                        RegistrationDate = x.RegistrationDate,
                        PratitionerId = x.PratitionerId,
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner.Name,
                            SipNo = x.Pratitioner.SipNo
                        },
                        StartMaternityLeave = x.StartMaternityLeave,
                        EndMaternityLeave = x.EndMaternityLeave,
                        StartDateSickLeave = x.StartDateSickLeave,
                        EndDateSickLeave = x.EndDateSickLeave,
                    }
                })) ?? new();
                var patienss = (await Mediator.Send(new GetSingleUserQuery
                {
                    Predicate = x => x.Id == data.PatientId,
                    Select = x => new User
                    {
                        Id = x.Id,
                        IsEmployee = x.IsEmployee,
                        Name = x.Name,
                        Gender = x.Gender,
                        DateOfBirth = x.DateOfBirth
                    },
                })) ?? new();

                var age = 0;

                data = data == null ? new GeneralConsultanServiceDto() : data;

                if (data is not null && data.Patient is not null && data.Patient.DateOfBirth == null)
                {
                    age = 0;
                }
                else
                {
                    age = DateTime.Now.Year - patienss.DateOfBirth.GetValueOrDefault().Year;
                }
                if (data is not null && data.IsSickLeave)
                {
                    startSickLeave = data.StartDateSickLeave;
                    endSickLeave = data.EndDateSickLeave;
                }
                else if (data.IsMaternityLeave)
                {
                    startSickLeave = data.StartMaternityLeave;
                    endSickLeave = data.EndMaternityLeave;
                }

                int TotalDays = endSickLeave.GetValueOrDefault().Day - startSickLeave.GetValueOrDefault().Day;

                string WordDays = ConvertNumberHelper.ConvertNumberToWord(TotalDays);

                string todays = data.RegistrationDate.ToString("dddd", culture);

                //Gender
                string Gender = "";
                string OppositeSex = "";
                if (patienss.Gender != null)
                {
                    Gender = patienss.Gender == "Male" ? "MALE (L)" : "FEMALE (P)";
                    OppositeSex = patienss.Gender == "Male" ? "<strike>F(P)</strike>" : "<strike>M(L)</strike>";
                }

                isPrint = true;
                string GetDefaultValue(string value, string defaultValue = "-")
                {
                    return value ?? defaultValue;
                }

                var mergeFields = new Dictionary<string, string>
                {
                    {"<<NamePatient>>", GetDefaultValue(patienss.Name)},
                    {"<<startDate>>", GetDefaultValue(startSickLeave.GetValueOrDefault().ToString("dd MMMM yyyy"))},
                    {"<<endDate>>", GetDefaultValue(endSickLeave.GetValueOrDefault().ToString("dd MMMM yyyy"))},
                    {"<<NameDoctor>>", GetDefaultValue(data?.Pratitioner?.Name)},
                    {"<<SIPDoctor>>", GetDefaultValue(data?.Pratitioner?.SipNo)},
                    {"<<AddressPatient>>", GetDefaultValue(patienss.DomicileAddress1) + GetDefaultValue(patienss.DomicileAddress2)},
                    {"<<AgePatient>>", GetDefaultValue(age.ToString())},
                    {"<<WordDays>>", GetDefaultValue(WordDays)},
                    {"<<Days>>", GetDefaultValue(todays)},
                    {"<<TotalDays>>", GetDefaultValue(TotalDays.ToString())},
                    {"<<Dates>>", GetDefaultValue(data.RegistrationDate.ToString("dd MMMM yyyy"))},
                    {"<<Times>>", GetDefaultValue(data.RegistrationDate.ToString("H:MM"))},
                    {"<<Date>>", DateTime.Now.ToString("dd MMMM yyyy")},  // Still no null check needed
                    {"<<Genders>>", GetDefaultValue(Gender)},
                    {"<<OppositeSex>>", GetDefaultValue(OppositeSex, "")} // Use empty string if null
                };

                if (patienss.IsEmployee == false)
                {
                    DocumentContent = await DocumentProvider.GetDocumentAsync("SuratIzin.docx", mergeFields);
                }
                else if (patienss.IsEmployee == true)
                {
                    DocumentContent = await DocumentProvider.GetDocumentAsync("Employee.docx", mergeFields);
                }
                // Konversi byte array menjadi base64 string
                //var base64String = Convert.ToBase64String(DocumentContent);

                //// Panggil JavaScript untuk membuka dan mencetak dokumen
                //await JsRuntime.InvokeVoidAsync("printDocument", base64String);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task OnPrint()
        {
            try
            {
                if (GeneralConsultanService.Id == 0)
                    return;

                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                var image = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\mcdermott_logo.png");
                var file = System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\Slip_Registration.pdf");
                QuestPDF.Fluent.Document
                    .Create(x =>
                    {
                        x.Page(page =>
                        {
                            page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);

                            page.Header().Row(row =>
                            {
                                row.ConstantItem(150).Image(File.ReadAllBytes(image));
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Slip Registration").FontSize(36).SemiBold();
                                    c.Item().Text($"MedRec: {GeneralConsultanService.Patient?.NoRm}");
                                    c.Item().Text($"Patient: {GeneralConsultanService.Patient?.Name}");
                                    c.Item().Text($"Identity Number: {GeneralConsultanService.Patient?.NoId}");
                                    c.Item().Text($"Reg Type: {GeneralConsultanService.TypeRegistration}");
                                    c.Item().Text($"Service: {GeneralConsultanService.Service?.Name}");
                                    c.Item().Text($"Physicion: {GeneralConsultanService.Pratitioner?.Name}");
                                    c.Item().Text($"Payment: {GeneralConsultanService.Payment}");
                                    c.Item().Text($"Registration Date: {GeneralConsultanService.RegistrationDate}");
                                    c.Item().Text($"Schedule Time: {GeneralConsultanService.ScheduleTime}");
                                });
                            });
                            //page.Header().Text("Slip Registration").SemiBold().FontSize(30);
                        });
                    })
                .GeneratePdf(file);

                await Helper.DownloadFile("Slip_Registration.pdf", HttpContextAccessor, HttpClient, JsRuntime);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private bool IsPopUpPainScale = false;

        private void OnClickPainScalePopUp()
        {
            IsPopUpPainScale = true;
        }

        private void OnClosePopup()
        {
            IsPopUpPainScale = false;
        }

        #endregion OnClick

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

        private CancellationTokenSource? _ctsLocation;

        private async Task OnInputLocation(ChangeEventArgs e)
        {
            try
            {
                _ctsLocation?.Cancel();
                _ctsLocation?.Dispose();
                _ctsLocation = new CancellationTokenSource();

                await Task.Delay(Helper.CBX_DELAY, _ctsLocation.Token);

                await LoadLocation(e.Value?.ToString() ?? "");
            }
            finally
            {
                _ctsLocation?.Dispose();
                _ctsLocation = null;
            }
        }

        private async Task LoadLocation(string? e = "", Expression<Func<Locations, bool>>? predicate = null)
        {
            try
            {
                Locations = await Mediator.QueryGetComboBox<Locations, LocationDto>(e, predicate, select: x => new Domain.Entities.Locations
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentLocation = new Locations
                    {
                        Name = x.ParentLocation.Name
                    }
                });
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboBox Location
    }
}