using DocumentFormat.OpenXml;
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class MedicalCheckupPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private (bool, GroupMenuDto, User) Test = new();
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

        private async Task ClearCanvas()
        {
            await JsRuntime.InvokeVoidAsync("clearCanvas");
        }

        private async Task SaveCanvas()
        {
            await JsRuntime.InvokeVoidAsync("saveCanvas");
        }

        private IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private bool ShowForm { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool IsDashboard { get; set; } = false;
        private string StagingText { get; set; } = EnumStatusMCU.HRCandidat.GetDisplayName();
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new()
        {
            TypeRegistration = "MCU",
            MedexType = "CANDIDATE EMPLOYEE PEA",
            TypeMedical = "Annual MCU",
        };

        private bool IsStatus(EnumStatusMCU status) => GeneralConsultanService.StatusMCU == status;

        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = [];
        private List<UserDto> Patients = [];
        private List<UserDto> Physicions = [];
        private List<ServiceDto> Services = [];

        private List<string> RegisType = new List<string>
        {
            "MCU"
        };

        private List<string> MedexType = new List<string>
        {
            "CANDIDATE EMPLOYEE PEA",
            "PRE-EMPLOYMENT POST PEA",
            "PRE-EMPLOYMENT FULL"
        };

        private List<string> MCUType = [
            "Annual MCU",
            "Pre Employment MCU",
            "Oil & Gas UK",
            "HIV & AIDS",
            "Covid19*",
            "Drug & Alcohol Test",
            "Maternity Checkup"
        ];

        private async Task SelectedItemServiceChanged(ServiceDto e)
        {
            try
            {
                if (e is null)
                {
                    GeneralConsultanService.ServiceId = null;
                    return;
                }

                Physicions = await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(e.Id)));
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        #region Chart

        public class StatusMcuData
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public List<StatusMcuData> GetStatusMcuCounts(List<GeneralConsultanServiceDto> services)
        {
            var aa = services.GroupBy(s => s.StatusMCU)
                            .Select(g => new StatusMcuData
                            {
                                Status = g.Key.GetDisplayName(),
                                Count = g.Count()
                            }).ToList();
            return aa;
        }

        #endregion Chart

        private async Task SelectedItemPatientChanged(UserDto e)
        {
            if (e is null)
            {
                GeneralConsultanService.Patient = new();
                return;
            }

            GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();
        }

        private async Task OnClickConfirm()
        {
            IsLoading = true;
            try
            {
                if (GeneralConsultanService.Id == 0)
                {
                    IsLoading = false;
                    return;
                }

                if (GeneralConsultanService.IsBatam)
                {
                    switch (GeneralConsultanService.StatusMCU)
                    {
                        case EnumStatusMCU.Draft:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.HRCandidat;
                            StagingText = EnumStatusMCU.Examination.GetDisplayName();
                            break;

                        case EnumStatusMCU.HRCandidat:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Examination;
                            StagingText = EnumStatusMCU.Result.GetDisplayName();
                            break;

                        case EnumStatusMCU.Examination:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Result;
                            StagingText = EnumStatusMCU.Done.GetDisplayName();
                            break;

                        case EnumStatusMCU.Result:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Done;
                            StagingText = EnumStatusMCU.Done.GetDisplayName();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (GeneralConsultanService.StatusMCU)
                    {
                        case EnumStatusMCU.Draft:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.EmployeeTest;
                            StagingText = EnumStatusMCU.HRCandidat.GetDisplayName();
                            break;

                        case EnumStatusMCU.EmployeeTest:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.HRCandidat;
                            StagingText = EnumStatusMCU.Examination.GetDisplayName();
                            break;

                        case EnumStatusMCU.HRCandidat:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Examination;
                            StagingText = EnumStatusMCU.Result.GetDisplayName();
                            break;

                        case EnumStatusMCU.Examination:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Result;
                            StagingText = EnumStatusMCU.Done.GetDisplayName();
                            break;

                        case EnumStatusMCU.Result:
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Done;
                            StagingText = EnumStatusMCU.Done.GetDisplayName();
                            break;

                        default:
                            break;
                    }
                }

                await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
            }
            catch (Exception Ex)
            {
                Ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private async Task OnDeleting(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null)
                    return;

                if (SelectedDataItems is not null && SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();

                    //a = a.Where(x => x.StagingStatus == "Planned" || x.StagingStatus == "Canceled").ToList();

                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private void NewItem_Click()
        {
            ShowForm = true;
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private async Task EditItem_Click()
        {
            ShowForm = true;
            IsLoading = true;
            try
            {
                GeneralConsultanService = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>().Id))).FirstOrDefault() ?? new();
                Physicions = (await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(GeneralConsultanService.ServiceId.GetValueOrDefault()))));
                GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();
                if (GeneralConsultanService is not null)
                {
                    if (GeneralConsultanService.IsBatam)
                    {
                        switch (GeneralConsultanService.StatusMCU)
                        {
                            case EnumStatusMCU.Draft:
                                StagingText = EnumStatusMCU.HRCandidat.GetDisplayName();
                                break;

                            case EnumStatusMCU.HRCandidat:
                                StagingText = EnumStatusMCU.Examination.GetDisplayName();
                                break;

                            case EnumStatusMCU.Examination:
                                StagingText = EnumStatusMCU.Result.GetDisplayName();
                                break;

                            case EnumStatusMCU.Result:
                                StagingText = EnumStatusMCU.Done.GetDisplayName();
                                break;

                            case EnumStatusMCU.Done:
                                StagingText = EnumStatusMCU.Done.GetDisplayName();
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (GeneralConsultanService.StatusMCU)
                        {
                            case EnumStatusMCU.Draft:
                                StagingText = EnumStatusMCU.EmployeeTest.GetDisplayName();
                                break;

                            case EnumStatusMCU.EmployeeTest:
                                StagingText = EnumStatusMCU.HRCandidat.GetDisplayName();
                                break;

                            case EnumStatusMCU.HRCandidat:
                                StagingText = EnumStatusMCU.Examination.GetDisplayName();
                                break;

                            case EnumStatusMCU.Examination:
                                StagingText = EnumStatusMCU.Result.GetDisplayName();
                                break;

                            case EnumStatusMCU.Result:
                                StagingText = EnumStatusMCU.Done.GetDisplayName();
                                break;

                            case EnumStatusMCU.Done:
                                StagingText = EnumStatusMCU.Done.GetDisplayName();
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private async Task OnRowDoubleClick(GridRowClickEventArgs e)
        {
        }

        private async Task LoadComboBox()
        {
            Patients = await Mediator.Send(new GetUserQuery(x => x.IsPatient == true || x.IsEmployeeRelation == true));
            Services = await Mediator.Send(new GetServiceQuery(x => x.IsMcu == true));
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private List<StatusMcuData> statusMcuData = [];

        private async Task LoadData()
        {
            ShowForm = false;
            GeneralConsultanService = new()
            {
                TypeRegistration = "MCU",
                MedexType = "CANDIDATE EMPLOYEE PEA",
                TypeMedical = "Annual MCU",
            };
            SelectedDataItems = [];
            GeneralConsultanServices = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.TypeRegistration == "MCU" && x.IsMcu == true));

            statusMcuData = GetStatusMcuCounts(GeneralConsultanServices);

            IsLoading = false;
        }

        private async Task ImportFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "fileInput");
        }

        private async Task ExportToExcel()
        {
            await Helper.GenerateColumnImportTemplateExcelFileAsync(JsRuntime, FileExportService, "medical_checkup_template.xlsx",
            [
                new()
                {
                    Column = "Patient",
                    Notes = "Mandatory \nNIP/Oracle/SAP"
                },
                new()
                {
                    Column = "Service",
                    Notes = "Mandatory"
                },
                 new()
                {
                    Column = "Physicion"
                },
                new()
                {
                    Column = "MCU Type",
                    Notes = "Mandatory \nSelect one: \nAnnual MCU \nPre Employment MCU \nOil & Gas UK \nHIV & AIDS \nCovid19* \nDrug & Alcohol Test \nMaternity Checkup"
                },
                new()
                {
                    Column = "Medex Type ",
                    Notes = "Mandatory \nSelect one: \nCANDIDATE EMPLOYEE PEA \nPRE-EMPLOYMENT POST PEA \nPRE-EMPLOYMENT FULL"
                },
                new()
                {
                    Column = "Candidate Form",
                    Notes = "Mandatory \nSelect one: \nBatam \nOutside Batam"
                },
                new()
                {
                    Column = "Registration Date",
                    Notes = "Mandatory \nDD-MM-YYYY"
                },
            ]);
        }

        public async Task ImportExcelFile(InputFileChangeEventArgs e)
        {
            IsLoading = true;
            foreach (var file in e.GetMultipleFiles(1))
            {
                try
                {
                    using MemoryStream ms = new();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using ExcelPackage package = new(ms);
                    ExcelWorksheet ws = package.Workbook.Worksheets.FirstOrDefault();

                    var headerNames = new List<string>() { "Patient", "Service", "Physicion", "MCU Type", "Medex Type", "Candidate Form", "Registration Date" };

                    if (Enumerable.Range(1, ws.Dimension.End.Column)
                        .Any(i => headerNames[i - 1].Trim().ToLower() != ws.Cells[1, i].Value?.ToString()?.Trim().ToLower()))
                    {
                        ToastService.ShowInfo("The header must match with the template.");
                        return;
                    }

                    var a = new List<GeneralConsultanServiceDto>();

                    bool IsValid = true;
                    for (int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        var col1Patient = ws.Cells[row, 1].Value?.ToString()?.Trim();
                        var patient = (await Mediator.Send(new GetUserQuery(x => x.NIP == col1Patient || x.Oracle == col1Patient || x.SAP == col1Patient))).FirstOrDefault() ?? null;
                        if (patient is null)
                        {
                            ShowErrorImport(row, 1, col1Patient);
                            IsValid = false;
                        }

                        var colService = ws.Cells[row, 2].Value?.ToString()?.Trim();
                        var ser = Services.FirstOrDefault(x => x.Name == colService);
                        if (ser is null)
                        {
                            ShowErrorImport(row, 2, colService);
                            IsValid = false;
                        }

                        var colPhysician = ws.Cells[row, 3].Value?.ToString()?.Trim();
                        long? phyId = null;
                        if (!string.IsNullOrWhiteSpace(colPhysician))
                        {
                            var cekPhys = (await Mediator.Send(new GetUserQuery(x => x.Name == colPhysician && x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(ser.Id)))).FirstOrDefault();
                            if (cekPhys is null)
                            {
                                ShowErrorImport(row, 3, colPhysician);
                                IsValid = false;
                            }
                            phyId = cekPhys.Id;
                        }

                        var col2MCUType = ws.Cells[row, 4].Value?.ToString()?.Trim();
                        if (MCUType.FirstOrDefault(x => x == col2MCUType) is null)
                        {
                            ShowErrorImport(row, 4, col2MCUType);
                            IsValid = false;
                        }

                        var col3Medex = ws.Cells[row, 5].Value?.ToString()?.Trim();
                        if (MedexType.FirstOrDefault(x => x == col3Medex) is null)
                        {
                            ShowErrorImport(row, 5, col3Medex);
                            IsValid = false;
                        }

                        var col4Candidate = ws.Cells[row, 6].Value?.ToString()?.Trim();
                        if (!col4Candidate.Equals("Batam") && !col4Candidate.Equals("Outside Batam"))
                        {
                            ShowErrorImport(row, 6, col4Candidate);
                            IsValid = false;
                        }

                        var col5Date = ws.Cells[row, 7].Value?.ToString()?.Trim();
                        bool successDate = DateTime.TryParseExact(col5Date, "dd-MM-yyyy",
                                            CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                                            out DateTime dateValue);

                        if (!successDate)
                        {
                            ShowErrorImport(row, 7, col5Date);
                            IsValid = false;
                        }

                        if (!IsValid)
                            continue;

                        var b = new GeneralConsultanServiceDto
                        {
                            PatientId = patient.Id,
                            TypeRegistration = "MCU",
                            RegistrationDate = dateValue,
                            IsMcu = true,
                            TypeMedical = col2MCUType,
                            PratitionerId = phyId,
                            ServiceId = ser.Id,
                            MedexType = col3Medex,
                            IsBatam = col4Candidate.Equals("Batam"),
                            IsOutsideBatam = col4Candidate.Equals("OutsideBatam"),
                        };

                        if (!GeneralConsultanServices.Any(x => x.PatientId == b.PatientId &&
                                       x.TypeRegistration == b.TypeRegistration &&
                                       x.RegistrationDate == b.RegistrationDate &&
                                       x.IsMcu == b.IsMcu &&
                                       x.ServiceId == ser.Id &&
                                       x.PratitionerId == phyId &&
                                       x.TypeMedical == b.TypeMedical &&
                                       x.MedexType == b.MedexType &&
                                       x.IsBatam == b.IsBatam &&
                                       x.IsOutsideBatam == b.IsOutsideBatam))
                        {
                            a.Add(b);
                        }
                    }

                    await Mediator.Send(new CreateListGeneralConsultanServiceRequest(a));

                    await LoadData();
                    SelectedDataItems = [];

                    ToastService.ShowSuccess("Successfully Imported.");
                }
                catch (Exception ex)
                {
                    ToastService.ShowError(ex.Message);
                }
            }

            IsLoading = false;
        }

        private void ShowErrorImport(int row, int col, string val)
        {
            ToastService.ShowInfo($"Data \"{val}\" in row {row} and column {col} is invalid");
        }

        private async Task OnValidSubmitSave()
        {
            try
            {
                IsLoading = true;
                try
                {
                    GeneralConsultanService.IsMcu = true;

                    if (GeneralConsultanService.StatusMCU == EnumStatusMCU.Draft)
                    {
                        if (GeneralConsultanService.IsBatam)
                            StagingText = EnumStatusMCU.HRCandidat.GetDisplayName();
                        else
                            StagingText = EnumStatusMCU.EmployeeTest.GetDisplayName();
                    }

                    if (GeneralConsultanService.Id == 0)
                        GeneralConsultanService = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                    else
                        await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                    GeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId) ?? new();

                    ToastService.ClearInfoToasts();
                    ToastService.ShowInfo("Saved Successfully");
                }
                catch (Exception ex)
                {
                    ex.HandleException(ToastService);
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoading = false;
        }

        private async Task OnInvalidSubmitSave()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

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

                await LoadComboBox();
                StateHasChanged();

                Grid?.SelectRow(0, true);
                await JsRuntime.InvokeVoidAsync("initializeSignaturePad");
            }
        }

        private async Task OnClickCancel()
        {
            ShowForm = false;
            await LoadData();
        }
    }
}