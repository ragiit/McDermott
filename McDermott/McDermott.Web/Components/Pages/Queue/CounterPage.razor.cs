﻿using FluentValidation.Results;
using McDermott.Application.Dtos.Queue;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class CounterPage
    {
        #region Relation Data

        private List<CounterDto> counters = new();
        private List<CounterDto> countersActive = new();
        private List<CounterDto> countersInActive = new();
        private List<ServiceDto> Services = [];
        private List<UserDto> Physicians = [];
        public List<UserDto> Phys = new();
        private List<ServiceDto> ServiceP = [];
        private CounterDto counterForm = new();
        private List<KioskQueueDto> KiosksQueue = new();
        private List<KioskDto> Kiosks = new();

        #endregion Relation Data

        #region setings Grid

        private IReadOnlyList<object> SelectedDataItems { get; set; } = new ObservableRangeCollection<object>();
        private BaseAuthorizationLayout AuthorizationLayout = new();
        public IGrid Grid { get; set; }
        private List<string> NameCard = new();
        private string textPopUp = "";
        private bool PanelVisible { get; set; } = true;
        private bool IsLoading { get; set; } = true;
        private bool PopUpVisible { get; set; } = false;
        private bool PopUpEditCOunter { get; set; } = false;
        private bool ArchiveCard { get; set; } = false;
        private bool EditItemsEnabled { get; set; }
        private bool showFormProcess { get; set; } = false;
        private bool GirdDetail { get; set; } = false;
        private long CountCard { get; set; } = 0;
        private long? _ServiceId { get; set; }
        private long? _physicionId { get; set; }
        private long idServiceK { get; set; }
        private long ActiveTabIndex { get; set; } = 1;
        private string NameCounter { get; set; } = string.Empty;
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private long? PhysicianId { get; set; }
        private string? userBy;
        private User? User = new();

        private int FocusedRowVisibleIndex { get; set; }

        #endregion setings Grid

        #region Async Data And Auth

        //private long? SelectServiced
        //{
        //    get => _ServiceId;
        //    set
        //    {
        //        _ServiceId = value;

        //        if (value is null)
        //            return;

        //        counterForm.ServiceKId = value;
        //        List<ServiceDto> service = new();
        //        service = Services.Where(x => x.Id == value).ToList();

        //        //get Service Flag P
        //        ServiceP = Services.Where(x => x.ServicedId == value && x.IsPatient == true).ToList();

        //    }
        //}

        private long? SelectPhysicion
        {
            get => _physicionId;
            set
            {
                _physicionId = value;

                if (value is null)
                    return;

                counterForm.ServiceId = value;
                List<ServiceDto> service = new();
                service = Services.Where(x => x.Id == value).ToList();

                //get Physicion
                foreach (var i in service)
                {
                    if (i != null)
                    {
                        Phys = Physicians.Where(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(value.GetValueOrDefault())).ToList();
                    }
                }
            }
        }

        public MarkupString GetIssuePriorityIconHtml(string status)
        {
            string priorytyClass = "info";
            string title = "Call";
            if (status == "call")
            {
                priorytyClass = "info";
                title = " Call ";

                string html = string.Format("<div class='row justify-content-center'><div class='col-sm-5 pl-0'><span class='badge bg-{0} py-1 px-4' title='{1}'>{1}</span></div></div>", priorytyClass, title);
                return new MarkupString(html);
            }
            else if (status == "hadir")
            {
                priorytyClass = "success";
                title = " Hadir ";

                string html = string.Format("<div class='row justify-content-center'><div class='col-sm-5 pl-0'><span class='badge bg-{0} py-1 px-4' title='{1}'>{1}</span></div></div>", priorytyClass, title);
                return new MarkupString(html);
            }
            return new MarkupString("");
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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetUserInfo();
            }
            catch { }
            //var by =

            await LoadData();
            await LoadDataPhysicion();
        }

        private async Task LoadData()
        {
            StateHasChanged();
            PopUpVisible = false;
            PanelVisible = true;
            countersActive.Clear();
            counterForm = new();
            IsLoading = true;
            counters = (await Mediator.Send(new GetCounterQuery
            {
                IsGetAll = true
            })).Item1;
            IsLoading = false;

            countersActive = [.. counters.Where(x => x.IsActive == true)];
            countersInActive = [.. counters.Where(x => x.IsActive == false)];

            Services = (await Mediator.Send(new GetServiceQuery
            {
                Predicate = x => counters.Select(z => z.ServiceId).Contains(x.Id),
                IsGetAll = true
            })).Item1;
            PanelVisible = false;
        }

        #endregion Async Data And Auth

        #region Config Grid

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        private void UpdateEditItemsEnabled(bool enabled)
        {
            EditItemsEnabled = enabled;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
            UpdateEditItemsEnabled(true);
        }

        #endregion Config Grid

        #region Button Function

        private void NewItem_Click()
        {
            counterForm = new();
            PopUpVisible = true;
            textPopUp = "Add Counter";
        }

        private async Task EditItem_Click(long Id)
        {
            try
            {
                var General = await Mediator.Send(new GetSingleCounterQuery
                {
                    Predicate = x => x.Id == Id
                });
                counterForm = General;
                PopUpVisible = true;
                textPopUp = $"Edit Counter {General.Name}";

                //var results = await Mediator.Send(new GetUserQueryNew
                //{
                //    Predicate = x => x.Id == General.PhysicianId,
                //    Select = x => new User
                //    {
                //        Id = x.Id,
                //        Name = x.Name,
                //        Email = x.Email,
                //        MobilePhone = x.MobilePhone,
                //        Gender = x.Gender,
                //        DateOfBirth = x.DateOfBirth,
                //        IsPhysicion = x.IsPhysicion,
                //        IsNurse = x.IsNurse,
                //    }
                //});
                //Physicians = results.Item1;

                //var resultK = await Mediator.Send(new GetServiceQuery
                //{
                //    Predicate = x => x.Id == General.ServiceKId,
                //});
                //ServiceK = resultK.Item1;

                //var resultP = await Mediator.Send(new GetServiceQuery
                //{
                //    Predicate = x => x.Id == General.ServiceId,
                //});
                //ServiceK = resultP.Item1;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItem_Click(long Id)
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        #region Card

        private async Task ShowOnProcess(long id)
        {
            try
            {
                StateHasChanged();
                PanelVisible = true;
                counterForm = new();

                var General = await Mediator.Send(new GetSingleCounterQuery
                {
                    Predicate = x => x.Id == id
                });
                counterForm.Id = General.Id;
                counterForm.Name = General.Name;
                textPopUp = "Configurtion Counter " + counterForm.Name;
                PopUpEditCOunter = true;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task InActive_Click(long Id)
        {
            try
            {
                if (Id != 0)
                {
                    var a = counters.Where(x => x.Id == Id).FirstOrDefault();

                    counterForm.Id = Id;
                    counterForm.Name = a.Name;
                    counterForm.Status = "open";
                    await Mediator.Send(new UpdateCounterRequest(counterForm));

                    //NavigationManager.NavigateTo("transaction/counter", true);
                    await LoadData();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CloseDetail()
        {
            GirdDetail = false;
        }

        private async Task DetailList(long Id)
        {
            try
            {
                NavigationManager.NavigateTo($"queue/queue-counters/view/{Id}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task Archive_Click()
        {
            try
            {
                ArchiveCard = true;
                await LoadData();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void onBack()
        {
            try
            {
                ArchiveCard = false;
                Kiosks.Clear();
            }
            catch { }
        }

        #endregion Card

        #region Button Export, Import And Colmn Chooser

        private async Task ExportXlsxItem_Click()
        {
            await Grid.ExportToXlsxAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportXlsItem_Click()
        {
            await Grid.ExportToXlsAsync("ExportResult", new GridXlExportOptions()
            {
                ExportSelectedRowsOnly = true,
            });
        }

        private async Task ExportCsvItem_Click()
        {
            await Grid.ExportToCsvAsync("ExportResult", new GridCsvExportOptions
            {
                ExportSelectedRowsOnly = true,
            });
        }

        #endregion Button Export, Import And Colmn Chooser

        private void OnCancel()
        {
            counterForm = new();
            PopUpVisible = false;
            showFormProcess = false;
        }

        #endregion Button Function

        #region Methode Delete

        private async Task OnDelete(long Id)
        {
            try
            {
                if (Id != null)
                {
                    await Mediator.Send(new DeleteCounterRequest(Id));
                }

                await LoadData();
            }
            catch { }
        }

        #endregion Methode Delete

        #region Methode Save And Update

        private async Task OnSave()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(counterForm.Name))
                    return;

                counterForm.IsActive = true;
                counterForm.Status = "open";
                var edit = counterForm;
                if (counterForm.Id == 0)
                    await Mediator.Send(new CreateCounterRequest(counterForm));
                else
                    await Mediator.Send(new UpdateCounterRequest(counterForm));

                counterForm = new();
                await LoadData();
            }
            catch { }
        }

        private bool IsLoadingAddNewCounter = false;

        private async Task OnSaveAddNewCounter(int state)
        {
            try
            {
                if (state == 1)
                {
                    IsLoadingAddNewCounter = true;

                    counterForm.IsActive = true;
                    counterForm.Status = "on process";
                    if (counterForm.Id != 0)
                    {
                        await Mediator.Send(new UpdateCounterRequest(counterForm));
                    }
                    NavigationManager.NavigateTo($"queue/queue-counters/view/{counterForm.Id}");
                    showFormProcess = false;
                    await LoadData();
                }
                else if (state == 2)
                {
                    IsLoadingAddNewCounter = true;

                    ValidationResult results = new AddCounterPopUp().Validate(counterForm);

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

                    if (!success)
                    {
                        IsLoadingAddNewCounter = false;
                        return;
                    }

                    var checkName = await Mediator.Send(new ValidateCounterQuery(x => x.Id != counterForm.Id && x.Name.ToLower().Equals(counterForm.Name.ToLower())));

                    if (checkName)
                    {
                        ToastService.ShowInfo("Counter name already exist");
                        IsLoadingAddNewCounter = false;
                        return;
                    }

                    counterForm.IsActive = true;
                    counterForm.Status = "open";

                    if (counterForm.Id == 0)
                        await Mediator.Send(new CreateCounterRequest(counterForm));
                    else
                        await Mediator.Send(new UpdateCounterRequest(counterForm));

                    ToastService.ShowSuccess($"'{counterForm.Name}' successfully added");

                    counterForm = new();
                    await LoadData();
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally
            {
                IsLoadingAddNewCounter = false;
            }
        }

        private async Task StopProcess(long id)
        {
            try
            {
                var a = counters.Where(x => x.Id == id).FirstOrDefault();
                counterForm.Id = a.Id;
                counterForm.Name = a.Name;
                counterForm.ServiceId = a.ServiceId;
                counterForm.ServiceKId = a.ServiceKId;
                counterForm.IsActive = true;
                counterForm.Status = "stop";

                if (a != null)
                {
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task ResumeProcess(long id)
        {
            try
            {
                var a = counters.Where(x => x.Id == id).FirstOrDefault();
                counterForm.Id = a.Id;
                counterForm.Name = a.Name;
                counterForm.ServiceId = a.ServiceId;
                counterForm.ServiceKId = a.ServiceKId;
                counterForm.IsActive = true;
                counterForm.Status = "on process";

                if (a != null)
                {
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task OnProcess()
        {
            try
            {
                counterForm.IsActive = true;
                counterForm.Status = "on process";
                var edit = counterForm;
                if (counterForm.Id != 0)
                {
                    await Mediator.Send(new UpdateCounterRequest(counterForm));
                }
                NavigationManager.NavigateTo($"queue/queue-counters/view/{counterForm.Id}");
                showFormProcess = false;
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        #endregion Methode Save And Update

        private bool IsLoadingEditCounter = false;

        #region ComboboxServiceK

        private DxComboBox<ServiceDto, long?> refServiceKComboBox { get; set; }
        private int ServiceKComboBoxIndex { get; set; } = 0;
        private int totalCountServiceK = 0;

        private async Task OnSearchServiceK()
        {
            await LoadDataServiceK();
        }

        private async Task OnSearchServiceKIndexIncrement()
        {
            if (ServiceKComboBoxIndex < (totalCountServiceK - 1))
            {
                ServiceKComboBoxIndex++;
                await LoadDataServiceK(ServiceKComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServiceKIndexDecrement()
        {
            if (ServiceKComboBoxIndex > 0)
            {
                ServiceKComboBoxIndex--;
                await LoadDataServiceK(ServiceKComboBoxIndex, 10);
            }
        }

        private async Task OnInputServiceKChanged(string e)
        {
            ServiceKComboBoxIndex = 0;
            await LoadDataServiceK();
        }

        private List<ServiceDto> ServiceK { get; set; } = [];

        private async Task LoadDataServiceK(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoadingEditCounter = true;
                var result = await Mediator.Send(new GetServiceQuery
                {
                    Predicate = x => x.IsKiosk == true,
                    SearchTerm = refServiceKComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                ServiceK = result.Item1;
                totalCountServiceK = result.PageCount;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoadingEditCounter = false; }
        }

        #endregion ComboboxServiceK

        #region ComboboxServiceP

        private DxComboBox<ServiceDto, long?> refServicePComboBox { get; set; }
        private int ServicePComboBoxIndex { get; set; } = 0;
        private int totalCountServiceP = 0;

        private async Task OnSearchServiceP()
        {
            await LoadDataServiceP();
        }

        private async Task OnSearchServicePIndexIncrement()
        {
            if (ServicePComboBoxIndex < (totalCountServiceP - 1))
            {
                ServicePComboBoxIndex++;
                await LoadDataServiceP(ServicePComboBoxIndex, 10);
            }
        }

        private async Task OnSearchServicePIndexDecrement()
        {
            if (ServicePComboBoxIndex > 0)
            {
                ServicePComboBoxIndex--;
                await LoadDataServiceP(ServicePComboBoxIndex, 10);
            }
        }

        private async Task OnInputServicePChanged(string e)
        {
            ServicePComboBoxIndex = 0;
            await LoadDataServiceP();
        }

        private async Task LoadDataServiceP(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                IsLoadingEditCounter = true;
                var result = await Mediator.Send(new GetServiceQuery
                {
                    Predicate = x => x.IsPatient == true,
                    SearchTerm = refServicePComboBox?.Text ?? "",
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                });
                ServiceP = result.Item1;
                totalCountServiceP = result.PageCount;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { IsLoadingEditCounter = false; }
        }

        #endregion ComboboxServiceP

        #region ComboboxPhysicion

        private DxComboBox<UserDto, long?> refPhysicionComboBox { get; set; }
        private int PhysicionComboBoxIndex { get; set; } = 0;
        private int totalCountPhysicion = 0;

        private async Task OnSearchPhysicion()
        {
            await LoadDataPhysicion();
        }

        private async Task OnSearchPhysicionIndexIncrement()
        {
            if (PhysicionComboBoxIndex < (totalCountPhysicion - 1))
            {
                PhysicionComboBoxIndex++;
                await LoadDataPhysicion(PhysicionComboBoxIndex, 10);
            }
        }

        private async Task OnSearchPhysicionIndexDecrement()
        {
            if (PhysicionComboBoxIndex > 0)
            {
                PhysicionComboBoxIndex--;
                await LoadDataPhysicion(PhysicionComboBoxIndex, 10);
            }
        }

        private async Task OnInputPhysicionChanged(string e)
        {
            PhysicionComboBoxIndex = 0;
            await LoadDataPhysicion();
        }

        private async Task LoadDataPhysicion(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                PanelVisible = true;
                var result = await Mediator.Send(new GetUserQuery2(
                               x => x.IsDoctor == true && x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(counterForm.ServiceId.GetValueOrDefault()),
                               searchTerm: refPhysicionComboBox?.Text ?? "",
                               pageSize: pageSize,
                               pageIndex:
                               pageIndex,
                               includes: [],
                               select: x => new User
                               {
                                   Id = x.Id,
                                   Name = x.Name,
                                   Email = x.Email,
                                   MobilePhone = x.MobilePhone,
                                   Gender = x.Gender,
                                   DateOfBirth = x.DateOfBirth,
                                   IsPhysicion = x.IsPhysicion,
                                   IsNurse = x.IsNurse,
                               }
                           ));
                Physicians = result.Item1;
                totalCountPhysicion = result.pageCount;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion ComboboxPhysicion
    }
}