using Microsoft.Identity.Client;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace McDermott.Web.Extentions
{
    public class TeamsMeetingService
    {
        private readonly string _clientId = "YOUR_CLIENT_ID";
        private readonly string _tenantId = "YOUR_TENANT_ID";
        private readonly string _clientSecret = "YOUR_CLIENT_SECRET";

        private async Task<string> GetAccessTokenAsync()
        {
            var app = ConfidentialClientApplicationBuilder.Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{_tenantId}"))
                .Build();

            var scopes = new string[] { "https://graph.microsoft.com/.default" };
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;
        }

        //public async Task<string> CreateTeamsMeetingAsync()
        //{
        //    var accessToken = await GetAccessTokenAsync();

        //    var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
        //    {
        //        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        //    }));

        //    var newEvent = new Event
        //    {
        //        Subject = "Teams Meeting",
        //        Start = new DateTimeTimeZone
        //        {
        //            DateTime = DateTime.UtcNow.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),
        //            TimeZone = "UTC"
        //        },
        //        End = new DateTimeTimeZone
        //        {
        //            DateTime = DateTime.UtcNow.AddHours(2).ToString("yyyy-MM-ddTHH:mm:ss"),
        //            TimeZone = "UTC"
        //        },
        //        Location = new Location
        //        {
        //            DisplayName = "Online"
        //        },
        //        OnlineMeeting = new OnlineMeeting
        //        {
        //            Create = true
        //        }
        //    };

        //    var createdEvent = await graphClient.Me.Events
        //        .Request()
        //        .AddAsync(newEvent);

        //    return createdEvent.OnlineMeeting.JoinUrl;
        //}
    }
}