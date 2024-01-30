using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace McDermott.Web.Authentication
{
    public class CustomAuthenticationStateProvider
    {
        //private readonly ProtectedSessionStorage _sessionStorage;
        //private ClaimsPrincipal _ = new ClaimsPrincipal(new ClaimsIdentity());

        //public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
        //{
        //    _sessionStorage = sessionStorage;
        //}

        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    try
        //    {
        //        var a = await _sessionStorage.GetAsync<string>("us");
        //        var b = a.Success ? a.Value : null;
        //        if (b == null)
        //            return await Task.FromResult(new AuthenticationState(_));

        //        var user = new UserDto();

        //        var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, "argi gege ya gais")
        //        }, "CustomAuth"));

        //        return await Task.FromResult(new AuthenticationState(claimPrincipal));
        //    }
        //    catch (Exception)
        //    {
        //        return await Task.FromResult(new AuthenticationState(_));
        //    }
        //}

        //public async Task UpdateAuthState(User user)
        //{
        //    ClaimsPrincipal claims = new();
        //    if (user is not null)
        //    {
        //        await _sessionStorage.SetAsync("us", user);
        //        var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, "argi gege ya gais")
        //        }, "CustomAuth"));
        //    }
        //    else
        //    {
        //        await _sessionStorage.DeleteAsync("us");
        //        claims = _;
        //    }

        //    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        //}
    }
}