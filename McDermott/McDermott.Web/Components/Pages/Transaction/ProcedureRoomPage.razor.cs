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

        private List<GeneralConsultanMedicalSupportDto> GeneralConsultanMedicalSupports = [];
        private GeneralConsultanMedicalSupportDto GeneralConsultanMedicalSupport = new();
        private GeneralConsultanServiceDto GeneralConsultanService = new();
        private List<UserDto> Doctors { get; set; } = [];
        private List<LabTestDto> LabTests = [];
        private List<IBrowserFile> BrowserFiles = [];

        private bool Loading = false;
        private string StagingText = "In-Progress";
        private bool ShowForm = false;
        private bool FormValidationState = false;
        private bool PopUpProcedureRoom = false;
        private bool PanelVisible { get; set; } = true;
        private int FocusedRowVisibleIndex { get; set; }
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        #endregion Properties

        #region LoadData

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;

            LabTests = await Mediator.Send(new GetLabTestQuery());
            Doctors = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));

            await GetUserInfo();
            await LoadData();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            ShowForm = false;
            GeneralConsultanService = new();
            GeneralConsultanMedicalSupport = new();
            SelectedDataItems = [];
            GeneralConsultanMedicalSupports = await Mediator.Send(new GetGeneralConsultanMedicalSupportQuery());
            PanelVisible = false;
        }

        #endregion LoadData

        #region Methods

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

                if (GeneralConsultanMedicalSupport.Status == "Finish")
                {
                    GeneralConsultanService.StagingStatus = "Waiting";

                    await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                }

                GeneralConsultanMedicalSupport = await Mediator.Send(new UpdateGeneralConsultanMedicalSupportRequest(GeneralConsultanMedicalSupport));

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

        private void Grid_CustomizeDataRowEditor(GridCustomizeDataRowEditorEventArgs e)
        {
            ((ITextEditSettings)e.EditSettings).ShowValidationIcon = true;
        }

        #region ToolBar Button

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private async Task EditItem_Click()
        {
            ShowForm = true;

            GeneralConsultanMedicalSupport = SelectedDataItems[0].Adapt<GeneralConsultanMedicalSupportDto>();

            var generalConsultanService = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == GeneralConsultanMedicalSupport.GeneralConsultanServiceId));

            if (generalConsultanService.Count == 0)
                return;

            GeneralConsultanService = generalConsultanService[0];

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