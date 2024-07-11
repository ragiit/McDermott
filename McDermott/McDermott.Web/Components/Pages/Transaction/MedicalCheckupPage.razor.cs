using DocumentFormat.OpenXml;

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

        private async Task CloseFollowUpClick()
        {
            IsFollowUp = false;
        }

        private bool IsFollowUp { get; set; } = false;
        private bool IsLoadingFollowUp { get; set; } = false;
        private InsurancePolicyDto SelectedInsurancePolicyFollowUp { get; set; } = new();
        private GeneralConsultanServiceDto FollowUpGeneralConsultanService { get; set; } = new();
        private List<InsurancePolicyDto> FollowUpInsurancePolicies { get; set; } = [];

        private async Task SelectedItemPatientChangedFollowUp(UserDto e)
        {
            FollowUpGeneralConsultanService.InsurancePolicyId = null;
            FollowUpInsurancePolicies.Clear();
            FollowUpGeneralConsultanService.Patient = new();
            SelectedInsurancePolicyFollowUp = new();

            if (e is null)
                return;

            FollowUpGeneralConsultanService.Patient = Patients.FirstOrDefault(x => x.Id == e.Id) ?? new();
            FollowUpGeneralConsultanService.PatientId = e.Id;

            FollowUpInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == e.Id && x.Insurance != null && FollowUpGeneralConsultanService.Payment != null && x.Insurance.IsBPJSKesehatan == FollowUpGeneralConsultanService.Payment.Equals("BPJS") && x.Active == true));
        }

        private List<string> Method = new List<string>
        {
            "MCU",
            "Gas And Oil"
        };

        private class HomeStatusTemp
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        private List<HomeStatusTemp> _homeStatusTemps = [
                    new()
            {
                Code = "1",
                Name = "Meninggal",
            },
            new()
            {
                Code = "3",
                Name = "Berobat Jalan",
            },
            new()
            {
                Code = "4",
                Name = "Rujuk Vertikal",
            },
            new()
            {
                Code = "6",
                Name = "Rujuk Horizontal",
            },
        ];

        private void SelectedItemRegisTypeChangedFollowUp(String e)
        {
            if (e is null)
            {
                return;
            }

            if (e.Equals("Emergency"))
            {
                Method =
                [
                    "General",
                    "Work Related Injury",
                    "Road Accident Injury",
                ];
                FollowUpGeneralConsultanService.TypeMedical = Method[0];
            }
            else if (e.Equals("MCU"))
            {
                Method =
                [
                    "Annual MCU",
                    "Pre Employment MCU",
                    "Oil & Gas UK",
                    "HIV & AIDS",
                    "Covid19*",
                    "Drug & Alcohol Test",
                    "Maternity Checkup"
                ];
                FollowUpGeneralConsultanService.TypeMedical = Method[0];
            }
            else if (e.Equals("General Consultation"))
                FollowUpGeneralConsultanService.TypeMedical = null;
        }

        private async Task HandleValidSubmitFollowUp()
        {
            IsLoadingFollowUp = true;
            try
            {
                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicyFollowUp == null || SelectedInsurancePolicyFollowUp.Id == 0 ? null : SelectedInsurancePolicyFollowUp.Id;
                if (!FollowUpGeneralConsultanService.Payment!.Equals("Personal") && (SelectedInsurancePolicyFollowUp is null || SelectedInsurancePolicyFollowUp.Id == 0))
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                if (FollowUpGeneralConsultanService.AppointmentDate is null)
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfoSubmittingForm();
                    return;
                }

                var patient = await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id != FollowUpGeneralConsultanService.Id && x.ServiceId == FollowUpGeneralConsultanService.ServiceId && x.PatientId == FollowUpGeneralConsultanService.PatientId && x.Status!.Equals(EnumStatusGeneralConsultantService.Planned) && x.RegistrationDate.GetValueOrDefault().Date <= DateTime.Now.Date));

                if (patient.Count > 0)
                {
                    IsLoadingFollowUp = false;
                    ToastService.ShowInfo($"Patient in the name of \"{patient[0].Patient?.Name}\" there is still a pending transaction");
                    return;
                }

                FollowUpGeneralConsultanService.Status = EnumStatusGeneralConsultantService.Planned;

                if (FollowUpGeneralConsultanService.Id == 0)
                    await Mediator.Send(new CreateGeneralConsultanServiceRequest(FollowUpGeneralConsultanService));

                ToastService.ShowSuccess("Successfully Follow Up Patient");
                IsFollowUp = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoadingFollowUp = false;
        }

        private void HandleInvalidSubmit()
        {
            ToastService.ShowInfoSubmittingForm();
        }

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

        private void OnReferToClick()
        {
            IsFollowUp = true;
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
                            GeneralConsultanService.StatusMCU = EnumStatusMCU.Result;
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

        private async Task SelectedItemServiceChangedFollowUp(ServiceDto e)
        {
            FollowUpGeneralConsultanService.ServiceId = null;

            if (!Convert.ToBoolean(UserLogin.IsEmployee) && !Convert.ToBoolean(UserLogin.IsPatient) && Convert.ToBoolean(UserLogin.IsUser) && !Convert.ToBoolean(UserLogin.IsNurse) && Convert.ToBoolean(UserLogin.IsDoctor) && Convert.ToBoolean(UserLogin.IsPhysicion))
            {
                Physicions = await Mediator.Send(new GetUserQuery(x => x.Id == UserLogin.Id));

                FollowUpGeneralConsultanService.PratitionerId = Physicions.Count > 0 ? Physicions[0].Id : null;
            }
            else
            {
                Physicions.Clear();

                if (e is null)
                {
                    FollowUpGeneralConsultanService.PratitionerId = null;
                    return;
                }

                Physicions = await Mediator.Send(new GetUserQuery(x => x.DoctorServiceIds != null && x.DoctorServiceIds.Contains(e.Id)));
            }
        }

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private BPJSIntegrationDto SelectedBPJSIntegrationFollowUp { get; set; } = new();

        private async Task SelectedItemInsurancePolicyChangedFollowUp(InsurancePolicyDto result)
        {
            ToastService.ClearInfoToasts();
            SelectedInsurancePolicyFollowUp = new();
            FollowUpGeneralConsultanService.InsurancePolicyId = null;

            SelectedBPJSIntegrationFollowUp = new();

            if (result is null)
                return;

            SelectedInsurancePolicyFollowUp = result;

            ToastService.ClearWarningToasts();

            var bpjs = (await Mediator.Send(new GetBPJSIntegrationQuery(x => x.InsurancePolicyId == result.Id))).FirstOrDefault();
            if (bpjs is not null)
            {
                var count = GeneralConsultanServices.Where(x => x.PatientId == FollowUpGeneralConsultanService.PatientId && x.Status == EnumStatusGeneralConsultantService.Planned).Count();
                if (!string.IsNullOrWhiteSpace(bpjs.KdProviderPstKdProvider))
                {
                    var parameter = (await Mediator.Send(new GetSystemParameterQuery(x => x.Key.Contains("pcare_code_provider")))).FirstOrDefault();
                    if (parameter is not null)
                    {
                        if (!Convert.ToBoolean(parameter.Value?.Equals(bpjs.KdProviderPstKdProvider)))
                        {
                            ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                        }
                        else
                        {
                            SelectedBPJSIntegrationFollowUp = bpjs;
                        }
                    }
                }
                else
                {
                    ToastService.ShowWarning($"Participants are not registered as your Participants. Participants have visited your FKTP {count} times.");
                }
            }
        }

        private async Task SelectedItemPaymentChangedFollowUp(string e)
        {
            FollowUpGeneralConsultanService.Payment = null;
            FollowUpGeneralConsultanService.InsurancePolicyId = null;
            SelectedInsurancePolicyFollowUp = new();

            if (e is null)
                return;

            FollowUpInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == FollowUpGeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
        }

        private List<ClassTypeDto> ClassTypes = [];

        private async Task LoadComboBox()
        {
            ClassTypes = await Mediator.Send(new GetClassTypeQuery());
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
                            PatientId = patient == null ? null : patient.Id,
                            TypeRegistration = "MCU",
                            RegistrationDate = dateValue,
                            IsMcu = true,
                            TypeMedical = col2MCUType,
                            PratitionerId = phyId,
                            ServiceId = ser == null ? null : ser.Id,
                            MedexType = col3Medex,
                            IsBatam = col4Candidate.Equals("Batam"),
                            StatusMCU = col4Candidate.Equals("Batam") ? EnumStatusMCU.HRCandidat : EnumStatusMCU.EmployeeTest,
                            IsOutsideBatam = col4Candidate.Equals("Outside Batam"),
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

                try
                {
                    Grid?.SelectRow(0, true);
                }
                catch { }
                //await JsRuntime.InvokeVoidAsync("initializeSignaturePad");
            }
        }

        private List<IBrowserFile> BrowserFiles = [];

        private async Task DownloadFile(string fileName)
        {
            if (GeneralConsultanService.Id != 0 && !string.IsNullOrWhiteSpace(fileName))
            {
                await Helper.DownloadFile(fileName, HttpContextAccessor, HttpClient, JsRuntime);
            }
        }

        private async void SelectFiles(InputFileChangeEventArgs e)
        {
            BrowserFiles.Add(e.File);

            GeneralConsultanService.McuExaminationDocs = e.File.Name;

            using (var memoryStream = new MemoryStream())
            {
                await e.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(memoryStream); // Batas ukuran file 10 MB
                GeneralConsultanService.McuExaminationBase64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            StateHasChanged();
            StateHasChanged();
        }

        private async Task SelectFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "file");
        }

        private void RemoveSelectedFile()
        {
            GeneralConsultanService.McuExaminationDocs = null;
            GeneralConsultanService.McuExaminationBase64 = null;
        }

        private async Task OnClickCancel()
        {
            ShowForm = false;
            await LoadData();
        }
    }
}