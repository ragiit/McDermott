﻿using DevExpress.Blazor.Reporting;
using DevExpress.Blazor.Reporting.Models;
using DevExpress.XtraReports;
using McDermott.Web.Components.Pages.Reports;
using static McDermott.Application.Features.Commands.GetDataCommand;

namespace McDermott.Web.Components.Pages.Transaction.GeneralConsultationServices
{
    public partial class GeneralConsultationServicezPage
    {
        private List<GeneralConsultanServiceDto> GeneralConsultanServices { get; set; } = [];

        #region Default Grid

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

        private string FormUrl = "clinic-service/general-consultation-services";
        private bool PanelVisible { get; set; } = true;
        public IGrid Grid { get; set; }
        private int FocusedRowVisibleIndex { get; set; }
        private IReadOnlyList<object> SelectedDataItems { get; set; } = [];

        private DxReportViewer reportViewer { get; set; }
        private IReport Report { get; set; }
        private ElementReference viewerComponent;

        private void OnCustomizeToolbar(ToolbarModel toolbarModel)
        {
            toolbarModel.AllItems.Add(new ToolbarItem()
            {
                // Use Open Iconic's icon.
                IconCssClass = "oi oi-command",
                Text = "Full Screen",
                AdaptiveText = "Full Screen",
                AdaptivePriority = 1,
                Click = async (args) =>
                {
                    await JsRuntime.InvokeVoidAsync("customApi.requestFullscreen", viewerComponent);
                }
            });
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                PanelVisible = true;
                await GetUserInfo();
                await LoadData();

                if (GeneralConsultanServices.Count == 0)
                    return;

                IsDeleteGC = GeneralConsultanServices.FirstOrDefault()?.Status == EnumStatusGeneralConsultantService.Planned || GeneralConsultanServices.FirstOrDefault()?.Status == EnumStatusGeneralConsultantService.Canceled;

                using var stream = new MemoryStream();
                // Buat laporan
                var myReport = new ReportRujukanBPJS();

                var gx = await Mediator.Send(new GetSingleGeneralConsultanServicesQuery
                {
                    Predicate = x => x.Id == 103,
                    Select = x => new GeneralConsultanService
                    {
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                            DateOfBirth = x.Patient.DateOfBirth,
                            Gender = x.Patient.Gender,
                        },
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner.Name,
                        },

                        VisitNumber = x.VisitNumber,
                        ReferVerticalSpesialisParentSpesialisName = x.ReferVerticalSpesialisParentSpesialisName,
                        PPKRujukanName = x.PPKRujukanName,
                        ReferVerticalSpesialisSaranaName = x.ReferVerticalSpesialisSaranaName,
                        InsurancePolicyId = x.InsurancePolicyId,
                        ReferDiagnosisNm = x.ReferDiagnosisNm,
                        ReferDateVisit = x.ReferDateVisit,
                        PracticeScheduleTimeDate = x.PracticeScheduleTimeDate,
                        ReferralExpiry = x.ReferralExpiry,
                        ReferSelectFaskesDate = x.ReferSelectFaskesDate,
                        ReferralNo = x.ReferralNo
                    }
                }) ?? new();

                var inspolcy = await Mediator.Send(new GetSingleInsurancePolicyQuery
                {
                    Predicate = x => x.Id == gx.InsurancePolicyId,
                    Select = x => new InsurancePolicy
                    {
                        PolicyNumber = x.PolicyNumber,
                        JnsKelasKode = x.JnsKelasKode
                    }
                }) ?? new();

                // Header
                myReport.xrLabelNoKunjungan.Text = gx.VisitNumber;

                myReport.xrLabelTsDokter.Text = gx.ReferVerticalSpesialisParentSpesialisName; // Spesialis
                myReport.xrLabelDi.Text = gx.PPKRujukanName;

                // Patient
                myReport.xrLabelNama.Text = gx.Patient?.Name ?? "-";
                myReport.xrLabelBPJS.Text = inspolcy.PolicyNumber;
                myReport.xrLabelDiagnosaa.Text = gx.ReferDiagnosisNm; // Diagnosa
                myReport.xrLabelUmur.Text = gx.Patient?.Age.ToString() ?? "-";
                myReport.xrLabelTahun.Text = gx.Patient?.DateOfBirth.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelStatusTanggunan.Text = inspolcy.JnsKelasKode;
                myReport.xrLabelGender.Text = gx.Patient?.Gender == "Male" ? "L" : "P";

