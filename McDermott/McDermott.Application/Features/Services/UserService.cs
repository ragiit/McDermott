using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace McDermott.Application.Features.Services
{
    public class UserService : IUserService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IJSRuntime jsRuntime, IHttpContextAccessor httpContextAccessor)
        {
            _jsRuntime = jsRuntime;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClaimValue(string claimType)
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var claim = user.FindFirst(claimType);
                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return null;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            try
            {
                GetClaimValue("asd");
                var data = await _jsRuntime.InvokeAsync<string>("blazorLocalStorage.getItem", "dotnet");
                if (!string.IsNullOrEmpty(data))
                {
                    data = Decrypt(data);
                    return JsonConvert.DeserializeObject<User>(data)!;
                }

                return new User();
            }
            catch (InvalidOperationException)
            {
                return null;
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
    }
}