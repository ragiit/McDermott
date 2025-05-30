﻿using SignaturePad;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class ProcedureRoomPage
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

        private SignaturePadOptions _options = new SignaturePadOptions
        {
            LineCap = LineCap.Round,
            LineJoin = LineJoin.Round,
        };

        #region Properties

        private List<LabResultDetailDto> LabResultDetails = [];
        private LabResultDetailDto LabResultDetail = new();
        private LabTestDetailDto LabTest = new();
        private LabUomDto LabUom = new();
        private GroupDto NameGroup { get; set; } = new();
        private UserDto NameUser { get; set; } = new();

        private List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupports = [];
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport = new();
        private GeneralConsultanServiceDto GeneralConsultanService = new();
        private List<GroupDto> groups = [];
        private List<UserDto> user_group = [];
        private List<UserDto> Doctors { get; set; } = [];
        private List<LabTestDto> LabTests = [];
        private IEnumerable<LabTestDetailDto> SelectedLabTests = [];
        private List<IBrowserFile> BrowserFiles = [];
        private List<long> DeletedLabTestIds = [];

        private List<string> ResultValueTypes =
        [
            "Low",
            "Normal",
            "High",
            "Positive",
            "Negative",
        ];

        private IEnumerable<string> Recommends =
        [
            "FIT",
            "UNFIT",
            "REASSESS"
        ];

        private bool IsAddOrUpdateOrDeleteLabResult = false;
        private bool Loading = false;
        private string StagingText = "In-Progress";
        private bool ShowForm = false;
        private bool FormValidationState = false;
        private bool PopUpProcedureRoom = false;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        private int FocusedRowLabTestVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IGrid GridLabTest { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private IReadOnlyList<object> SelectedLabTestDataItems { get; set; } = [];

        #endregion Properties

        #region LoadData

        [SupplyParameterFromQuery]
        public long genserv { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

                await LoadDataAsync();
                StateHasChanged();

                PanelVisible = true;

                var uri = new Uri(NavigationManager.Uri);
                var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

                GeneralConsultanService = new();
                if (queryDictionary.TryGetValue("genserv", out var genSetValue))
                {
                    ShowForm = false;
                    ToastService.ShowInfo(genserv.ToString());
                    GeneralConsultanService = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == genSetValue.ToString().ToLong()))).Item1.FirstOrDefault() ?? new();
                    ShowForm = true;
                }
                PanelVisible = false;

                try
                {
                    if (ShowForm)
                        Grid?.SelectRow(0, true);
                }
                catch { }
                StateHasChanged();
            }
        }

        private List<UserDto> Employees = [];

        private async Task LoadDataAsync()
        {
            Employees = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true && x.IsPatient == true));
            var LabUoms = await Mediator.Send(new GetLabUomQuery());
            this.LabUoms = LabUoms.Item1;
            var LabTests = (await Mediator.Send(new GetLabTestQuery())).Item1;
            this.LabTests = LabTests;
            Doctors = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
            var result2 = await Mediator.Send(new GetGroupQuery { IsGetAll = true });
            groups = result2.Item1;
            NameGroup = groups.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
            user_group = await Mediator.Send(new GetUserQuery());
            NameUser = user_group.FirstOrDefault(x => x.GroupId == UserAccessCRUID.GroupId && x.Id == UserLogin.Id) ?? new();
        }

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

        private void OnSelectEmployee(UserDto e)
        {
            if (e is null)
            {
                return;
            }

            GeneralConsultanMedicalSupport.Employee = Employees.FirstOrDefault(x => x.Id == e.Id);
        }

        protected override async Task OnInitializedAsync()
        {
        }

        public bool IsConfinedSpace { get; set; } = false;

        private void OnConfinedSpaceChanged(bool value, bool isYesOption)
        {
            if (isYesOption)
            {
                IsConfinedSpace = value;
                GeneralConsultanMedicalSupport.IsConfinedSpace = true;
            }
            else
            {
                GeneralConsultanMedicalSupport.IsConfinedSpace = value;
                IsConfinedSpace = !value;
            }
        }

        public class YesNoOptions
        {
            public string Text { get; set; }
            public bool Value { get; set; }
        }

        private IEnumerable<YesNoOptions> Options = new[]
      {
        new YesNoOptions { Text = "Yes", Value = true },
        new YesNoOptions { Text = "No", Value = false }
    };

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            PanelVisible = true;
            PertamaKali = true;
            ShowForm = false;
            GeneralConsultanService = new();
            GeneralConsultanMedicalSupport = new();
            SelectedDataItems = [];
            //var sets = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery());
            //foreach (var item in sets)
            //{
            //    if (item.IsConfinedSpace)
            //    {
            //        item.GeneralConsultanService = new();
            //        item.GeneralConsultanService.Patient = item.Employee;
            //    }
            //}
            //GeneralConsultanMedicalSupports = sets;
            PanelVisible = false;
        }

        #endregion LoadData

        #region Methods

        private async Task SelectedItemLabTest(LabTestDto e)
        {
            if (e is null)
            {
                GeneralConsultanMedicalSupport.LabTestId = null;
                LabResultDetails.Clear();
                return;
            }

            //LabResultDetails.Clear();

            //if (PertamaKali)
            //{
            //    PertamaKali = false;
            //    return;
            //}

            var details = await Mediator.Send(new GetLabTestDetailQuery
            {
                Predicate = x => x.LabTestId == e.Id
            });
            foreach (var item in details.Item1)
            {
                LabResultDetails.Add(new LabResultDetailDto
                {
                    IsFromDB = true,
                    Id = Helper.RandomNumber,
                    NormalRange = GeneralConsultanService.Patient.Gender.Equals("Male") ? item.NormalRangeMale : item.NormalRangeFemale,
                    Parameter = item.Name,
                    Remark = item.Remark,
                    LabUomId = item.LabUomId,
                    LabUom = item.LabUom,
                    ResultValueType = item.ResultValueType
                });
            }

            //if (GeneralConsultanMedicalSupport.Id == 0)
            //{
            //    var temp = new List<LabResultDetailDto>();
            //    foreach (var item in details)
            //    {
            //        temp.Add(new LabResultDetailDto
            //        {
            //            IsFromDB = true,
            //            Id = Helper.RandomNumber,
            //            NormalRange = GeneralConsultanService.Patient.Gender.Equals("Male") ? item.NormalRangeMale : item.NormalRangeFemale,
            //            Parameter = item.Name,
            //            Remark = item.Remark,
            //            LabUomId = item.LabUomId,
            //            LabUom = item.LabUom
            //        });
            //    }

            //    LabResultDetails.AddRange(temp);
            //}
            //else
            //{
            //    LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));
            //}

            //LabResultDetail.LabTestId = e.Id;

            //var a = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == e.Id));
            //LabTestDetails = [];
            //LabResultDetailsLabTestDetails.AddRange(a);
            GridLabTest.Reload();

            GeneralConsultanMedicalSupport.LabTestId = e.Id;
        }

        private async Task OnClickConfirm()
        {
            switch (GeneralConsultanMedicalSupport.Status)
            {
                case EnumStatusGeneralConsultantServiceProcedureRoom.Draft:
                    StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish.GetDisplayName();
                    GeneralConsultanMedicalSupport.Status = EnumStatusGeneralConsultantServiceProcedureRoom.InProgress;

                    if (GeneralConsultanMedicalSupport.Id == 0)
                    {
                        StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish.GetDisplayName();
                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.ProcedureRoom;
                        await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        GeneralConsultanMedicalSupport.GeneralConsultanServiceId = GeneralConsultanService.Id;
                        GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    }
                    else
                        GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

                    if ((GeneralConsultanMedicalSupport.LabTestId is not null && GeneralConsultanMedicalSupport.LabTestId != 0))
                    {
                        await Mediator.Send(new DeleteLabResultDetailRequest(ids: DeletedLabTestIds));

                        LabResultDetails.ForEach(x => x.Id = 0);

                        await Mediator.Send(new CreateListLabResultDetailRequest(LabResultDetails));

                        LabResultDetails.Clear();

                        LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));

                        DeletedLabTestIds = LabResultDetails.Select(x => x.Id).ToList();

                        IsAddOrUpdateOrDeleteLabResult = false;
                    }
                    break;

                case EnumStatusGeneralConsultantServiceProcedureRoom.InProgress:
                    GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Waiting;
                    await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                    GeneralConsultanMedicalSupport.Status = EnumStatusGeneralConsultantServiceProcedureRoom.Finish;
                    StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish.GetDisplayName();
                    await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    break;

                case EnumStatusGeneralConsultantServiceProcedureRoom.Finish:
                    GeneralConsultanMedicalSupport.Status = EnumStatusGeneralConsultantServiceProcedureRoom.Finish;
                    StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish.GetDisplayName();
                    await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    break;

                default:
                    break;
            }

            //await OnSave();
        }

        private void OnSaveLabTest(GridEditModelSavingEventArgs e)
        {
            IsAddOrUpdateOrDeleteLabResult = true;
            var editModel = LabResultDetail;

            //editModel.LabTestDetail = LabTests.FirstOrDefault(l => l.Id == selectedLabTestId);

            if (editModel.Id == 0)
            {
                editModel.Id = Helper.RandomNumber;
                editModel.GeneralConsultanMedicalSupportId = GeneralConsultanMedicalSupport.Id;
                LabResultDetails.Add(editModel);
            }
            else
                LabResultDetails[FocusedRowLabTestVisibleIndex] = editModel;

            IsAddOrUpdateOrDeleteLabResult = true;
        }

        private void OnResultTextChanged(ChangeEventArgs v)
        {
            if (v.Value is null)
                return;

            var value = v.Value.ToString();

            if (long.TryParse(value, out _))
            {
                if (!Regex.IsMatch(LabResultDetail.NormalRange, @"^\d+-\d+$"))
                    LabResultDetail.ResultType = "Negative";
                else
                {
                    var splits = LabResultDetail.NormalRange.Split("-");
                    if (value.ToLong() < splits[0].ToLong())
                    {
                        LabResultDetail.ResultType = "Low";
                    }
                    else
                    {
                        LabResultDetail.ResultType = "Normal";

                        if (value.ToLong() > splits[1].ToLong())
                        {
                            LabResultDetail.ResultType = "High";
                        }
                    }
                }
            }
            else
            {
                LabResultDetail.ResultType = "Negative";
            }
        }

        private List<LabUomDto> LabUoms = [];

        private async Task AddNewLabResult()
        {
            LabResultDetail = new();
            LabUom = new();
            await GridLabTest.StartEditNewRowAsync();
        }

        private async Task EditLabResult(GridCommandColumnCellDisplayTemplateContext context)
        {
            var selected = (LabResultDetailDto)context.DataItem;

            var copy = selected.Adapt<LabResultDetailDto>();

            await GridLabTest.StartEditRowAsync(FocusedRowLabTestVisibleIndex);

            var w = LabResultDetails.FirstOrDefault(x => x.Id == copy.Id);

            this.LabResultDetail = copy;
        }

        private async Task EditLab2Result(int index)
        {
            await GridLabTest.StartEditRowAsync(index);

            //var labTest = LabTests.FirstOrDefault(x => x.Id == LabResultDetail.LabTestDetailId);

            LabTest = new();
            LabUom = new();

            //if (labTest is not null)
            //{
            //    selectedLabTestId = labTest.Id;

            //    if (GeneralConsultanService.Patient is not null && GeneralConsultanService.Patient.Gender is not null)
            //    {
            //        if (GeneralConsultanService.Patient.Gender.Equals("Male"))
            //            labTest.NormalRangeByGender = labTest.NormalRangeMale;
            //        else
            //            labTest.NormalRangeByGender = labTest.NormalRangeFemale;
            //    }

            //    labTest.LabUom ??= new LabUomDto();

            //    LabTest = labTest;
            //    LabUom = labTest.LabUom;
            //}
        }

        private async Task OnDeleteLabTest()
        {
            IsAddOrUpdateOrDeleteLabResult = true;
            //var aaa = SelectedLabTestDataItems.Adapt<List<LabResultDetailDto>>();

            LabResultDetails.Remove(LabResultDetails.FirstOrDefault(x => x.Id == SelectedLabTestDataItems[0].Adapt<LabResultDetailDto>().Id));

            SelectedLabTestDataItems = [];
        }

        private long selectedLabTestId { get; set; }

        private void SelectedItemParameter(LabTestDetailDto e)
        {
            if (e is null)
                return;

            selectedLabTestId = e.Id;

            var labTest = e;

            if (GeneralConsultanService.Patient is not null && GeneralConsultanService.Patient.Gender is not null)
            {
                if (GeneralConsultanService.Patient.Gender.Equals("Male"))
                    labTest.NormalRangeByGender = labTest.NormalRangeMale;
                else
                    labTest.NormalRangeByGender = labTest.NormalRangeFemale;
            }

            labTest.LabUom ??= new LabUomDto();

            LabTest = labTest;
            LabUom = labTest.LabUom;
        }

        private GeneralConsultanMedicalSupportLogDto generalLog = new GeneralConsultanMedicalSupportLogDto();

        private bool IsStatus(EnumStatusGeneralConsultantServiceProcedureRoom status) => GeneralConsultanMedicalSupport.Status == status;

        private async Task OnSave()
        {
            try
            {
                Loading = true;

                if (GeneralConsultanMedicalSupport.IsConfinedSpace)
                {
                    if (GeneralConsultanMedicalSupport.Id == 0)
                        GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                    else
                        GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

                    Loading = false;

                    ToastService.ShowSuccess("Save Successfully");
                    return;
                }

                BrowserFiles.Distinct();

                foreach (var item in BrowserFiles)
                {
                    await FileUploadService.UploadFileAsync(item, 0, []);
                }

                GeneralConsultanMedicalSupport.LabResulLabExaminationtIds = SelectedLabTests.Select(x => x.Id).ToList();

                switch (GeneralConsultanMedicalSupport.Status)
                {
                    case EnumStatusGeneralConsultantServiceProcedureRoom.Draft:

                        if (GeneralConsultanMedicalSupport.Id == 0)
                        {
                            StagingText = "In-Progress";
                            GeneralConsultanService.Status = EnumStatusGeneralConsultantService.ProcedureRoom;
                            await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                            GeneralConsultanMedicalSupport.GeneralConsultanServiceId = GeneralConsultanService.Id;
                            GeneralConsultanMedicalSupport = await Mediator.Send(new CreateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                        }
                        else
                            GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));
                        break;

                    case EnumStatusGeneralConsultantServiceProcedureRoom.InProgress:
                        GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

                        break;

                    case EnumStatusGeneralConsultantServiceProcedureRoom.Finish:
                        GeneralConsultanService.Status = EnumStatusGeneralConsultantService.Waiting;

                        await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                        //generalLog.GeneralConsultanServiceId = GeneralConsultanMedicalSupport.GeneralConsultanServiceId;
                        //generalLog.UserById = NameGroup.Id;
                        //generalLog.Status = GeneralConsultanService.StagingStatus;

                        //await Mediator.Send(new CreateGeneralConsultationLogRequest(generalLog));
                        break;

                    default:
                        break;
                }

                //generalLog.ProcedureRoomId = GeneralConsultanMedicalSupport.Id;
                //generalLog.UserById = NameGroup.Id;
                //generalLog.Status = GeneralConsultanMedicalSupport.Status;

                //await Mediator.Send(new CreateGeneralConsultationLogRequest(generalLog));

                if ((GeneralConsultanMedicalSupport.LabTestId is not null && GeneralConsultanMedicalSupport.LabTestId != 0))
                {
                    await Mediator.Send(new DeleteLabResultDetailRequest(ids: DeletedLabTestIds));

                    LabResultDetails.ForEach(x =>
                    {
                        x.Id = 0;
                        x.GeneralConsultanMedicalSupportId = GeneralConsultanMedicalSupport.Id;
                    });
                    await Mediator.Send(new CreateListLabResultDetailRequest(LabResultDetails));

                    LabResultDetails.Clear();

                    LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));

                    DeletedLabTestIds = LabResultDetails.Select(x => x.Id).ToList();

                    IsAddOrUpdateOrDeleteLabResult = false;
                }

                ToastService.ShowSuccess("Saved Successfully");

                Loading = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task HandleValidSubmit()
        {
            FormValidationState = true;
            await OnSave();
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfo("Please ensure that all fields marked in red are filled in before submitting the form.");
            FormValidationState = false;
        }

        #endregion Methods

        #region SaveDelete

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    var u = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == ((GeneralConsultanMedicalSupportDto)e.DataItem).GeneralConsultanServiceId))).Item1.FirstOrDefault();

                    if (u is not null)
                    {
                        u.Status = EnumStatusGeneralConsultantService.Physician;
                        await Mediator.Send(new UpdateGeneralConsultanServiceRequest(u));
                    }

                    await Mediator.Send(new DeleteGeneralConsultanMedicalSupportRequest(((GeneralConsultanMedicalSupportDto)e.DataItem).Id));
                }
                else
                {
                    var selectedItems = SelectedDataItems.Adapt<List<GeneralConsultanMedicalSupportDto>>();
                    foreach (var item in selectedItems)
                    {
                        var u = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == item.GeneralConsultanServiceId))).Item1.FirstOrDefault();

                        if (u is not null)
                        {
                            u.Status = EnumStatusGeneralConsultantService.Physician;
                            await Mediator.Send(new UpdateGeneralConsultanServiceRequest(u));
                        }
                    }

                    await Mediator.Send(new DeleteGeneralConsultanMedicalSupportRequest(ids: SelectedDataItems.Adapt<List<GeneralConsultanMedicalSupportDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #endregion SaveDelete

        #region FileAttachmentLab

        private void RemoveSelectedFileLab()
        {
            GeneralConsultanMedicalSupport.LabEximinationAttachment = null;
        }

        private async void SelectFilesLab(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.LabEximinationAttachment = e.File.Name;
        }

        private async Task SelectFileLab()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "labFile");
        }

        private async Task DownloadFile(string fileName)
        {
            if (GeneralConsultanMedicalSupport.Id != 0 && !string.IsNullOrWhiteSpace(fileName))
            {
                await Helper.DownloadFile(fileName, HttpContextAccessor, HttpClient, JsRuntime);
            }
        }

        #endregion FileAttachmentLab

        #region FileAttachmentRadiology

        private void RemoveSelectedFileRadiology()
        {
            GeneralConsultanMedicalSupport.RadiologyEximinationAttachment = null;
        }

        private void SelectFilesRadiology(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.RadiologyEximinationAttachment = e.File.Name;

            //await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileRadiology()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "radiologyFile");
        }

        #endregion FileAttachmentRadiology

        #region FileAttachmentAlcohol

        private void RemoveSelectedFileAlcohol()
        {
            GeneralConsultanMedicalSupport.AlcoholEximinationAttachment = null;
        }

        private void SelectFilesAlcohol(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.AlcoholEximinationAttachment = e.File.Name;

            //await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileAlcohol()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "alcoholFile");
        }

        #endregion FileAttachmentAlcohol

        #region FileAttachmentDrug

        private void RemoveSelectedFileDrug()
        {
            GeneralConsultanMedicalSupport.DrugEximinationAttachment = null;
        }

        private void SelectFilesDrug(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanMedicalSupport.DrugEximinationAttachment = e.File.Name;

            ToastService.ShowInfo(BrowserFiles.Count().ToString());

            //await FileUploadService.UploadFileAsync(e.File, 1 * 1024 * 1024, []);
        }

        private async Task SelectFileDrug()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "drugFile");
        }

        #endregion FileAttachmentDrug

        #region Grid Function

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void GridLabTest_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowLabTestVisibleIndex = args.VisibleIndex;
            if (args.DataItem is not null)
                LabResultDetail = args.DataItem as LabResultDetailDto;
            else
                LabResultDetail = new();
        }

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #region ToolBar Button

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task AddItemLabTest_Click()
        {
            await GridLabTest.StartEditNewRowAsync();
        }

        private async Task UpdateItemLabTest_Click()
        {
            await GridLabTest.StartEditRowAsync(FocusedRowLabTestVisibleIndex);
        }

        private void DeleteItemLabTest_Click()
        {
            GridLabTest.ShowRowDeleteConfirmation(FocusedRowLabTestVisibleIndex);
        }

        private bool PertamaKali { get; set; } = false;

        private void NewConfinedSpace()
        {
            ShowForm = true;
            GeneralConsultanMedicalSupport = new()
            {
                IsConfinedSpace = true
            };
        }

        private async Task EditItem_Click()
        {
            ShowForm = true;

            GeneralConsultanMedicalSupport = SelectedDataItems[0].Adapt<GeneralConsultanMedicalSupportDto>();

            //GeneralConsultanMedicalSupport.SignatureEmployeeImagesMedicalHistory = Encoding.UTF8.GetBytes($"data:image/png;base64, {GeneralConsultanMedicalSupport.SignatureEmployeeImagesMedicalHistoryBase64}");
            //GeneralConsultanMedicalSupport.SignatureEximinedDoctor = Encoding.UTF8.GetBytes($"data:image/png;base64, {GeneralConsultanMedicalSupport.SignatureEximinedDoctorBase64}");

            var generalConsultanService = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == GeneralConsultanMedicalSupport.GeneralConsultanServiceId))).Item1;

            if (generalConsultanService.Count == 0)
                return;

            //SelectedLabTests = LabTests.Where(x => GeneralConsultanMedicalSupport.LabResulLabExaminationtIds != null && GeneralConsultanMedicalSupport.LabResulLabExaminationtIds.Contains(x.Id)).ToList();

            GeneralConsultanService = generalConsultanService[0];

            LabResultDetails.Clear();
            DeletedLabTestIds.Clear();

            LabResultDetails = await Mediator.Send(new GetLabResultDetailQuery(x => x.GeneralConsultanMedicalSupportId == GeneralConsultanMedicalSupport.Id));
            PertamaKali = true;
            DeletedLabTestIds = LabResultDetails.Select(x => x.Id).ToList();

            //LabResultDetails.ForEach(x =>
            //{
            //    if (GeneralConsultanService.Patient.Gender is not null)
            //    {
            //        if (GeneralConsultanService.Patient.Gender.Equals("Male"))
            //            x.LabTest.NormalRangeByGender = x.LabTest.NormalRangeMale;
            //        else
            //            x.LabTest.NormalRangeByGender = x.LabTest.NormalRangeFemale;
            //    }
            //});

            switch (GeneralConsultanMedicalSupport.Status)
            {
                case EnumStatusGeneralConsultantServiceProcedureRoom.Draft:
                    StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.InProgress.GetDisplayName();
                    break;

                case EnumStatusGeneralConsultantServiceProcedureRoom.InProgress:
                case EnumStatusGeneralConsultantServiceProcedureRoom.Finish:
                    StagingText = EnumStatusGeneralConsultantServiceProcedureRoom.Finish.GetDisplayName();
                    break;

                default:
                    break;
            }
        }

        private void OnCancel()
        {
            ShowForm = false;
            GeneralConsultanMedicalSupport = new();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

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

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile");
        }

        #endregion ToolBar Button

        #endregion Grid Function
    }
}