using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using DevExpress.Blazor.RichEdit;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel;
using static McDermott.Application.Features.Commands.Transaction.AccidentCommand;

namespace McDermott.Web.Components.Pages.Transaction
{
    public partial class AccidentPage
    {
        #region UserLoginAndAccessRole

        private bool IsStatus(EnumStatusAccident status) => Accident.Status == status;

        private bool IsStatusGenSet(EnumStatusGeneralConsultantService status) => GeneralConsultanService.Status == status;

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private (bool, GroupMenuDto, User) Test = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;
        private string SelectedRegType { get; set; } = "Accident";

        public static string GetNullText(IEnumerable<string> natureOfInjury) =>
      string.Join(" / ", natureOfInjury);

        private async Task OnClickPopUpHistoricalMedical()
        {
            if (GeneralConsultanService.PatientId == 0 || GeneralConsultanService.PatientId is null)
            {
                ToastService.ShowInfo("Please select the Employee");
                return;
            }
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
                    // Retrieve the 'id' query parameter from the URL
                    var query = new Uri(NavigationManager.Uri).Query;
                    var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(query);

                    if (queryParams.TryGetValue("id", out var idParam))
                    {
                        if (long.TryParse(idParam, out var id))
                        {
                            ShowForm = true;
                            IsLoading = true;
                            Id = id;
                            GeneralConsultanService = ((await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == Id))).Item1).FirstOrDefault() ?? new();

                            await LoadSelectedData();
                            IsLoading = false;
                        }
                    }
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    var aaa = ex;
                }

                //Grid?.SelectRow(0, true);
                StateHasChanged();
            }
        }

        private async Task ShowPopup()
        {
            isAccident = true;
        }

        #region Test

        private BECanvasComponent canvasReference;
        private Canvas2DContext ctx;
        private List<Point> operationPoints = new List<Point>();
        private int pointCounter = 1; // Untuk memberi nomor urut pada titik
        private Point? selectedPoint = null;

        public class Point
        {
            public int Number { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }

        private async Task OnPopupShown2()
        {
            await LoadImageAsync("image/aciddent.png");
            await LoadPoints(); // Memuat titik-titik yang sudah tersimpan
            await DrawPointsAsync();
        }

        private async Task LoadImageAsync(string ImagePath)
        {
            await JsRuntime.InvokeVoidAsync("loadImageToCanvas", canvasReference, ImagePath);
        }

        private async Task HandleClick(MouseEventArgs e)
        {
            if (selectedPoint != null)
            {
                selectedPoint.X = e.ClientX;
                selectedPoint.Y = e.ClientY;
                selectedPoint = null;
            }
            else
            {
                var point = new Point
                {
                    X = e.ClientX,
                    Y = e.ClientY,
                    Number = pointCounter++
                };

                operationPoints.Add(point);
            }

            await DrawPointsAsync();
        }

        private async Task DrawPointsAsync()
        {
            await ctx.ClearRectAsync(0, 0, 500, 700); // Bersihkan canvas sebelum menggambar
            await LoadImageAsync("images/aciddent.png"); // Load ulang gambar

            foreach (var point in operationPoints)
            {
                await ctx.BeginPathAsync();
                await ctx.ArcAsync(point.X, point.Y, 5, 0, 2 * Math.PI);
                await ctx.FillAsync();
                await ctx.FillTextAsync(point.Number.ToString(), point.X + 10, point.Y);
            }
        }

        private async Task SavePoints()
        {
            var pointData = operationPoints.Select(p => new { p.X, p.Y, p.Number }).ToList();
            var jsonMarker = System.Text.Json.JsonSerializer.Serialize(pointData);

            // Simpan data JSON marker ke dalam GeneralConsultanService
            GeneralConsultanService.Markers = jsonMarker;

            // Kirim request menggunakan Mediator untuk memperbarui GeneralConsultanService
            var updateRequest = new UpdateGeneralConsultanServiceRequest(GeneralConsultanService);
            await Mediator.Send(updateRequest);
        }

        private async Task LoadPoints()
        {
            var dto = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == GeneralConsultanService.Id))).Item1.FirstOrDefault(); // Gantilah ID sesuai kebutuhan

            if (dto != null)
            {
                operationPoints = System.Text.Json.JsonSerializer.Deserialize<List<Point>>(dto.Markers);
            }
            await DrawPointsAsync();
        }

        private void EditPoint(Point point)
        {
            selectedPoint = point;
        }

        private void DeletePoint(Point point)
        {
            operationPoints.Remove(point);
            StateHasChanged(); // Render ulang UI
        }

        private async Task ResetPoints()
        {
            operationPoints.Clear();
            pointCounter = 1;
            await DrawPointsAsync();
        }

        #endregion Test

        private async Task OnPopupShown()
        {
            IsLoading = true;

            try
            {
                if (!string.IsNullOrEmpty(GeneralConsultanService.Markers))
                {
                    var markers = JsonConvert.DeserializeObject<List<Marker>>(GeneralConsultanService.Markers) ?? new List<Marker>();
                    await JsRuntime.InvokeVoidAsync("initializeCanvas", "MyCanvas", markers);
                }
                else
                {
                    await JsRuntime.InvokeVoidAsync("initializeCanvas", "MyCanvas");
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ResetMarkers()
        {
            await JsRuntime.InvokeVoidAsync("resetMarkers");
        }

        private string description { get; set; }

        private async Task SaveMarkers()
        {
            IsLoading = true;

            try
            {
                if (GeneralConsultanService.Id != 0)
                {
                    // Get the latest markers data from the JavaScript side
                    var markersData = await JsRuntime.InvokeAsync<string>("getMarkersData", "MyCanvas");

                    GeneralConsultanService.Markers = markersData;
                    GeneralConsultanService.Description = description;

                    var updatedRecord = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                    await EditItem_Click();
                }
                else
                {
                    ToastService.ShowInfo("Data Accident cannot be null");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveMarkers: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private List<IBrowserFile> BrowserFiles = [];

        private async void SelectFiles(InputFileChangeEventArgs e)
        {
            var file = e.File;
            bool isError = false;

            // Validate file size
            if (file.Size > 10 * 1024 * 1024)
            {
                // Handle error: File too large
                ToastService.ShowError("The file is too large. Maximum allowed size is 10 MB.");
                isError = true;
            }

            // Validate file type
            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                // Handle error: File is not PDF
                ToastService.ShowError("Invalid file type. Only PDF files are allowed.");
                isError = true;
            }

            if (isError)
                return;

            BrowserFiles.Clear();
            BrowserFiles.Add(e.File);

            GeneralConsultanService.AccidentExaminationDocs = e.File.Name;

            using (var memoryStream = new MemoryStream())
            {
                await e.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(memoryStream); // Batas ukuran file 10 MB
                GeneralConsultanService.AccidentExaminationBase64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            StateHasChanged();
        }

        private async Task DownloadFile()
        {
            IsLoading = true;
            try
            {
                if (GeneralConsultanService.Id != 0 && !string.IsNullOrWhiteSpace(GeneralConsultanService.AccidentExaminationBase64))
                {
                    // Contoh data base64
                    var base64Data = $"data:application/pdf;base64,{GeneralConsultanService.AccidentExaminationBase64}";
                    var fileNames = GeneralConsultanService.AccidentExaminationDocs;

                    // Memanggil fungsi JavaScript untuk mengunduh file
                    await JsRuntime.InvokeVoidAsync("downloadBase64File", base64Data, fileNames);
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

        private void RemoveSelectedFile()
        {
            GeneralConsultanService.AccidentExaminationDocs = null;
            GeneralConsultanService.AccidentExaminationBase64 = null;
        }

        private async Task SelectFile()
        {
            await JsRuntime.InvokeVoidAsync("clickInputFile", "file");
        }

        private void OnSelectAccidentLocation(string e)
        {
            if (e is null)
                return;

            if (e.Equals("Outside"))
            {
                Accident.Sent = "Hospital";
            }
        }

        public byte[]? DocumentContent;
        private bool isPrint { get; set; } = false;
        private DxRichEdit richEdit;
        private DevExpress.Blazor.RichEdit.Document documentAPI;

        private async Task PrintAccident()
        {
            try
            {
                if (GeneralConsultanService.Id == 0)
                    return;

                var gen = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == GeneralConsultanService.Id))).Item1.FirstOrDefault() ?? new();
                //var accident = (await Mediator.Send(new GetAccidentQuery(x => x.GeneralConsultanServiceId == gen.Id))).FirstOrDefault() ?? new();
                var accident = await Mediator.Send(new GetSingleAccidentQuery
                {
                    Predicate = x => x.GeneralConsultanServiceId == gen.Id
                });
                isPrint = true;
                var mergeFields = new Dictionary<string, string>
                {
                    {"<<EmployeeName>>", gen?.Patient?.Name.GetDefaultValue() },
                    {"<<EmployeeNIP>>", gen?.Patient?.NIP?.GetDefaultValue() ?? "-"},
                    {"<<EmployeeDepartment>>", gen?.Patient?.Department?.Name.GetDefaultValue() ?? "-"},
                    {"<<DateOfOccurence>>", accident.DateOfOccurrence.ToString("dd MMMM yyyy")},
                    {"<<TimeOccurence>>", accident.DateOfOccurrence.ToString("HH:mm:ss")},
                    {"<<DateTreatment>>", accident.DateOfFirstTreatment.ToString("dd MMMM yyyy")},
                    {"<<TimeTreatment>>", accident.DateOfFirstTreatment.ToString("HH:mm:ss")},
                    {"<<AreaOfYard>>", accident.AreaOfYard?.GetDefaultValue() ?? "-"},
                    {"<<SupervisorName>>", gen?.Patient?.Supervisor?.Name.GetDefaultValue() ?? "-"},
                };

                //Field dateField = await documentAPI.Fields.Create(, "DATE");

                //await documentAPI.Fields.UpdateAsync(dateField);

                DocumentContent = await DocumentProvider.GetDocumentAsync("AccidentForms.docx", mergeFields);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
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

        private async Task OnClickCancel()
        {
            ShowForm = false;
            Accident = new();
            await LoadData();
        }

        private async Task ClosePopUp()
        {
            isPrint = false;
            await EditItem_Click();
        }

        private async Task ClosePopUpAccident()
        {
            isAccident = false;
        }

        private List<string> ClassType = new List<string>
        {
            "FA",
            "MTC",
            "RWC",
            "LTA",
            "FATALITY",
            "OCCUPATIONAL ILLNESS"
        };

        private List<string> RegisType = new List<string>
        {
            "Accident"
        };

        private List<string> SentType = new List<string>
        {
            "Back to Work",
            "Main Clinic",
            "Home",
            "Hospital",
            //"MCU"
        };

        #region Nature of Injury

        private void OnSelectSent(string e)
        {
            if (e is null)
            {
                return;
            }

            if (e.Equals("Hospital"))
            {
                StagingText = EnumStatusAccident.HospitalizationReferral;
            }

            //RefreshStagingText();
        }

        public static IEnumerable<string> NatureOfInjury1 { get; set; } = new List<string>
        {
            "Foreign body",
            "Eye irritation"
        };

        public static IEnumerable<string> NatureOfInjury2 { get; set; } = new List<string>
        {
            "Abrasion",
            "Laceration",
            "Puncture",
            "Scratch"
        };

        public static IEnumerable<string> NatureOfInjury3 { get; set; } = new List<string>
        {
            "Bruise",
            "Contussion",
            "Crushing"
        };

        public static IEnumerable<string> NatureOfInjury4 { get; set; } = new List<string>
        {
            "Sprain"
        };

        public static IEnumerable<string> NatureOfInjury5 { get; set; } = new List<string>
        {
            "Fracture",
            "Dislocation"
        };

        public static IEnumerable<string> NatureOfInjury6 { get; set; } = new List<string>
        {
            "Burn",
            "Chemical burn",
            "Electric burn"
        };

        public static IEnumerable<string> NatureOfInjury7 { get; set; } = new List<string>
        {
            "Occupational illness",
            "LBP",
            "Dermatitis"
        };

        public static IEnumerable<string> NatureOfInjury8 { get; set; } = new List<string>
        {
            "Asphyxia",
            "Intoxication",
            "Amputation",
            "Concussion"
        };

        #endregion Nature of Injury

        #region Part of Body

        public static IEnumerable<string> PartOfBody1 { get; set; } = [
            "Head",
            "face",
            "neck"
        ];

        public static IEnumerable<string> PartOfBody2 { get; set; } = [
            "Eye"
        ];

        public static IEnumerable<string> PartOfBody3 { get; set; } = [
            "Ear"
        ];

        public static IEnumerable<string> PartOfBody4 { get; set; } = [
            "Back"
        ];

        public static IEnumerable<string> PartOfBody5 { get; set; } = [
            "Trunk (except back and internal)"
        ];

        public static IEnumerable<string> PartOfBody6 { get; set; } = [
            "Arm"
        ];

        public static IEnumerable<string> PartOfBody7 { get; set; } = [
            "Hand and wrist"
        ];

        public static IEnumerable<string> PartOfBody8 { get; set; } = [
            "Fingers"
        ];

        public static IEnumerable<string> PartOfBody9 { get; set; } = [
            "Leg"
        ];

        public static IEnumerable<string> PartOfBody10 { get; set; } = [
            "Feet and ankles"
        ];

        public static IEnumerable<string> PartOfBody11 { get; set; } = [
            "Toes"
        ];

        public static IEnumerable<string> PartOfBody12 { get; set; } = [
            "Internal and Others"
        ];

        public static IEnumerable<string> EmployeeCauseOfInjury1 { get; set; } = new[]
        {
            "Falls",
            "Slips",
            "Trips"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury2 { get; set; } = new[]
        {
            "Fire",
            "hot materials"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury3 { get; set; } = new[]
        {
            "Pressurized gas"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury4 { get; set; } = new[]
        {
    "Foreign body"
};

        public static IEnumerable<string> EmployeeCauseOfInjury5 { get; set; } = new[]
        {
            "Electricity"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury6 { get; set; } = new[]
        {
            "Sandblast"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury7 { get; set; } = new[]
        {
            "Animal",
            "plant"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury8 { get; set; } = new[]
        {
            "Struck",
            "caught (by, against, between) objects"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury9 { get; set; } = new[]
        {
            "Chemicals"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury10 { get; set; } = new[]
        {
            "Welding flash"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury11 { get; set; } = new[]
        {
            "Vehicle accident"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury12 { get; set; } = new[]
        {
            "Overexertion"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury13 { get; set; } = new[]
        {
            "Smoke",
            "gas"
        };

        public static IEnumerable<string> EmployeeCauseOfInjury14 { get; set; } = new[]
        {
            "Others"
        };

        #endregion Part of Body

        #region Treatment

        public static IEnumerable<string> Treatment1 { get; set; } = [
            "Cleaning and dressing",
            "Removal of foreign body with cotton wool",
            "Removal of foreign body with needle and magnet"
        ];

        public static IEnumerable<string> Treatment2 { get; set; } = [
            "Stitching"
        ];

        public static IEnumerable<string> Treatment3 { get; set; } = [
            "Splinting"
        ];

        public static IEnumerable<string> Treatment4 { get; set; } = [
            "Antibiotics"
        ];

        public static IEnumerable<string> Treatment5 { get; set; } = [
            "Painkillers"
        ];

        public static IEnumerable<string> Treatment6 { get; set; } = [
            "Tetanus toxoid injection, 0.5 cc"
        ];

        public static IEnumerable<string> Treatment7 { get; set; } = [
            "Others"
        ];

        #endregion Treatment

        public static IEnumerable<string> EmployeeClass { get; set; } = [
           "FA",
           "MTC",
           "RWC",
           "LTA",
           "FATALITY",
           "OCCUPATIONAL ILLNESS",
       ];

        #endregion UserLoginAndAccessRole

        private EnumStatusAccident StagingText = EnumStatusAccident.MedicalTreatment;

        private bool PanelVisible { get; set; } = true;
        private bool ShowForm { get; set; } = false;
        private bool isAccident { get; set; } = false;
        private int FocusedRowVisibleIndex { get; set; }

        private List<UserDto> Physicions { get; set; } = [];
        public IGrid Grid { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];
        private List<InsurancePolicyDto> InsurancePolicies { get; set; } = [];
        private InsurancePolicyDto SelectedInsurancePolicy { get; set; } = new();
        private UserDto UserForm = new();
        private GeneralConsultanServiceDto GeneralConsultanService { get; set; } = new();
        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = [];

        private async Task SelectedItemInsurancePolicyChanged(InsurancePolicyDto result)
        {
        }

        private List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        private List<string> AccidentLocations = new List<string>
        {
            "Inside",
            "Outside"
        };

        private async Task SelectedItemPaymentChanged(string e)
        {
            //GeneralConsultanService.Payment = null;
            //GeneralConsultanService.InsurancePolicyId = null;
            //SelectedInsurancePolicy = new();

            SelectedInsurancePolicy = new();

            if (e is null)
                return;

            await OnChangeInsuracePolicies(GeneralConsultanService.Patient, e);
        }

        private async Task SelectedItemPaymentChangedReferTo(string e)
        {
            //SelectedInsurancePolicy = new();

            //if (e.Equals("BPJS"))
            //    InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
            //else
            //    InsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && (x.Insurance.IsBPJSKesehatan == false || x.Insurance.IsBPJSTK == false) && x.Active == true));

            //ReferToGeneralConsultanService.Payment = null;
            //ReferToGeneralConsultanService.InsurancePolicyId = null;
            //SelectedInsurancePolicyFollowUp = new();

            //if (e is null)
            //    return;

            //ReferToInsurancePolicies = await Mediator.Send(new GetInsurancePolicyQuery(x => x.UserId == ReferToGeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJSKesehatan == e.Equals("BPJS") && x.Active == true));
        }

        #region CPPT

        private List<NursingDiagnosesDto> NursingDiagnoses = [];
        private List<DiagnosisDto> Diagnoses = [];
        private DiagnosisDto SelectedDiagnosis { get; set; } = new();
        private int FocusedGridTabCPPTRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItemsCPPT { get; set; } = [];
        private NursingDiagnosesDto SelectedNursingDiagnosis { get; set; } = new();
        private List<GeneralConsultanCPPTDto> GeneralConsultanCPPTs = [];
        private List<AwarenessDto> Awareness { get; set; } = [];

        private List<string> ClinicVisitTypes = new List<string>
        {
            "Healthy",
            "Sick"
        }; private bool IsPopUpPainScale { get; set; } = false;

        private IGrid? GridCppt { get; set; }

        private void OnDeleteTabCPPTConfirm(GridDataItemDeletingEventArgs e)
        {
            GeneralConsultanCPPTs.Remove((GeneralConsultanCPPTDto)e.DataItem);
            GridCppt.Reload();
        }

        private void OnClickPainScalePopUp()
        {
            IsPopUpPainScale = true;
        }

        private bool IsLoadingCPPT = false;

        private async Task OnClickSaveCPPT()
        {
            IsLoadingCPPT = true;
            try
            {
                await Mediator.Send(new DeleteGeneralConsultanCPPTRequest(deleteByGeneralServiceId: GeneralConsultanService.Id));

                GeneralConsultanCPPTs.ForEach(x => { x.GeneralConsultanService = null; x.GeneralConsultanServiceId = GeneralConsultanService.Id; x.Id = 0; });
                GeneralConsultanCPPTs = await Mediator.Send(new CreateListGeneralConsultanCPPTRequest(GeneralConsultanCPPTs));

                ToastService.ShowSuccess("Successfully Saving CPPT");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            IsLoadingCPPT = false;
        }

        private void GridTabCPPT_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedGridTabCPPTRowVisibleIndex = args.VisibleIndex;
        }

        public static List<string> GetPropertyNames<T>(T obj)
        {
            List<string> propertyNames = new List<string>();
            Type type = typeof(T);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                propertyNames.Add(prop.Name);
            }

            return propertyNames;
        }

        private class InputCPPTGeneralConsultanCPPT
        {
            public string? Subjective { get; set; }
            public string? Objective { get; set; }

            [DisplayName("Planning")]
            public string? Plan { get; set; }

            public string? Diagnosis { get; set; }

            [DisplayName("Nurse Diagnosis")]
            public string? NursingDiagnosis { get; set; }

            public DateTime Date { get; set; } = DateTime.Now;
        }

        private InputCPPTGeneralConsultanCPPT FormInputCPPTGeneralConsultan = new();

        private void OnClickConfirmCPPT()
        {
            var temps = new List<GeneralConsultanCPPTDto>();
            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = UserLogin.Name,
            });
            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = "Date and Time",
                Body = DateTime.Now.ToString("dd-MMM-yyy HH:mm:tt")
            });

            foreach (var key in GetPropertyNames(new InputCPPTGeneralConsultanCPPT()))
            {
                if (key == "Date")
                    continue;

                PropertyInfo property = typeof(InputCPPTGeneralConsultanCPPT).GetProperty(key);
                object? value = property?.GetValue(FormInputCPPTGeneralConsultan);

                if (value != null)
                {
                    string title = key;

                    if (title.Equals("Plan"))
                        title = "Planning";
                    else if (title.Equals("NursingDiagnosis"))
                        title = "Nurse Diagnosis";

                    temps.Add(new GeneralConsultanCPPTDto
                    {
                        Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                        Title = title,
                        Body = (value is null ? "" : value.ToString()) ?? "",
                    });

                    //if (title.Equals("Planning"))
                    //{
                    //    if (IsStatus(EnumStatusGeneralConsultantService.NurseStation))
                    //    {
                    //        temps.Add(new GeneralConsultanCPPTDto
                    //        {
                    //            Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                    //            Title = "Nursing Diagnosis",
                    //            Body = SelectedNursingDiagnosis.Problem,
                    //        });
                    //    }
                    //    else
                    //    {
                    //        temps.Add(new GeneralConsultanCPPTDto
                    //        {
                    //            Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                    //            Title = "Diagnosis",
                    //            Body = SelectedDiagnosis.Name,
                    //        });
                    //    }
                    //}
                }
            }

            temps.Add(new GeneralConsultanCPPTDto
            {
                Id = new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second,
                Title = "Diagnosis",
                Body = SelectedDiagnosis.Name,
            });
            GeneralConsultanCPPTs.AddRange(temps);

            GridCppt.Reload();
            OnClickClearCPPT();
        }

        private void OnClickClearCPPT()
        {
            FormInputCPPTGeneralConsultan = new InputCPPTGeneralConsultanCPPT();
            SelectedDiagnosis = new();
            SelectedNursingDiagnosis = new();
        }

        private void OnClickGetObjectives()
        {
            FormInputCPPTGeneralConsultan.Objective = $"Weight: {GeneralConsultanService.Weight}, Height: {GeneralConsultanService.Height}, RR: {GeneralConsultanService.RR}, SpO2: {GeneralConsultanService.SpO2}, BMIIndex: {Math.Round(GeneralConsultanService.BMIIndex, 2).ToString()}, BMIState: {GeneralConsultanService.BMIState}, Temp: {GeneralConsultanService.Temp}, HR: {GeneralConsultanService.HR}, Systolic: {GeneralConsultanService.Systolic}, DiastolicBP: {GeneralConsultanService.DiastolicBP}, E: {GeneralConsultanService.E}, V: {GeneralConsultanService.V}, M: {GeneralConsultanService.M}";
        }

        private void OnClickClearAllCPPT()
        {
            GeneralConsultanCPPTs.Clear();
        }

        #endregion CPPT

        private AccidentDto Accident { get; set; } = new();
        private List<AccidentDto> Data = [];
        private List<DepartmentDto> Departments = [];
        private List<UserDto> Employees = [];

        private string SelectedNIP { get; set; } = "-";
        private string SelectedDepartment { get; set; } = "-";

        private async Task OnClickConfirm()
        {
            try
            {
                IsLoading = true;
                if (Accident.Id != 0)
                {
                    if (GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS"))
                    {
                        switch (Accident.Status)
                        {
                            case EnumStatusAccident.Draft:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    Accident.Status = EnumStatusAccident.MedicalTreatment;
                                }
                                else
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                break;

                            case EnumStatusAccident.MedicalTreatment:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                break;

                            case EnumStatusAccident.HospitalizationReferral:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                else
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.PatientControlMonitoring;
                                    }
                                }
                                break;

                            case EnumStatusAccident.PatientControlMonitoring:
                                Accident.Status = EnumStatusAccident.Done;
                                break;

                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (Accident.Status)
                        {
                            case EnumStatusAccident.Draft:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    Accident.Status = EnumStatusAccident.MedicalTreatment;
                                }
                                else
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                    {
                                        Accident.Status = EnumStatusAccident.Done;
                                    }
                                }
                                break;

                            case EnumStatusAccident.MedicalTreatment:
                                if (Accident.AccidentLocation.Equals("Inside"))
                                {
                                    if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                                    {
                                        Accident.Status = EnumStatusAccident.HospitalizationReferral;
                                    }
                                    else
                                        Accident.Status = EnumStatusAccident.Done;
                                }
                                break;

                            case EnumStatusAccident.HospitalizationReferral:

                                Accident.Status = EnumStatusAccident.Done;
                                break;

                            default:
                                break;
                        }
                    }

                    GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));
                    Accident = await Mediator.Send(new UpdateAccidentRequest(Accident));

                    RefreshStagingText();
                }

                var usr = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();
                if (usr is not null)
                {
                    if (usr.CurrentMobile != UserForm.CurrentMobile)
                    {
                        usr.CurrentMobile = UserForm.CurrentMobile;
                        await Mediator.Send(new UpdateUserRequest(usr));
                        Employees = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true && x.IsPatient == true));
                        Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                    }
                }
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private void RefreshStagingText()
        {
            if (GeneralConsultanService.Payment is not null && GeneralConsultanService.Payment.Equals("BPJS"))
            {
                switch (Accident.Status)
                {
                    case EnumStatusAccident.Draft:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            StagingText = EnumStatusAccident.MedicalTreatment;
                        }
                        else
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                            {
                                StagingText = EnumStatusAccident.PatientControlMonitoring;
                            }
                        }
                        break;

                    case EnumStatusAccident.MedicalTreatment:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                            {
                                StagingText = EnumStatusAccident.PatientControlMonitoring;
                            }
                        }
                        break;

                    case EnumStatusAccident.HospitalizationReferral:
                        StagingText = EnumStatusAccident.PatientControlMonitoring;
                        break;

                    case EnumStatusAccident.PatientControlMonitoring:
                        StagingText = EnumStatusAccident.Done;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                switch (Accident.Status)
                {
                    case EnumStatusAccident.Draft:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            StagingText = EnumStatusAccident.MedicalTreatment;
                        }
                        else
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                            {
                                StagingText = EnumStatusAccident.Done;
                            }
                        }
                        break;

                    case EnumStatusAccident.MedicalTreatment:
                        if (Accident.AccidentLocation.Equals("Inside"))
                        {
                            if (Accident.Sent is not null && Accident.Sent.Equals("Hospital"))
                            {
                                StagingText = EnumStatusAccident.HospitalizationReferral;
                            }
                            else
                                StagingText = EnumStatusAccident.Done;
                        }
                        break;

                    case EnumStatusAccident.HospitalizationReferral:

                        StagingText = EnumStatusAccident.Done;
                        break;

                    default:
                        break;
                }
            }
        }

        private void RefreshDate()
        {
            Accident.DateOfFirstTreatment = DateTime.Now;
            Accident.DateOfOccurrence = DateTime.Now;
        }

        private async Task OnSelectEmployee(UserDto e)
        {
            SelectedNIP = "-";
            SelectedDepartment = "-";
            supName = "-";
            SelectedInsurancePolicy = new();
            GeneralConsultanService.Patient = new();
            if (e is null)
                return;

            SelectedNIP = e.NIP ?? "-";
            SelectedDepartment = e.Department?.Name ?? "-";
            supName = e.Supervisor?.Name ?? "-";

            UserForm = Employees.FirstOrDefault(x => x.Id == e.Id) ?? new();
            GeneralConsultanService.Patient = Employees.FirstOrDefault(x => x.Id == e.Id) ?? new();

            await OnChangeInsuracePolicies(e, GeneralConsultanService.Payment);

            //Accident.DepartmentId = e.DepartmentId.GetValueOrDefault();
            //SelectedNIP = e.NIP ?? "-";
        }

        private async Task OnChangeInsuracePolicies(UserDto d, string ee)
        {
            InsurancePolicies = (await Mediator.Send(new GetInsurancePolicyQuery
            {
                Predicate = x => x.UserId == GeneralConsultanService.PatientId && x.Insurance != null && x.Insurance.IsBPJS == GeneralConsultanService.Payment.Equals("BPJS") && x.Active == true
            })).Item1;
        }

        private string supName = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            PanelVisible = true;
        }

        private async Task RefreshSupervisorName()
        {
            var selectedSupervisor = await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId));
            var supervisor = selectedSupervisor.FirstOrDefault();

            if (supervisor != null)
            {
                supName = supervisor.Name;
            }
        }

        private async Task LoadComboBox()
        {
            Employees = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true && x.IsPatient == true));
            Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
            Diagnoses = (await Mediator.Send(new GetDiagnosisQuery())).Item1;
            Awareness = await Mediator.Send(new GetAwarenessQuery());
            Departments = (await Mediator.Send(new GetDepartmentQuery())).Item1;
            Projects = (await Mediator.Send(new GetProjectQuery())).Item1;
        }

        private async Task OnValidSubmitSave()
        {
            IsLoading = true;
            try
            {
                GeneralConsultanService.TypeRegistration = "Accident";

                GeneralConsultanService.InsurancePolicyId = SelectedInsurancePolicy.Id != 0 ? SelectedInsurancePolicy.Id : null;

                if (GeneralConsultanService.Id == 0)
                    GeneralConsultanService = await Mediator.Send(new CreateGeneralConsultanServiceRequest(GeneralConsultanService));
                else
                    GeneralConsultanService = await Mediator.Send(new UpdateGeneralConsultanServiceRequest(GeneralConsultanService));

                Accident.GeneralConsultanServiceId = GeneralConsultanService.Id;

                if (Accident.Id == 0)
                    Accident = await Mediator.Send(new CreateAccidentRequest(Accident));
                else
                    Accident = await Mediator.Send(new UpdateAccidentRequest(Accident));

                //Accident.Employee = Employees.FirstOrDefault(x => x.Id == General);
                //Accident.Department = Departments.FirstOrDefault(x => x.Id == Accident.DepartmentId);

                GeneralConsultanService.Patient = Employees.FirstOrDefault(x => x.Id == GeneralConsultanService.PatientId);

                RefreshStagingText();

                var usr = (await Mediator.Send(new GetUserQuery(x => x.Id == GeneralConsultanService.PatientId))).FirstOrDefault();
                if (usr is not null)
                {
                    if (usr.CurrentMobile != UserForm.CurrentMobile)
                    {
                        usr.CurrentMobile = UserForm.CurrentMobile;
                        await Mediator.Send(new UpdateUserRequest(usr));
                        Employees = await Mediator.Send(new GetUserQuery(x => x.IsEmployee == true && x.IsPatient == true));
                        Physicions = await Mediator.Send(new GetUserQuery(x => x.IsDoctor == true && x.IsPhysicion == true));
                    }
                }

                ToastService.ClearInfoToasts();
                ToastService.ShowSuccess("Saved Successfully");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }

            IsLoading = false;
        }

        private void OnInvalidSubmitSave()
        {
            ToastService.ShowInfoSubmittingForm();
        }

        private async Task LoadData()
        {
            PanelVisible = true;
            ShowForm = false;
            SelectedDataItems = [];
            GeneralConsultanServices = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.TypeRegistration == "Accident"))).Item1;
            // Populate the Accident property
            foreach (var service in GeneralConsultanServices)
            {
                //service.Accident = (await Mediator.Send(new GetAccidentQuery(x => x.GeneralConsultanServiceId == service.Id))).FirstOrDefault() ?? new();
            }

            GeneralConsultanService = new();
            Accident = new();
            PanelVisible = false;
        }

        private async Task OnClickBack()
        {
            await LoadData();
        }

        public async Task OnDeleting(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems is null || SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(((GeneralConsultanServiceDto)e.DataItem).Id));
                }
                else
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>().Select(x => x.Id).ToList()));
                }

                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task ExportToExcel()
        {
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;
        }

        private void NewItem_Click()
        {
            ShowForm = true;
            Accident = new()
            {
                Id = 0,

                //SelectedEmployeeCauseOfInjury1 = EmployeeCauseOfInjury1.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury2 = EmployeeCauseOfInjury2.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury3 = EmployeeCauseOfInjury3.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury4 = EmployeeCauseOfInjury4.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury5 = EmployeeCauseOfInjury5.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury6 = EmployeeCauseOfInjury6.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury7 = EmployeeCauseOfInjury7.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury8 = EmployeeCauseOfInjury8.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury9 = EmployeeCauseOfInjury9.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury10 = EmployeeCauseOfInjury10.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury11 = EmployeeCauseOfInjury11.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury12 = EmployeeCauseOfInjury12.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury13 = EmployeeCauseOfInjury13.Select(x => x).AsEnumerable(),
                //SelectedEmployeeCauseOfInjury14 = EmployeeCauseOfInjury14.Select(x => x).AsEnumerable(),

                //SelectedNatureOfInjury1 = NatureOfInjury1.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury2 = NatureOfInjury2.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury3 = NatureOfInjury3.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury4 = NatureOfInjury4.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury5 = NatureOfInjury5.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury6 = NatureOfInjury6.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury7 = NatureOfInjury7.Select(x => x).AsEnumerable(),
                //SelectedNatureOfInjury8 = NatureOfInjury8.Select(x => x).AsEnumerable(),

                //SelectedPartOfBody1 = PartOfBody1.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody2 = PartOfBody2.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody3 = PartOfBody3.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody4 = PartOfBody4.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody5 = PartOfBody5.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody6 = PartOfBody6.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody7 = PartOfBody7.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody8 = PartOfBody8.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody9 = PartOfBody9.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody10 = PartOfBody10.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody11 = PartOfBody11.Select(x => x).AsEnumerable(),
                //SelectedPartOfBody12 = PartOfBody12.Select(x => x).AsEnumerable(),

                //SelectedTreatment1 = Treatment1.Select(x => x).AsEnumerable(),
                //SelectedTreatment2 = Treatment2.Select(x => x).AsEnumerable(),
                //SelectedTreatment3 = Treatment3.Select(x => x).AsEnumerable(),
                //SelectedTreatment4 = Treatment4.Select(x => x).AsEnumerable(),
                //SelectedTreatment5 = Treatment5.Select(x => x).AsEnumerable(),
                //SelectedTreatment6 = Treatment6.Select(x => x).AsEnumerable(),
                //SelectedTreatment7 = Treatment7.Select(x => x).AsEnumerable()
            };

            RefreshDate();
            //StagingText = EnumStatusAccident.RestrictedWorkCase.GetDisplayName();
        }

        private bool IsLoading { get; set; } = false;

        [SupplyParameterFromQuery]
        public long Id { get; set; }

        private List<ProjectDto> Projects { get; set; } = [];

        private List<string> InformationFrom =
        [
            "Auto Anamnesa",
            "Allo Anamnesa"
        ];

        private List<string> HumptyDumpty =
 [
     "Risiko rendah 0-6",
     "Risiko sedang 7-11",
     "Risiko Tinggi >= 12"
 ];

        private List<string> Geriati =
       [
           "Risiko rendah 0-3",
       "Risiko Tinggi >= 4"
       ];

        private List<string> RiskOfFalling =
        [
            "Humpty Dumpty",
            "Morse",
            "Geriatri",
        ];

        private List<string> Morse =
        [
            "Risiko rendah 0-24",
            "Risiko sedang 25-44",
            "Risiko Tinggi >= 45"
        ];

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
                RiskOfFallingDetail = HumptyDumpty.ToList();
            }
            else if (e == "Morse")
            {
                RiskOfFallingDetail = Morse.ToList();
            }
            else
            {
                RiskOfFallingDetail = Geriati.ToList();
            }
        }

        private List<string> Colors = new List<string>
        {
            "Red",
            "Yellow",
            "Green",
            "Black",
        };

        private List<string> RiskOfFallingDetail = [];

        private async Task EditItem_Click()
        {
            ShowForm = true;
            isAccident = false;
            IsLoading = true;

            try
            {
                GeneralConsultanService = (await Mediator.Send(new GetGeneralConsultanServiceQuery(x => x.Id == SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>().Id))).Item1.FirstOrDefault() ?? new();
                UserForm = GeneralConsultanService.Patient ?? new();
                //NavigationManager.NavigateTo($"clinic-service/accident?id={GeneralConsultanService.Id}");
                //Id = GeneralConsultanService.Id;
                await LoadSelectedData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
                ShowForm = false;
            }
            IsLoading = false;
        }

        private async Task LoadSelectedData()
        {
            //Accident = (await Mediator.Send(new GetAccidentQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id))).FirstOrDefault() ?? new();

            GeneralConsultanCPPTs.Clear();
            GeneralConsultanCPPTs = await Mediator.Send(new GetGeneralConsultanCPPTQuery(x => x.GeneralConsultanServiceId == GeneralConsultanService.Id));

            await OnSelectEmployee(GeneralConsultanService.Patient);

            SelectedInsurancePolicy = InsurancePolicies.FirstOrDefault(x => x.Id == GeneralConsultanService.InsurancePolicyId) ?? new();

            await RefreshSupervisorName();
            RefreshStagingText();
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }
    }
}