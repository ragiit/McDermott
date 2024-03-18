using static McDermott.Application.Features.Commands.Config.GroupMenuCommand;

namespace McDermott.Web.Components.Layout
{
    public partial class BaseLayoutRazor
    { // Properti yang dapat di-override oleh halaman pengguna
        protected bool IsUserAccessPage { get; set; } = false; // Nilai default adalah kembali ke halaman utama

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);

                if (firstRender)
                {
                    await oLocal.GetCookieUserLogin();
                }
            }
            catch (Exception)
            {
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var url = NavigationManager.Uri;

                User user = await oLocal.GetCookieUserLogin();

                var groups = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == (long)user!.GroupId!));

                var z = groups?.Where(x => x.Menu?.Url != null && x.Menu.Url.ToLower().Contains(url.ToLower().Replace(NavigationManager.BaseUri, ""))).FirstOrDefault();

                if (z is null && url != NavigationManager.BaseUri)
                {
                    NavigationManager.NavigateTo("", true);
                }

                if (user is null)
                {
                    await oLocal.InvokeVoidAsync("clearAllCookies");
                    NavigationManager.NavigateTo("login", true);
                }

                IsUserAccessPage = true;
            }
            catch (JSDisconnectedException)
            {
            }
        }
    }
}