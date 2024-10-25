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

                User cookieUser = await _oLocal.GetCookieUserLogin();

                if (cookieUser is null)
                {
                    await _oLocal.InvokeVoidAsync("deleteCookie", CookieHelper.USER_INFO);
                    _navigationManager.NavigateTo("login", true);
                    return (false, null!, null!);
                }

                //var groups = await _mediator.Send(new GetGroupMenuQuery(x => x.GroupId == (long)user!.GroupId!)!);

                //var user = (await _mediator.Send(new GetUserQuery2(predicate: x => x.Id == cookieUser.Id, 0, short.MaxValue, includes: [],
                //    select: x => new User
                //    {
                //        Id = x.Id,
                //        Name = x.Name,
                //        GroupId = x.GroupId
                //    }))).Item1.FirstOrDefault() ?? new();

                var user = await _mediator.Send(new GetSingleUserQuery
                {
                    Predicate = x => x.Id == cookieUser.Id,
                    Select = x => new User
                    {
                        Id = x.Id,
                        Name = x.Name,
                        GroupId = x.GroupId,
                        IsAdmin = x.IsAdmin,
                        IsDefaultData = x.IsDefaultData,
                        IsDoctor = x.IsDoctor,
                        IsEmployee = x.IsEmployee,
                        IsHr    = x.IsHr,
                        IsNurse = x.IsNurse,
                        IsPatient = x.IsPatient,
                        IsPhysicion = x.IsPhysicion,
                        IsPharmacy = x.IsPharmacy,
                        IsUser = x.IsUser,
                        IsMcu = x.IsMcu, 
                    }
                });

                var groups = (await _mediator.QueryGetHelper<GroupMenu, GroupMenuDto>(0, short.MaxValue, predicate: x => x.GroupId == (long)user!.GroupId!,
                    includes:
                    [
                        x => x.Menu
                    ],
                    select: x => new GroupMenu
                    {
                        Id = x.Id,
                        GroupId = x.GroupId,
                        IsCreate = x.IsCreate,
                        IsDelete = x.IsDelete,
                        IsDefaultData = x.IsDefaultData,
                        IsImport = x.IsImport,
                        IsRead = x.IsRead,
                        IsUpdate = x.IsUpdate,
                        Menu = new Menu
                        {
                            Url = x.Menu.Url
                        }
                    })).Item1;

                url = url.ToLower().Replace(_navigationManager.BaseUri, "");

                var userAccessCRUID = groups?.FirstOrDefault(x => x.Menu?.Url != null && url.Contains(x.Menu.Url.ToLower()));

                if (!string.IsNullOrWhiteSpace(url) &&
                    userAccessCRUID is null &&
                    url != _navigationManager.BaseUri &&
                    !url.Contains("queue/kiosk/") &&
                    !url.Contains("transaction/print-document-medical")
                    )
                {
                    _navigationManager.NavigateTo("/unauthorized", true);
                    toastService?.ClearErrorToasts();
                    toastService?.ShowError("Unauthorized Access\r\n\r\nYou are not authorized to view this page. If you need access, please contact the administrator.\r\n");
                    //return (false, null!, null!);
                }

                isAccess = true;

                var User = user.Adapt<User>();

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