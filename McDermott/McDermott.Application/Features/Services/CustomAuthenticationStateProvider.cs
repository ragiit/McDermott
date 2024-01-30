using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Application.Features.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _sessionStorage;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal _ = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ILocalStorageService sessionStorage, IHttpContextAccessor httpContextAccessor)
        {
            _sessionStorage = sessionStorage;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var a = await _sessionStorage.GetItemAsync<string>("dotnet");
                if (a == null)
                    return await Task.FromResult(new AuthenticationState(_));

                var user = JsonConvert.DeserializeObject<User>(Decrypt(a));

                var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name)
                }, "CustomAuth"));

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

        public async Task UpdateAuthState(User user)
        {
            ClaimsPrincipal claims = new();
            if (user is not null)
            {
                //await _sessionStorage.SetItemAsync("us", user);
                claims = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name)
                }, "CustomAuth"));
            }
            else
            {
                //await _sessionStorage.DeleteAsync("us");
                claims = _;
            }
            _httpContextAccessor.HttpContext.User = claims;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claims)));
        }
    }
}