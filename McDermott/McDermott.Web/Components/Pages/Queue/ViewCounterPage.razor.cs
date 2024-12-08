using McDermott.Application.Dtos.Queue;
using Toolbelt.Blazor.SpeechSynthesis;
using static McDermott.Application.Features.Commands.Queue.CounterCommand;
using static McDermott.Application.Features.Commands.Queue.DetailQueueDisplayCommand;
using static McDermott.Application.Features.Commands.Queue.KioskQueueCommand;

namespace McDermott.Web.Components.Pages.Queue
{
    public partial class ViewCounterPage
    {
        #region UserLoginAndAccessRole

        [Inject]
        public UserInfoService UserInfoService { get; set; }

        private GroupMenuDto UserAccessCRUID = new();
        private User UserLogin { get; set; } = new();
        private bool IsAccess = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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

        #region Relation Data

        private List<CounterDto> counters = new();
        private List<KioskQueueDto> KiosksQueue = new();
        private List<KioskQueueDto> DataKiosksQueue = new();
        private KioskQueueDto FormKiosksQueue = new();
        private List<KioskDto> Kiosks = new();
        private List<ServiceDto> Services = [];
        private List<UserDto> Physicians = [];
        public List<UserDto> Phys = new();
        private List<ServiceDto> ServiceK = new();
        private List<ServiceDto> ServiceP = [];
        private User? User = new();
        private KioskQueueDto DataPatient = new();
        private KioskQueueDto FormCounters = new();

        #endregion Relation Data

        #region Grid properties

        public IGrid Grid { get; set; }

        #endregion Grid properties

        #region variabel data

        private HubConnection hubConnection;
        private string NameCounter { get; set; } = string.Empty;
        private string NameClasses { get; set; }
        private string NameServices { get; set; } = string.Empty;
        private string NameServicesK { get; set; } = string.Empty;
        private string? Phy { get; set; } = string.Empty;
        private string? userBy;
        private long? sId { get; set; }
        private long? PhysicianId { get; set; }
        private bool ShowPresent { get; set; }
        private bool PanelVisible { get; set; } = true;

        [Parameter]
        public long CounterId { get; set; }

        #endregion variabel data

        #region Async Data

        private CultureInfo indonesianCulture = new CultureInfo("id-ID");
        private string currentTime;
        private Timer timer;
        private CancellationTokenSource cts = new();

