using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace McDermott.Application.Features.Services
{
    public class CustomAuthenticationStateProvider(ILocalStorageService sessionStorage, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, ProtectedSessionStorage protectedSession) : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _sessionStorage = sessionStorage;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMemoryCache _cache = memoryCache;
        private ClaimsPrincipal _ = new(new ClaimsIdentity());
        private ProtectedSessionStorage _protectedSessionStorage = protectedSession;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                //string cacheKey = "USER_INFO";

                //if (_cache.TryGetValue(cacheKey, out dynamic result))
                //{
                //    if (result is not null && !string.IsNullOrWhiteSpace(result))
                //    {
                //        var user = JsonConvert.DeserializeObject<User>(Decrypt(result));

                //        var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                //        [
                //            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //    new Claim(ClaimTypes.Name, user.Name)
                //        ], "CustomAuth"));

                //        _httpContextAccessor.HttpContext.User = claimPrincipal;

                //        return await Task.FromResult(new AuthenticationState(claimPrincipal));
                //    }
                //    return await Task.FromResult(new AuthenticationState(_));
                //}
                //else
                //    return await Task.FromResult(new AuthenticationState(_));

                var result = await _protectedSessionStorage.GetAsync<string>("USER_INFO");
                var userSession = result.Success ? (Decrypt(result.Value)) : null;
                if (userSession is null)
                    return await Task.FromResult(new AuthenticationState(_));

                var usr = JsonConvert.DeserializeObject<User>(userSession);

                var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()),
                    new Claim(ClaimTypes.Name, usr.Name)
                ], "CustomAuth"));

                _httpContextAccessor.HttpContext.User = claimPrincipal;

                return await Task.FromResult(new AuthenticationState(claimPrincipal));
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(_));
            }
        }

        public string Decrypt(string cipherText, string key = "mysmallkey123456")
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // IV harus sama dengan yang digunakan saat enkripsi

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public async Task UpdateAuthState(string user)
        {
            ClaimsPrincipal claims = new();

            if (!string.IsNullOrWhiteSpace(user))
            {
                await _protectedSessionStorage.SetAsync("USER_INFO", user);

                var usr = JsonConvert.DeserializeObject<User>(Decrypt(user));

                var authenticationProperties = new AuthenticationProperties
                {
                    AllowRefresh = true, // Sesuaikan sesuai kebutuhan
                    IsPersistent = true, // Sesuaikan sesuai kebutuhan
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Sesuaikan sesuai kebutuhan
                };

                claims = new ClaimsPrincipal(new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()),
                     new Claim(ClaimTypes.Name, usr.Name)
                ], "CustomAuth"));

                // Menentukan skema autentikasi di sini
                _httpContextAccessor.HttpContext.User = claims;
            }
            else
            {
                await _protectedSessionStorage.DeleteAsync("USER_INFO");
                claims = _;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));

            //if (user is not null)
            //{
            //    var User = JsonConvert.DeserializeObject<User>(Decrypt(user));

            //    claims = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, User.Id.ToString()),
            //        new Claim(ClaimTypes.Name, User.Name)
            //    }, "CustomAuth"));

            //    _httpContextAccessor.HttpContext.User = claims;

            //    _cache.Set("USER_INFO", user, TimeSpan.FromDays(1));
            //    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
            //}
            //else
            //{
            //    //await _sessionStorage.DeleteAsync("us");
            //    claims = _;
            //    _cache.Remove("USER_INFO");
            //    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
            //}
        }
    }
}