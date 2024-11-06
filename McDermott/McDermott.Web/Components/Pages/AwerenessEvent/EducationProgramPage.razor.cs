using McDermott.Application.Dtos.AwarenessEvent;

namespace McDermott.Web.Components.Pages.AwerenessEvent
{
    public partial class EducationProgramPage
    {
        #region Data Relation
        private List<EducationProgramDto> getEducationPrograms = [];
        private EducationProgramDto postEducationPrograms = new();
        #endregion
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

    }
}