        private async Task UpdateTimeAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    currentTime = DateTime.Now.ToString("HH:mm:ss", indonesianCulture);
                    StateHasChanged();
                    await Task.Delay(500, token); // Update setiap 0.5 detik
                }
            }
            catch (TaskCanceledException)
            {
                // Dilewati saat dibatalkan
            }
        }

        protected override async Task OnInitializedAsync()
        {
            cts = new CancellationTokenSource();
            _ = UpdateTimeAsync(cts.Token); // Tidak menunggu
            try
            {
                await GetUserInfo();
                await LoadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            timer = new Timer(async _ => await LoadData(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
            timer?.Dispose();
            GC.SuppressFinalize(this);
        }

        private async Task LoadData()
        {
            try
            {
                await InvokeAsync(() => PanelVisible = true);

                var general = await Mediator.Send(new GetSingleCounterQuery { Predicate = x => x.Id == CounterId });
                DataKiosksQueue = await Mediator.Send(new GetKioskQueueQuery());

                KiosksQueue = DataKiosksQueue.Where(x =>
                    x.ServiceKId == general.ServiceKId &&
                    x.CreatedDate?.Date == DateTime.Now.Date &&
                    (general.ServiceId == null || x.ServiceId == general.ServiceId) &&
                    (general.PhysicianId == null || x.Kiosk.PhysicianId == general.PhysicianId) &&
                    (x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present"))
                    .ToList();

                var classes = await Mediator.Send(new GetClassTypeQuery());
                foreach (var queue in KiosksQueue)
                {
                    var classMatch = classes.FirstOrDefault(c => c.Id == queue.ClassTypeId);
                    queue.NameClass = classMatch?.Name ?? "-";
                }

                NameCounter = $"Queue Counter {general.Name}";
                sId = general.ServiceId;

                if (general.PhysicianId != null)
                {
                    var physician = await Mediator.Send(new GetSingleUserQuery
                    {
                        Predicate = x => x.Id == general.PhysicianId,
                        Select = x => new User { Id = x.Id, Name = x.Name }
                    });
                    Phy = physician?.Name ?? "-";
                }

                var servKiosk = await Mediator.Send(new GetSingleServiceQuery
                {
                    Predicate = x => x.Id == general.ServiceKId,
                    Select = x => new Service { Id = x.Id, Name = x.Name }
                });
                NameServicesK = servKiosk?.Name ?? "-";

                var serv = await Mediator.Send(new GetSingleServiceQuery
                {
                    Predicate = x => x.Id == sId,
                    Select = x => new Service { Id = x.Id, Name = x.Name }
                });
                NameServices = serv?.Name ?? "-";

                await InvokeAsync(() => PanelVisible = false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        //private async Task LoadData()
        //{
        //    try
        //    {
        //        await InvokeAsync(() =>
        //        {
        //            PanelVisible = true;
        //        });

        //        var us = await JsRuntime.GetCookieUserLogin();

        //        userBy = UserLogin.Name;

        //        ShowPresent = false;
        //        var general = await Mediator.Send(new GetSingleCounterQuery
        //        {
        //            Predicate = x => x.Id == CounterId
        //        });
        //        var resultService = await Mediator.Send(new GetServiceQuery());
        //        var physician = await Mediator.Send(new GetUserQuery());
        //        DataKiosksQueue = await Mediator.Send(new GetKioskQueueQuery());
        //        ServiceK = resultService.Item1.Where(x => x.IsKiosk == true).ToList();
        //        var GetClass = await Mediator.Send(new GetClassTypeQuery());

        //        //var cekServicesK = service
        //        if (general.ServiceId == null && general.PhysicianId == null)
        //        {
        //            KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date && (x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present"))];
        //        }
        //        else if (general.ServiceId != null && general.PhysicianId == null)
        //        {
        //            KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.ServiceId == general.ServiceId && x.CreatedDate.Value.Date == DateTime.Now.Date && (x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present"))];
        //        }
        //        else if (general.ServiceId == null && general.PhysicianId != null)
        //        {
        //            KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.Kiosk.PhysicianId == general.PhysicianId && x.CreatedDate.Value.Date == DateTime.Now.Date && x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present")];
        //        }
        //        else if (general.ServiceId != null && general.PhysicianId != null)
        //        {
        //            KiosksQueue = [.. DataKiosksQueue.Where(x => x.ServiceKId == general.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date && (x.Kiosk.PhysicianId == general.PhysicianId || x.Kiosk.PhysicianId == null) && x.ServiceId == general.ServiceId && x.QueueStage == null || x.QueueStage == "call" || x.QueueStage == "present")];
        //        }
        //        foreach (var cl in KiosksQueue)
        //        {
        //            var ClassId = KiosksQueue.Where(x => x.ClassTypeId == cl.ClassTypeId).Select(x => x.ClassTypeId).FirstOrDefault();
        //            var classList = GetClass.Where(x => x.Id == ClassId).FirstOrDefault();
        //            if (classList == null)
        //            {
        //                cl.NameClass = "-";
        //            }
        //            else
        //            {
        //                cl.NameClass = classList.Name;
        //            }
        //        }
        //        NameCounter = $"Queue Counter {general.Name}";

        //        sId = general.ServiceId;
        //        var Phys = await Mediator.Send(new GetSingleUserQuery
        //        {
        //            Predicate = x => x.Id == general.PhysicianId,
        //            Select = x => new User
        //            {
        //                Id = x.Id,
        //                Name = x.Name
        //            }
        //        });

        //        var skId = general.ServiceKId;
        //        var servk = await Mediator.Send(new GetSingleServiceQuery
        //        {
        //            Predicate = x => x.Id == skId,
        //            Select = x => new Service
        //            {
        //                Id = x.Id,
        //                Name = x.Name
        //            }
        //        });
        //        NameServicesK = servk.Name ?? "-";

        //        var serv = await Mediator.Send(new GetSingleServiceQuery
        //        {
        //            Predicate = x => x.Id == sId,
        //            Select = x => new Service
        //            {
        //                Id = x.Id,
        //                Name = x.Name
        //            }
        //        });
        //        NameServices = serv.Name ?? "-";
        //        if (general.PhysicianId is not null)
        //        {
        //            PhysicianId = general.PhysicianId;
        //            Phy = Phys.Name ?? "-";
        //        }
        //        await InvokeAsync(() =>
        //        {
        //            PanelVisible = false; // Jika diperlukan, panel disembunyikan di sini
        //            StateHasChanged(); // Memastikan bahwa perubahan UI diterapkan
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(
        //          "\n" +
        //          "==================== START ERROR ====================" + "\n" +
        //          "Message: " + ex.Message + "\n" +
        //          "Inner Message: " + ex.InnerException?.Message + "\n" +
        //          "Stack Trace: " + ex.StackTrace + "\n" +
        //          "==================== END ERROR ====================" + "\n");
        //    }
        //}

        #endregion Async Data

        #region Grid Configuration

        private void Grid_CustomizeElement(GridCustomizeElementEventArgs e)
        {
            if (e.ElementType == GridElementType.DataRow && e.VisibleIndex % 2 == 1)
            {
                e.CssClass = "alt-item";
            }
            else if (e.ElementType == GridElementType.HeaderCell)
            {
                e.Style = "background-color: rgba(0, 0, 0, 0.08)";
                e.CssClass = "header-bold";
            }
        }

        private async Task Refresh_Click()
        {
            await LoadData();
        }

        public MarkupString GetIssueStageIconHtml(string status)
        {
            string priorityClass;
            string title;

            switch (status)
            {
                case "call":
                    priorityClass = "info";
                    title = "Call";
                    break;

                case "present":
                    priorityClass = "success";
                    title = "Present";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        public MarkupString GetClassTypeIconHtml(string? classType)
        {
            string priorityClass;
            string title;

            switch (classType)
            {
                case "VVIP":
                    priorityClass = "danger";
                    title = "VVIP";
                    break;

                case "VIP":
                    priorityClass = "danger";
                    title = "VIP";
                    break;

                default:
                    return new MarkupString("");
            }

            string html = $"<div class='row '><div class='col-3'>" +
                          $"<span class='badge bg-{priorityClass} py-1 px-3' title='{title}'>{title}</span></div></div>";

            return new MarkupString(html);
        }

        #endregion Grid Configuration

        #region Function Button

        private async Task Click_Call(KioskQueueDto context)
        {
            try
            {
                var z = context;

                context.QueueStage = "call";
                context.QueueStatus = "calling";

                if (context.Id != 0)
                {
                    var dataQueue = await Mediator.Send(new UpdateKioskQueueRequest(context));

                    DetailQueueDisplayDto displayDet = new();
                    displayDet.KioskQueueId = context.Id;
                    displayDet.ServiceId = context.ServiceId;
                    displayDet.ServicekId = context.ServiceKId;
                    displayDet.NumberQueue = context.QueueNumber;
                    var data = await Mediator.Send(new CreateDetailQueueDisplayRequest(displayDet));

                    //await hubConnection.SendAsync("CallPatient", CounterId, context.Id);
                    await PlayAudioCall(data.NumberQueue ?? 0);
                }
                var cek = CounterId;
                //DataKiosksQueue = FormKiosksQueue;

                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task PlayAudioCall(long queueNumber)
        {
            var queueText = ConvertNumberToIndonesianWords(queueNumber);
            // Menggunakan System.Speech.Synthesis untuk Text-to-Speech (TTS)
            var utterancet = new SpeechSynthesisUtterance
            {
                Text = $"Nomor Antrian {queueText}",
                Lang = "id-ID", // BCP 47 language tag
                Pitch = 1.0, // 0.0 ~ 2.0 (Default 1.0)
                Rate = 0.6, // 0.1 ~ 10.0 (Default 1.0)
                Volume = 1.0 // 0.0 ~ 1.0 (Default 1.0)
            };
            await this.SpeechSynthesis.SpeakAsync(utterancet);
        }

        private async Task Click_Finish()
        {
            try
            {
                if (FormCounters.Id != 0)
                {
                    FormCounters.QueueStage = "finish";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowSuccess("Data Success!");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Click_Next()
        {
            try
            {
                var data = await Mediator.Send(new GetKioskQueueQuery());

                if (FormCounters.Id != 0)
                {
                    if (FormCounters.ClassTypeId != null)
                    {
                        var TodayQueu = data.Where(x => x.ServiceId == FormCounters.ServiceId && x.ServiceKId == FormCounters.ServiceKId && x.ClassTypeId == FormCounters.ClassTypeId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();
                        if (TodayQueu.Count == 0)
                        {
                            FormCounters.QueueNumber = 1;
                        }
                        else
                        {
                            var GetNoQueue = TodayQueu.OrderByDescending(x => x.QueueNumber).FirstOrDefault();
                            if (GetNoQueue.QueueNumber < 9)
                            {
                                FormCounters.QueueNumber = 1 * (long)GetNoQueue.QueueNumber + 2;
                            }
                            else
                            {
                                ToastService.ShowError("Penuh Eyy!!");
                            }
                        }
                    }
                    else
                    {
                        //Mendapatkan data berdasarkan Counter, service dan tanggal hari ini
                        var TodayQueu = data.Where(x => x.ServiceId == FormCounters.ServiceId && x.ServiceKId == FormCounters.ServiceKId && x.CreatedDate.Value.Date == DateTime.Now.Date).ToList();

                        if (TodayQueu.Count == 0)
                        {
                            FormCounters.QueueNumber = 2;
                        }
                        else
                        {
                            var GetNoQueue = TodayQueu.OrderByDescending(x => x.QueueNumber).FirstOrDefault();
                            if (GetNoQueue.QueueNumber < 10)
                            {
                                FormCounters.QueueNumber = 1 * ((long)GetNoQueue.QueueNumber + 2);
                            }
                            else
                            {
                                FormCounters.QueueNumber = 1 * ((long)GetNoQueue.QueueNumber + 1);
                            }
                        }
                    }
                    FormCounters.QueueStage = null;
                    FormCounters.QueueStatus = "waiting";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                    var datas = FormCounters;
                }
                ShowPresent = false;
                ToastService.ShowInfo("Patient Successfully Diverted");
                await LoadData();
            }
            catch (Exception ex)
            {
                ex.HandleException(ToastService);
            }
        }

        private async Task Click_Present(long Id)
        {
            try
            {
                ShowPresent = true;
                FormCounters = DataKiosksQueue.Where(x => x.Id == Id).FirstOrDefault();
                if (FormCounters.Id != 0)
                {
                    FormCounters.QueueStage = "present";
                    FormCounters.QueueStatus = "on process";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowSuccess("Data Success!");
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private async Task Click_Absent(long Id)
        {
            try
            {
                FormCounters = DataKiosksQueue.Where(x => x.Id == Id).FirstOrDefault();
                if (FormCounters.Id != 0)
                {
                    FormCounters.QueueStage = "absent";
                    FormCounters.QueueStatus = "cancel";
                    await Mediator.Send(new UpdateKioskQueueRequest(FormCounters));
                }
                ToastService.ShowError("Patient Absent!!");
                await LoadData();
            }
            catch (Exception ex)
            {
                ToastService.ShowError(ex.Message);
            }
        }

        private void CloseDetail()
        {
            NavigationManager.NavigateTo("queue/queue-counters");
        }

        #endregion Function Button

        public string ConvertNumberToIndonesianWords(long number)
        {
            string[] ones = { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan" };
            string[] tens = { "", "Sepuluh", "Dua Puluh", "Tiga Puluh", "Empat Puluh", "Lima Puluh", "Enam Puluh", "Tujuh Puluh", "Delapan Puluh", "Sembilan Puluh" };
            string[] thousands = { "", "Ribu", "Juta", "Miliar", "Triliun" };

            if (number == 0)
            {
                return "Nol";
            }

            var result = "";
            var thousandIndex = 0;

            while (number > 0)
            {
                var chunk = number % 1000;
                if (chunk > 0)
                {
                    result = ConvertHundredsToWords(chunk) + " " + thousands[thousandIndex] + " " + result;
                }
                number /= 1000;
                thousandIndex++;
            }

            return result.Trim();
        }

        private string ConvertHundredsToWords(long number)
        {
            string[] ones = { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan" };
            string[] tens = { "", "Sepuluh", "Dua Puluh", "Tiga Puluh", "Empat Puluh", "Lima Puluh", "Enam Puluh", "Tujuh Puluh", "Delapan Puluh", "Sembilan Puluh" };
            string[] teens = { "Sepuluh", "Sebelas", "Dua Belas", "Tiga Belas", "Empat Belas", "Lima Belas", "Enam Belas", "Tujuh Belas", "Delapan Belas", "Sembilan Belas" };

            string words = "";

            if (number >= 100)
            {
                words += ones[number / 100] + " Ratus ";
                number %= 100;
            }

            if (number >= 20)
            {
                words += tens[number / 10] + " ";
                number %= 10;
            }

            if (number >= 10)
            {
                words += teens[number - 10] + " ";
            }
            else if (number > 0)
            {
                words += ones[number] + " ";
            }

            return words.Trim();
        }
    }
}