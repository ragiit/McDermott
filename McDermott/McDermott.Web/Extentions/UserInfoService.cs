using MediatR;

namespace McDermott.Web.Extentions
{
    public class UserInfoService(NavigationManager navigationManager, IJSRuntime oLocal, IMediator mediator)
    {
        private readonly NavigationManager _navigationManager = navigationManager;
        private readonly IJSRuntime _oLocal = oLocal; // Ganti YourLocalService dengan service yang sesuai
        private readonly IMediator _mediator = mediator; // Ganti IMediator dengan interface Mediator yang sesuai

        public async Task<(bool, GroupMenuDto, User)> GetUserInfo(IToastService? toastService = null)
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

                //var groups = await _mediator.Send(new GetGroupMenuQuery(x => x.GroupId == (long)user!.GroupId!)!);
                var tempUser = await _mediator.Send(new GetUserQuery2(x => x.Id == user.Id, 0, 1));
                user = tempUser.Item1.FirstOrDefault().Adapt<User>();
                var result2 = await _mediator.Send(new GetGroupMenuQuery(x => x.GroupId == (long)user!.GroupId!, pageIndex: 0, pageSize: short.MaxValue));
                var groups = result2.Item1;

                var asdf = "queue/kiosk/1".Contains("queue/kiosk");

                url = url.ToLower().Replace(_navigationManager.BaseUri, "");

                var userAccessCRUID = groups?.FirstOrDefault(x => x.Menu?.Url != null && url.Contains(x.Menu.Url.ToLower()));

                if (!string.IsNullOrWhiteSpace(url) && userAccessCRUID is null && url != _navigationManager.BaseUri)
                {
                    toastService?.ClearErrorToasts();
                    toastService?.ShowError("Unauthorized Access\r\n\r\nYou are not authorized to view this page. If you need access, please contact the administrator.\r\n");
                    _navigationManager.NavigateTo("/unauthorized", true);
                    //return (false, null!, null!);
                }

                isAccess = true;

                var User = user as User;

                return (isAccess, userAccessCRUID!, User);
            }
            catch (JSDisconnectedException)
            {
                _navigationManager.NavigateTo("", true);
                return (false, null!, null!);
            }
        }
    }
}