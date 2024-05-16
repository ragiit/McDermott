using Serilog;

namespace McDermott.Web.Extentions
{
    public static class Helper
    {
        public static readonly string VERSION = "2.0.0";

        public static void ShowInfoSubmittingForm(this IToastService toastService, string message = "Please ensure that all fields marked in red are filled in before submitting the form.") => toastService.ShowInfo(message);

        public static string EncodeToBase64(this string input)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
            string base64Encoded = Convert.ToBase64String(bytesToEncode);
            return base64Encoded;
        }
        public static T EnumGetValue<T>(this Enum enumValue)
        {
            return (T)Convert.ChangeType(enumValue, typeof(T));
        }

        public static long RandomNumber => new Random().Next(1, 9000000) + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Second;

        public static string HashMD5(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = MD5.HashData(inputBytes);
            StringBuilder sb = new();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static void HandleException(this Exception ex, IToastService toastService)
        {
            string errorMessage = "An error occurred while saving data.";

            if (ex.InnerException is SqlException sqlException)
            {
                switch (sqlException.Number)
                {
                    case 547:
                        errorMessage = "Data cannot be deleted because it is associated with another entity.";
                        break;
                    // Add more cases as needed for specific SQL error numbers
                    default:
                        errorMessage = "An error occurred in the database, Code: " + sqlException.ErrorCode;
                        break;
                }
            }
            else
            {
                errorMessage = "An error occurred";
            }

            Log.Error(
                  "\n\n" +
                  "==================== START ERROR ====================" + "\n" +
                  "Message =====> " + ex.Message + "\n" +
                  "Inner Message =====> " + ex.InnerException?.Message + "\n" +
                  "Stack Trace =====> " + ex.StackTrace?.Trim() + "\n" +
                  "==================== END ERROR ====================" + "\n"
                  );

            toastService.ClearErrorToasts();
            toastService.ShowError(errorMessage);
        }

        public static User? UserLogin { get; set; }

        public static string HashWithSHA256(string data)
        {
            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(data));
            return Convert.ToBase64String(hashedBytes);
        }

        public static string Encrypt(string plainText, string key = "mysmallkey123456")
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16]; // IV harus unik, tetapi tidak perlu rahasia

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            MemoryStream memoryStream = new();
            using MemoryStream msEncrypt = memoryStream;
            using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter swEncrypt = new(csEncrypt);
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
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

        //public static async Task<(bool, GroupMenuDto)> CheckAccessUser(this NavigationManager NavigationManager, ILocalStorageService oLocal)
        //{
        //    try
        //    {
        //        dynamic user = await oLocal.GetItemAsync<string>("dotnet");
        //        var menu = await oLocal.GetItemAsync<string>("dotnet2");

        //        if (string.IsNullOrWhiteSpace(user) || string.IsNullOrEmpty(menu))
        //        {
        //            await oLocal.ClearAsync();

        //            NavigationManager.NavigateTo("login", true);

        //            return (false, new());
        //        }

        //        menu = Decrypt(menu);

        //        var menus = JsonConvert.DeserializeObject<List<GroupMenuDto>>(menu);
        //        var url = NavigationManager.Uri;

        //        var z = menus?.Where(x => x.Menu.Url != null && x.Menu.Url.Contains(url.Replace(NavigationManager.BaseUri, ""))).FirstOrDefault();

        //        if (z is null && url != NavigationManager.BaseUri)
        //        {
        //            NavigationManager.NavigateTo("", true);
        //            return (false, new());
        //        }

        //        return (true, z!);
        //    }
        //    catch (JSDisconnectedException)
        //    {
        //        return (false, new());
        //    }
        //}

        public static async Task<(bool, GroupMenuDto)> CheckAccessUser(this NavigationManager NavigationManager, IJSRuntime oLocal)
        {
            try
            {
                dynamic user = await oLocal.GetCookie(CookieHelper.USER_INFO);
                //var menu = await oLocal.GetCookie(CookieHelper.USER_GROUP);

                if (string.IsNullOrWhiteSpace(user) /*|| string.IsNullOrEmpty(menu)*/)
                {
                    await oLocal.InvokeVoidAsync("clearAllCookies");
                    NavigationManager.NavigateTo("login", true);

                    return (false, new());
                }

                //menu = Decrypt(menu);

                //var menus = JsonConvert.DeserializeObject<List<GroupMenuDto>>(menu);
                //var url = NavigationManager.Uri;

                //var z = menus?.Where(x => x.Menu.Url != null && x.Menu.Url.Contains(url.Replace(NavigationManager.BaseUri, ""))).FirstOrDefault();

                //if (z is null && url != NavigationManager.BaseUri)
                //{
                //    NavigationManager.NavigateTo("", true);
                //    return (false, new());
                //}

                return (true, null!);
            }
            catch (JSDisconnectedException)
            {
                return (false, new());
            }
        }

        // udah ga dipakai lagi
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

        // udah ga dipakai lagi
        public static void CustomNavigateToPage(this NavigationManager navigationManager, string page, long? id = null)
        {
            var action = id is null ? "add" : "edit";
            string url = $"{page}/{action}";

            if (id >= 0)
                url += $"/{id}";

            navigationManager.NavigateTo(url);
        }

        public static int ToInt32(this object o) => Convert.ToInt32(o);
        public static long ToLong(this object o) => Convert.ToInt64(o);

        public static async Task DownloadFile(string file, IHttpContextAccessor HttpContextAccessor, HttpClient Http, IJSRuntime JsRuntime)
        {
            try
            {
                var currentProtocol = HttpContextAccessor.HttpContext.Request.Scheme;

                var currentHost = HttpContextAccessor.HttpContext.Request.Host.Value;
                var baseUrl = $"{currentProtocol}://{currentHost}";

                var apiUrl = $"{baseUrl}/files/{file}";
                var response = await Http.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();

                    using (var ms = new MemoryStream())
                    {
                        await content.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        var base64Content = Convert.ToBase64String(bytes);

                        await JsRuntime.InvokeVoidAsync("downloadFile", new
                        {
                            fileName = file,
                            content = base64Content
                        });
                    }
                }
                else
                {
                    // Handle error
                    // You might want to show an error message or log the error
                }
            }
            catch (Exception)
            {
                // Handle exception
                // You might want to show an error message or log the exception
            }
        }

        public static void DeleteFile(string filePath)
        {
            try
            {
                // Path.Combine untuk membuat path lengkap ke file di dalam wwwroot
                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files");
                string fullPath = Path.Combine(wwwRootPath, filePath);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    Console.WriteLine($"File {filePath} berhasil dihapus.");
                }
                else
                {
                    Console.WriteLine($"File {filePath} tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menghapus file: {ex.Message}");
            }
        }
    }
}