                myReport.xrLabelRencana.Text = gx.ReferDateVisit.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelJadwal.Text = gx.PracticeScheduleTimeDate;
                myReport.xrLabelBerlakuDate.Text = gx.ReferralExpiry.GetValueOrDefault().ToString("dd-MMM-yyyy");
                myReport.xrLabelSalamDate.Text = $"Salam sejawat,\r\n{gx.ReferSelectFaskesDate.GetValueOrDefault().ToString("dd MMMM yyyy")}";
                myReport.xrLabelDokter.Text = gx.Pratitioner?.Name ?? "-";

                myReport.xrBarCode1.Text = gx.ReferralNo;
                myReport.xrBarCode1.ShowText = false;

                Report = myReport;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private List<StatusMcuData> StatusMcus = [];

        private async Task OnDelete(GridDataItemDeletingEventArgs e)
        {
            try
            {
                if (SelectedDataItems.Count == 1)
                {
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>().Id));
                }
                else
                {
                    var a = SelectedDataItems.Adapt<List<GeneralConsultanServiceDto>>();
                    await Mediator.Send(new DeleteGeneralConsultanServiceRequest(ids: a.Select(x => x.Id).ToList()));
                }
                await LoadData();
            }
            catch { }
        }

        private bool IsDeleteGC = false;

        private void Grid_FocusedRowChanged(GridFocusedRowChangedEventArgs args)
        {
            FocusedRowVisibleIndex = args.VisibleIndex;

            if (args.DataItem is null)
                return;

            var d = ((GeneralConsultanService)args.DataItem).Status;

            IsDeleteGC = d == EnumStatusGeneralConsultantService.Planned || d == EnumStatusGeneralConsultantService.Canceled;
        }

        private async Task Refresh_Click()
        {
            await LoadData();
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

        private object Data { get; set; }

        private async Task LoadData()
        {
            try
            {
                PanelVisible = true;
                SelectedDataItems = [];
                var dataSource = new GridDevExtremeDataSource<GeneralConsultanService>(await Mediator.Send(new GetQueryGeneralConsultanService
                {
                    OrderByList =
                    [
                        (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
                        (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
                        (x => x.IsClaim, true),
                    ],
                    Select = x => new GeneralConsultanService
                    {
                        Id = x.Id,
                        Status = x.Status,
                        PatientId = x.PatientId,
                        Patient = new User
                        {
                            Name = x.Patient.Name,
                        },
                        PratitionerId = x.PratitionerId,
                        Pratitioner = new User
                        {
                            Name = x.Pratitioner.Name,
                        },
                        ServiceId = x.ServiceId,
                        Service = new Service
                        {
                            Name = x.Service == null ? string.Empty : x.Service.Name,
                            IsMaternity = x.Service == null ? false : x.Service.IsMaternity,
                            IsTelemedicine = x.Service == null ? false : x.Service.IsTelemedicine,
                            IsMcu = x.Service == null ? false : x.Service.IsMcu,
                            IsVaccination = x.Service == null ? false : x.Service.IsVaccination
                        },
                        Payment = x.Payment,

                        AppointmentDate = x.AppointmentDate,
                        IsAlertInformationSpecialCase = x.IsAlertInformationSpecialCase,
                        RegistrationDate = x.RegistrationDate,
                        TypeRegistration = x.TypeRegistration,
                        ClassType = x.ClassType,
                        SerialNo = x.SerialNo,
                        Reference = x.Reference,
                        VisitNumber = x.VisitNumber,
                        KioskQueue = new KioskQueue
                        {
                            QueueNumber = x.KioskQueue.QueueNumber
                        },
                        IsClaim = x.IsClaim,
                    }
                }))
                {
                    CustomizeLoadOptions = (loadOptions) =>
                    {
                        loadOptions.PrimaryKey = ["Id"];
                        loadOptions.PaginateViaPrimaryKey = true;
                    }
                };

                Data = dataSource;
                PanelVisible = false;
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private async Task LoadData(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                //Data = "GeneralConsultanServices.OrderByDescending(x => x.RegistrationDate).ThenByDescending(x => x.IsAlertInformationSpecialCase).ThenByDescending(x => x.ClassType is not null)"

                PanelVisible = true;
                SelectedDataItems = [];
                var a = await Mediator.Send(new GetGeneralConsultanServicesQuery
                {
                    OrderByList =
                    [
                        (x => x.RegistrationDate, true),               // OrderByDescending RegistrationDate
                        (x => x.IsAlertInformationSpecialCase, true),  // ThenByDescending IsAlertInformationSpecialCase
                        (x => x.ClassType != null, true),               // ThenByDescending ClassType is not null
                        (x=>x.IsClaim, true),
                    ],
                    Predicate = x => x.Service != null && x.Service.IsVaccination == false && x.Service.IsMcu == false && x.Service.IsMaternity == false && x.Service.IsTelemedicine == false,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    SearchTerm = searchTerm,
                });
                GeneralConsultanServices = a.Item1;
                totalCount = a.PageCount;
                activePageIndex = pageIndex;
                //var a = await Mediator.QueryGetHelper<GeneralConsultanService, GeneralConsultanServiceDto>(pageIndex, pageSize, searchTerm);
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        #endregion Searching

        private void NewItem_Click()
        {
            NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Create.GetDisplayName()}");
            return;
        }

        private void EditItem_Click()
        {
            try
            {
                var GeneralConsultanService = SelectedDataItems[0].Adapt<GeneralConsultanServiceDto>();
                NavigationManager.NavigateTo($"{FormUrl}/{EnumPageMode.Update.GetDisplayName()}?Id={GeneralConsultanService.Id}");
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
            finally { PanelVisible = false; }
        }

        private void DeleteItem_Click()
        {
            Grid.ShowRowDeleteConfirmation(FocusedRowVisibleIndex);
        }

        private void ColumnChooserButton_Click()
        {
            Grid.ShowColumnChooser();
        }

        #endregion Default Grid

        public MarkupString GetIssuePriorityIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsAlertInformationSpecialCase && priority.ClassType is null)
                    return new MarkupString("");

                string priorytyClass = "danger";
                string title = string.Empty;

                if (priority.IsAlertInformationSpecialCase && priority.ClassType is not null)
                    title = $" Priority, {priority.ClassType}";
                else
                {
                    if (priority.ClassType is not null)
                        title = $"{priority.ClassType}";
                    if (priority.IsAlertInformationSpecialCase)
                        title = $" Priority ";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        public MarkupString GetIssueClaimIconHtml(GeneralConsultanServiceDto priority)
        {
            if (priority is not null)
            {
                if (!priority.IsClaim)
                    return new MarkupString("");

                string priorytyClass = "info";
                string title = string.Empty;

                if (priority.IsClaim)
                    title = $" Claim";
                else
                {
                    if (priority.IsClaim)
                        title = $"Claim";
                }

                string html = string.Format("<span class='badge bg-{0} py-1 px-2' title='{1} Priority'>{1}</span>", priorytyClass, title);

                return new MarkupString(html);
            }
            return new MarkupString("");
        }

        private bool IsDashboard { get; set; } = false;

        #region Chart

        private async Task LoadDashboard(bool f)
        {
            if (f)
            {
                IsDashboard = true;
                var ser = await Mediator.Send(new GetGeneralConsultanServicesQuery
                {
                    IsGetAll = true,
                    Select = x => new GeneralConsultanService
                    {
                        Status = x.Status
                    }
                });

                StatusMcus = GetStatusMcuCounts(ser.Item1);
            }
            else
                IsDashboard = false;
        }

        public class StatusMcuData
        {
            public string Status { get; set; }
            public int Count { get; set; }
        }

        public List<StatusMcuData> GetStatusMcuCounts(List<GeneralConsultanServiceDto> services)
        {
            var aa = services.GroupBy(s => s.Status)
                            .Select(g => new StatusMcuData
                            {
                                Status = g.Key.GetDisplayName(),
                                Count = g.Count()
                            }).ToList();
            return aa;
        }

        #endregion Chart
    }
}