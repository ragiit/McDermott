using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace McDermott.Web.Extentions
{
    public class GoogleMeetService
    {
        private readonly string[] Scopes = { CalendarService.Scope.Calendar };
        private readonly string ApplicationName = "Blazor Web App Client";

        public async Task<string> CreateMeetingAsync()
        {
            UserCredential credential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define the parameters of the new event.
            Event newEvent = new Event()
            {
                Summary = "Google Meet Meeting",
                Location = "Online",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Now.AddHours(1),
                    TimeZone = "America/Los_Angeles",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Now.AddHours(2),
                    TimeZone = "America/Los_Angeles",
                },
                Attendees = new EventAttendee[] {
                new EventAttendee() { Email = "attendee@example.com" },
            },
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        }
                    }
                },
            };

            // Insert the event into the calendar
            EventsResource.InsertRequest request = service.Events.Insert(newEvent, "primary");
            request.ConferenceDataVersion = 1;
            Event createdEvent = await request.ExecuteAsync();
            var hehe = createdEvent.HangoutLink;

            return hehe;
        }
    }
}