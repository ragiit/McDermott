using Microsoft.AspNetCore.Components.Routing;

namespace McDermott.Web.Components.Layout
{
    public partial class MainLayout
    {
        private List<MenuDto> HeaderMenuDtos = [];
        private List<MenuDto> DetailMenuDtos = [];
        private User? User = new();

        private bool showPreloader = true;

        private async Task OnClickLogout()
        {
            await JsRuntime.InvokeVoidAsync("clearAllCookies");

            NavigationManager.NavigateTo("/login", true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    await LoadUser();
                    StateHasChanged();
                }

                //var demoPageUrl = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path).Replace(NavigationManager.BaseUri, "");
            }
            catch { }
        }

        private async Task LoadUser()
        {
            try
            {
                var userJson = await CookieHelper.GetCookie(JsRuntime, CookieHelper.USER_INFO);
                User = JsonConvert.DeserializeObject<User>(userJson);
                _isInitComplete = true;
            }
            catch { }
        }

        private string currentUrl;
        private bool _isInitComplete = false;

        //protected override void OnInitialized()
        //{
        //    currentUrl = NavigationManager.Uri;
        //}

        //protected override void OnInitialized()
        //{
        //    NavigationManager.LocationChanged += HandleLocationChanged;
        //    UpdateBreadcrumb(NavigationManager.Uri);
        //}

        public class Breadcrumb
        {
            public string Name { get; set; }
            public bool IsActive { get; set; }
        }

        private List<Breadcrumb> Breadcrumbs = [];

        private void HandleLocationChanged(object sender, LocationChangedEventArgs args)
        {
            //UpdateBreadcrumb(args.Location);
        }

        private void UpdateBreadcrumb(string uri)
        {
            string[] segments = uri.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Mendapatkan URL saat ini
            var uriw = new Uri(NavigationManager.Uri);

            // Mendapatkan path dari URL

            Breadcrumbs.Clear();
            foreach (var segment in NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path).Replace(NavigationManager.BaseUri, "").Split("/"))
            {
                var aa = segment;
                //Breadcrumbs.Add(segment);
            }

            var asd = new List<Breadcrumb>();

            var urls = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path).Replace(NavigationManager.BaseUri, "").Split("/");

            string path = uriw.AbsolutePath;

            // Mendapatkan segmen terakhir dari path URL

            var aaa = path.Split('/').Last();

            Breadcrumbs.Clear();

            for (int i = 1; i < urls.Length; i++)
            {
                var name = urls[i];
                Breadcrumbs.Add(new Breadcrumb
                {
                    IsActive = aaa.ToLower() == name.ToLower(),
                    Name = char.ToUpper(name[0]) + name.Substring(1)
                });
            }

            var aaaaa = Breadcrumbs;

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                currentUrl = NavigationManager.Uri;
                await LoadUser();

                var userJson = await CookieHelper.GetCookie(JsRuntime, CookieHelper.USER_INFO);
                var user = JsonConvert.DeserializeObject<User>(userJson);

                if (user is null)
                    return;

                if (user.GroupId is null)
                {
                    showPreloader = false;
                    return;
                }

                var menus = await Mediator.Send(new GetMenuQuery());
                var groups = await Mediator.Send(new GetGroupMenuQuery(x => x.GroupId == (long)user!.GroupId!)!);

                var ids = groups.Select(x => x.MenuId).ToList();

                HeaderMenuDtos = [.. menus.Where(x => x.ParentMenu == null && ids.Contains(x.Id) && !x.Name.Equals("Template Page")).OrderBy(x => x.Sequence.ToInt32())];
                DetailMenuDtos = [.. menus.Where(x => x.ParentMenu != null && ids.Contains(x.Id)).OrderBy(x => x.Sequence.ToInt32())];

                //if (user.GroupId is not null)
                //{
                //    var g = await Mediator.Send(new GetGroupMenuByGroupIdRequest((long)user.GroupId));

                //    var encryptMenu = Helper.Encrypt(JsonConvert.SerializeObject(g));

                //    await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_GROUP, encryptMenu, 30);
                //    // await oLocal.SetItemAsync("dotnet2", encryptMenu);
                //}
                //else
                //{
                //    await JsRuntime.InvokeVoidAsync("setCookie", CookieHelper.USER_GROUP, string.Empty, 30);
                //    // await oLocal.SetItemAsync("dotnet2", new List<string>());
                //}

                showPreloader = false;

                StateHasChanged();
            }
            catch (Exception e)
            {
            }
        }
    }
}