using Blazored.LocalStorage;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace McDermott.Web
{
    public static class Helper
    {
        public static User? UserLogin { get; set; }

        public static string HashWithSHA256(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static string Encrypt(string plainText, string key = "mysmallkey123456")
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = new byte[16]; // IV harus unik, tetapi tidak perlu rahasia

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string key = "mysmallkey123456")
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

        public class EnumEditModeData
        {
            // Specifies an enumeration value
            public GridEditMode Value { get; set; }

            // Specifies text to display in the ComboBox
            public string DisplayName { get; set; }
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
        //public class DateInPastAttribute : ValidationAttribute
        //{
        //    public override bool IsValid(object value)
        //    {
        //        return (DateTime)value <= DateTime.Today;
        //    }
        //}

        public static async Task<bool> CheckAccessUser(this NavigationManager NavigationManager, ILocalStorageService oLocal)
        {
            try
            {
                dynamic user = await oLocal.GetItemAsync<string>("dotnet");
                var menu = await oLocal.GetItemAsync<string>("dotnet2");

                if (string.IsNullOrWhiteSpace(user) || string.IsNullOrEmpty(menu))
                {
                    await oLocal.ClearAsync();

                    NavigationManager.NavigateTo("login", true);

                    return false;
                }

                menu = Helper.Decrypt(menu);

                var menus = JsonConvert.DeserializeObject<List<GroupMenuDto>>(menu);
                var url = NavigationManager.Uri;

                var z = menus?.Where(x => x.Menu.Url != null && x.Menu.Url.Contains(url.Replace(NavigationManager.BaseUri, ""))).FirstOrDefault();

                if (z is null && !url.Contains("home"))
                {
                    NavigationManager.NavigateTo("home", true);
                    return false;
                }

                return true;
            }
            catch (JSDisconnectedException ex)
            {
                return false;
            }
        }

        public static async Task<User> GetUserInfo(this ILocalStorageService o)
        {
            var data = await o.GetItemAsync<string>("dotnet");
            if (!string.IsNullOrEmpty(data))
            {
                data = Decrypt(data);
                return JsonConvert.DeserializeObject<User>(data)!;
            }

            return new User();
        }

        //public static async Task<List<MenuDto>> GetUserMenuInfo(this ILocalStorageService o)
        //{
        //    return await o.GetItemAsync<List<MenuDto>>("Menu");
        //}

        public static void CustomNavigateToPage(this NavigationManager navigationManager, string page, int? id = 0)
        {
            var action = id is null ? "add" : "edit";
            string url = $"{page}/{action}";

            if (id >= 0)
                url += $"/{id}";

            navigationManager.NavigateTo(url);
        }

        public static int ToInt32(this object o) => Convert.ToInt32(o);
    }
}