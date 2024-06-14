using Org.BouncyCastle.Tls;
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
                var user = await UserInfoService.GetUserInfo();
                IsAccess = user.Item1;
                UserAccessCRUID = user.Item2;
                UserLogin = user.Item3;
            }
            catch { }
        }

        #endregion UserLoginAndAccessRole

        #region Properties

        private List<LabResultDetailDto> LabResultDetails = [];
        private LabResultDetailDto LabResultDetail = new();
        private LabTestDetailDto LabTest = new();
        private LabUomDto LabUom = new();
        private GroupDto NameGroup { get; set; } = new();

        private List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupports = [];
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport = new();
        private GeneralConsultanServiceDto GeneralConsultanService = new();
        private List<GroupDto> groups = [];
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

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
            await GetUserInfo();

            LabUoms = await Mediator.Send(new GetLabUomQuery());
            LabTests = await Mediator.Send(new GetLabTestQuery());
            Doctors = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));

            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            PertamaKali = true;
            ShowForm = false;
            GeneralConsultanService = new();
            GeneralConsultanMedicalSupport = new();
            SelectedDataItems = [];
            GeneralConsultanMedicalSupports = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery());
            groups = await Mediator.Send(new GetGroupQuery());
            NameGroup = groups.FirstOrDefault(x => x.Id == UserAccessCRUID.GroupId) ?? new();
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

            var details = await Mediator.Send(new GetLabTestDetailQuery(x => x.LabTestId == e.Id));
            foreach (var item in details)
            {
                LabResultDetails.Add(new LabResultDetailDto
                {
                    IsFromDB = true,
                    Id = Helper.RandomNumber,
                    NormalRange = GeneralConsultanService.Patient.Gender.Name.Equals("Male") ? item.NormalRangeMale : item.NormalRangeFemale,
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
            //            NormalRange = GeneralConsultanService.Patient.Gender.Name.Equals("Male") ? item.NormalRangeMale : item.NormalRangeFemale,
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
                case "Draft":
                    StagingText = "Finish";
                    GeneralConsultanMedicalSupport.Status = "In-Progress";
                    break;

                case "In-Progress":
                case "Finish":
                    StagingText = "Finish";
                    GeneralConsultanMedicalSupport.Status = "Finish";
                    break;

                default:
                    break;
            }

            await OnSave();
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
        GeneralConsultanlogDto generalLog = new GeneralConsultanlogDto();
        private async Task OnSave()
        {
            try
            {
                Loading = true;

                if (GeneralConsultanMedicalSupport.Id == 0)
                    return;

                BrowserFiles.Distinct();

                foreach (var item in BrowserFiles)
                {
                    await FileUploadService.UploadFileAsync(item, 0, []);
                }

                GeneralConsultanMedicalSupport.LabResulLabExaminationtIds = SelectedLabTests.Select(x => x.Id).ToList();

                if (GeneralConsultanMedicalSupport.Status == "Finish")
                {
                    GeneralConsultanService.StagingStatus = "Waiting";

                    await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                    

                    generalLog.GeneralConsultanServiceId = GeneralConsultanMedicalSupport.GeneralConsultanServiceId;
                    generalLog.UserById = NameGroup.Id;
                    generalLog.Status = GeneralConsultanService.StagingStatus;

                    await Mediator.Send(new CreateGeneralConsultationLogRequest(generalLog));
                }

                GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

                generalLog.ProcedureRoomId = GeneralConsultanMedicalSupport.Id;
                generalLog.UserById = NameGroup.Id;
                generalLog.Status = GeneralConsultanMedicalSupport.Status;

                await Mediator.Send(new CreateGeneralConsultationLogRequest(generalLog));

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
                    await Mediator.Send(new DeleteGeneralConsultanMedicalSupportRequest(((GeneralConsultanMedicalSupportDto)e.DataItem).Id));
                }
                else
                {
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

        private async Task EditItem_Click()
        {
            ShowForm = true;

            GeneralConsultanMedicalSupport = SelectedDataItems[0].Adapt<GeneralConsultanMedicalSupportDto>();

            var generalConsultanService = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == GeneralConsultanMedicalSupport.GeneralConsultanServiceId));

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
                case "Draft":
                    StagingText = "In-Progress";
                    break;

                case "In-Progress":
                case "Finish":
                    StagingText = "Finish";
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