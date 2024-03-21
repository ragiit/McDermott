using MediatR;

namespace McDermott.Web.Extentions
{
    public class UserInfoService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _oLocal; // Ganti YourLocalService dengan service yang sesuai
        private readonly IMediator _mediator; // Ganti IMediator dengan interface Mediator yang sesuai

        public UserInfoService(NavigationManager navigationManager, IJSRuntime oLocal, IMediator mediator)
        {
            _navigationManager = navigationManager;
            _oLocal = oLocal;
            _mediator = mediator;
        }

        public async Task<(bool, GroupMenuDto, User)> GetUserInfo()
        {
            try
            {
                bool isAccess = false;
                var url = _navigationManager.Uri;

                User user = await _oLocal.GetCookieUserLogin();

                if (user is null)
                {
                    await _oLocal.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);
                    _navigationManager.NavigateTo("login", true);
                    return (false, null!, null!);
                }


                var groups = await _mediator.Send(new GetGroupMenuQuery(x => x.GroupId == (long)user!.GroupId!)!);

                var asdf = "queue/kiosk/1".Contains("queue/kiosk");

                url = url.ToLower().Replace(_navigationManager.BaseUri, "");

                var userAccessCRUID = groups?.FirstOrDefault(x => x.Menu?.Url != null && url.Contains(x.Menu.Url.ToLower()));

                if (userAccessCRUID is null && url != _navigationManager.BaseUri)
                {
                    _navigationManager.NavigateTo("", true);
                }

                isAccess = true;

                var User = user as User;

                return (isAccess, userAccessCRUID!, User);
            }
            catch (JSDisconnectedException)
            {
                // Handle JSDisconnectedException if needed
                return (false, null!, null!);
            }
        }
    }
}