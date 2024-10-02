using DocumentFormat.OpenXml.Spreadsheet;
using MailKit.Search;
using McDermott.Application.Features.Services;
using Microsoft.AspNetCore.Components.Routing;
using OfficeOpenXml.Style;

namespace McDermott.Web.Components.Layout
{
    public partial class MainLayout
    {
        [Inject]
        public CustomAuthenticationStateProvider CustomAuth { get; set; }

        private List<MenuDto> HeaderMenuDtos = [];
        private List<MenuDto> DetailMenuDtos = [];
        private User? User = new();

        private bool showPreloader = true;

        private async Task Top()
        {
            await JsRuntime.InvokeVoidAsync("scrollToTop");
        }

        private async Task ScrollToTop()
        {
            await JsRuntime.InvokeVoidAsync("scrollToTop");
        }

        private async Task OnClickLogout()
        {
            await JsRuntime.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);

            var a = (CustomAuthenticationStateProvider)CustomAuth;
            await a.UpdateAuthState(string.Empty);

            NavigationManager.NavigateTo("login", true);
        }

        private string searchQuery = "";

        private void Search()
        {
            // No action required here since search is handled in the foreach loop
            StateHasChanged(); // Refresh UI after search
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    //await JsRuntime.InvokeVoidAsync("initializePushMenu");
                    showPreloader = false;
                    StateHasChanged();
                    //await JsRuntime.InvokeVoidAsync("scrollFunction");
                    await LoadUser();
                    //showPreloader = false;
                    StateHasChanged();
                }

                //var demoPageUrl = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path).Replace(NavigationManager.BaseUri, "");
            }
            catch { }
        }

        private List<ParentMenuTemp> ParentMenus { get; set; } = [];

        private class ParentMenuTemp
        {
            public long? ParentId { get; set; }
            public string? ParentName { get; set; }
            public string? Icon { get; set; }
            public long? Sequence { get; set; }
        }

        [Inject]
        private IJSRuntime _oLocal { get; set; }

        private async Task LoadUser()
        {
            try
            {
                var userJson = await CookieHelper.GetCookie(JsRuntime, CookieHelper.USER_INFO);

                User = JsonConvert.DeserializeObject<User>(userJson);
                _isInitComplete = true;

                StateHasChanged();

                if (User is null)
                    return;

                if (User.GroupId is null)
                {
                    showPreloader = false;
                    return;
                }

                var menus = (await Mediator.QueryGetHelper<Menu, MenuDto>(0, short.MaxValue)).Item1;
                var groups = (await Mediator.QueryGetHelper<GroupMenu, GroupMenuDto>(0, short.MaxValue, predicate: x => x.GroupId == (long)User!.GroupId!)).Item1;

                var groupMenuIds = groups.Select(x => x.MenuId).ToList();
                var parentMenuIds = menus.Where(x => groups.Select(z => z.MenuId).Contains(x.Id)).Select(x => x.ParentId).Distinct().ToList();
                //var childParentIds = groups.Where(x => menus.Any(m => m.Id == x.MenuId)).Select(x  => x.Menu.).Distinct().ToList();

                ParentMenus = [.. menus
                    .Where(x => x.ParentId == null && parentMenuIds.Contains(x.Id))
                    .Select(x => new ParentMenuTemp
                    {
                        ParentId = x.Id,
                        ParentName = x.Name,
                        Sequence = x.Sequence,
                        Icon = x.Icon
                    })
                    .DistinctBy(p => new { p.ParentId, p.ParentName, p.Sequence })
                    .OrderBy(x => x.Sequence)];

                //HeaderMenuDtos = menus
                //    .Where(x => x.ParentId == null && groupMenuIds.Contains(x.Id) && !x.Name.Equals("Template Page"))
                //    .OrderBy(x => x.Sequence.ToInt32())
                //    .ToList();

                DetailMenuDtos = [.. menus
                    .Where(x => x.ParentId != null && groupMenuIds.Contains(x.Id))
                    .OrderBy(x => x.Sequence.ToInt32())];
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
                showPreloader = true;
                currentUrl = NavigationManager.Uri;
            }
            catch (Exception)
            {
            }
        }
    }
}