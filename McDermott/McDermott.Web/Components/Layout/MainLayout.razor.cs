using McDermott.Domain.Entities;
using Newtonsoft.Json;

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
            await oLocal.ClearAsync();

            NavigationManager.NavigateTo("/login", true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadUser();
                StateHasChanged();
            }
        }

        private async Task LoadUser()
        {
            try
            {
                User = await oLocal.GetUserInfo();
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
        protected override async Task OnInitializedAsync()
        {
            try
            {
                currentUrl = NavigationManager.Uri;

                await LoadUser();

                var user = await oLocal.GetUserInfo();

                if (user is null)
                    return;

                if (user.GroupId is null)
                {
                    showPreloader = false;
                    return;
                }

                var menus = await Mediator.Send(new GetMenuQuery());
                var groups = await Mediator.Send(new GetGroupMenuByGroupIdRequest((int)user!.GroupId!)!);

                var ids = groups.Select(x => x.MenuId).ToList();

                HeaderMenuDtos = [.. menus.Where(x => x.ParentMenu == null && ids.Contains(x.Id)).OrderBy(x => x.Sequence.ToInt32())];
                DetailMenuDtos = [.. menus.Where(x => x.ParentMenu != null && ids.Contains(x.Id)).OrderBy(x => x.Sequence.ToInt32())];

                if (user.GroupId is not null)
                {
                    var g = await Mediator.Send(new GetGroupMenuByGroupIdRequest((int)user.GroupId));

                    var encryptMenu = Helper.Encrypt(JsonConvert.SerializeObject(g));

                    await oLocal.SetItemAsync("dotnet2", encryptMenu);
                }
                else
                {
                    await oLocal.SetItemAsync("dotnet2", new List<string>());
                }

                showPreloader = false;

                StateHasChanged();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }
        }
    }
}