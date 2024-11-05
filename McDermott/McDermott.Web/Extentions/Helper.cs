using DevExpress.Utils.Filtering.Internal;
using McDermott.Application.Dtos;
using McDermott.Application.Features.Services;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components.Web;
using Serilog;
using Path = System.IO.Path;

namespace McDermott.Web.Extentions
{
    public class AppSettings
    {
        public string BaseHref { get; set; }
    }  
    public static class Helper
    {
       

        public static void ShowErrorImport(this IToastService ToastService, int row, int col, string val)
        {
            ToastService.ShowInfo($"Data with name \"{val}\" in row {row} and column {col} is invalid");
        }

        public static void ShowErrorConflictValidation(this IToastService ToastService, string entityName)
        {
            ToastService.ShowInfo($"{entityName} already exists. Please check the details and try again.");
        }

        public static void ShowSuccessCountImported(this IToastService ToastService, int count)
        {
            ToastService.ShowSuccess($"{count} items were successfully imported.");
        }

        public static List<string> BloodTypes = new List<string>
        {
            "A+",
            "A-",
            "B+",
            "B-",
            "AB+",
            "AB-",
            "O+",
            "O-"
        };

        public static List<string> Trimester = new List<string>
        {
            "Trim I",
            "Trim II",
            "Trim III"
        };


        public static List<string> HumptyDumpty =
        [
            "Risiko rendah 0-6",
            "Risiko sedang 7-11",
            "Risiko Tinggi >= 12"
        ];

        public static string DisplayFormat { get; } = "HH:mm";
        public static List<string> Hospitals { get; set; } = new List<string> { "RSBK", "RSE", "RSHB", "RSBP", "RSAB", "RSGH", "RSMA", "RSHBH", "RSSD" };
        public static List<string> ExaminationPurposes { get; set; } = new List<string> { "Dentist", "Internist", "Pulmonologist", "Cardiologist", "Eye", "ENT", "Paediatric", "Surgeon", "Obstetrician", "Neurologist", "Urologist", "Neurosurgeon", "Orthopaedic", "Physiotherapist", "Dermatologist", "Psychiatrist", "Laboratorium" };
        public static List<string> Categories { get; set; } = new List<string> { "KANKER", "ACCIDENT Inside", "EMPLOYEE", "KELAINAN BAWAAN", "ACCIDENT Outside", "DEPENDENT" };
        public static List<string> ExamFor { get; set; } = new List<string> { "Pemeriksaan / penanganan lebih lanjut", "Pembedahan", "Perawatan", "Bersalin" };
        public static List<string> InpatientClasses { get; set; } = new List<string> { "VIP Class", "Class 1 B", "Class 2" };

        public static List<string> InformationFrom =
        [
            "Auto Anamnesa",
            "Allo Anamnesa"
        ];

        public static List<string> YesNoOptions =
       [
           "Yes",
            "No"
       ];

        public static List<string> Morse =
        [
            "Risiko rendah 0-24",
            "Risiko sedang 25-44",
            "Risiko Tinggi >= 45"
        ];

        public static List<string> RiwayatPenyakitKeluarga =
        [
            "DM",
            "Hipertensi",
            "Cancer",
            "Jantung",
            "TBC",
            "Anemia",
            "Other",
        ];

        public static List<string> Geriati =
       [
           "Risiko rendah 0-3",
            "Risiko Tinggi >= 4"
       ];

        public static List<string> RiskOfFalling =
        [
            "Humpty Dumpty",
            "Morse",
            "Geriatri",
        ];

        public static List<string> Colors = new List<string>
        {
            "Red",
            "Yellow",
            "Green",
            "Black",
        };

        public static string EmailMask { get; set; } = @"(\w|[.-])+@(\w|-)+\.(\w|-){2,4}";

        public static readonly List<string> IdentityTypes =
        [
            "KTP",
            "Paspor",
            "SIM",
            "VISA",
        ];

        public static List<string> RegisType = new List<string>
        {
            "Accident"
        };

        public static List<string> ClinicVisitTypes = new List<string>
        {
            "Healthy",
            "Sick"
        };

        public static List<string> Payments = new List<string>
        {
            "Personal",
            "Insurance",
            "BPJS"
        };

        public static readonly List<string> Genders =
        [
            "Male",
            "Female"
        ];

        public static List<string> EmployeeTypes = new()
        {
            "Employee",
            "Pre Employee",
            "Nurse",
            "Doctor",
        };

        public static List<string> ResultValueTypes =
        [
            "Quantitative",
            "Qualitative",
        ];

        public static readonly List<string> MartialStatuss =
        [
            "Single",
            "Married",
            "Divorced",
            "Widowed",
            "Separated",
            "Unmarried"
        ];

        public static List<string> URLS =
        [
            // Clinic Services
            "clinic-service/general-consultation-services",
            "clinic-service/medical-checkups",
            "clinic-service/procedure-rooms",
            "clinic-service/accidents",
            "clinic-service/telemedicines",
            "clinic-service/vaccinations",
            "clinic-service/maternities",
            "clinic-service/wellness",
            "clinic-service/reporting-and-analytics",

            // Queues
            "queue/queue-counters",
            "queue/queue-displays",
            "queue/kiosk-departments",
            "queue/kiosk-configurations",
            "queue/kiosk",

            // BPJS
            "bpjs/bpjs-classifications",
            "bpjs/system-parameters",

            // BPJS Configurations
            "bpjs-integration/physicians",
            "bpjs-integration/services",
            "bpjs-integration/medical-procedures",
            "bpjs-integration/awareness",
            "bpjs-integration/diagnoses",
            "bpjs-integration/providers",
            "bpjs-integration/allergies",
            "bpjs-integration/drugs",

            // Patiens
            "patient/patients",
            "patient/family-relations",
            "patient/insurance-policies",
            "patient/disease-history",

            // Pharmacies
            "pharmacy/presciptions",
            "pharmacy/medicament-groups",
            "pharmacy/signas",
            "pharmacy/medicine-dosages",
            "pharmacy/active-components",
            "pharmacy/drug-forms",
            "pharmacy/drug-routes",
            "pharmacy/reporting-pharmacy",

            // Inventories
            "inventory/products",
            "inventory/product-categories",
            "inventory/inventory-adjusments",
            "inventory/internal-transfers",
            "inventory/goods-receipts",
            "inventory/uom-categories",
            "inventory/uoms",
            "inventory/locations",
            "inventory/reporting-inventories",
            "inventory/stock-moves",
            "inventory/Maintenance",
            "inventory/Maintenance-records",

            // Employees
            "employee/employees",
            "employee/sick-leave-managements",
            "employee/claim-managements",
            "employee/departments",
            "employee/job-positions",

            // Medicals
            "medical/practitioners",
            "medical/doctor-schedules",
            "medical/doctor-schedule-slots",
            "medical/insurances",
            "medical/specialities",
            "medical/diagnoses",
            "medical/procedures",
            "medical/disease-categories",
            "medical/chronic-diagnoses",
            "medical/health-centers",
            "medical/building-and-locations",
            "medical/services",
            "medical/nursing-diagnoses",
            "medical/lab-tests",
            "medical/sample-types",
            "medical/lab-uoms",
            "medical/projects",
            "medical/buildings",

            // Configurations
            "configuration/users",
            "configuration/groups",
            "configuration/menus",
            "configuration/email-templates",
            "configuration/email-settings",
            "configuration/companies",
            "configuration/countries",
            "configuration/provinces",
            "configuration/cities",
            "configuration/districts",
            "configuration/sub-districts",
            "configuration/religions",
            "configuration/occupationals",
        ];

        public class HomeStatusTemp
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        public static List<HomeStatusTemp> _homeStatusTemps = [
            new()
            {
                Code = "1",
                Name = "Meninggal",
            },
            new()
            {
                Code = "3",
                Name = "Berobat Jalan",
            },
            new()
            {
                Code = "4",
                Name = "Rujuk Vertikal",
            },
            new()
            {
                Code = "6",
                Name = "Rujuk Horizontal",
            },
        ];

        public static List<string> ClassTypes = [
            "VIP",
        ];

        public static List<string> ClassType = new List<string>
        {
            "FA",
            "MTC",
            "RWC",
            "LTA",
            "FATALITY",
            "OCCUPATIONAL ILLNESS"
        };

        public static List<string> SentType = new List<string>
        {
            "Back to Work",
            "Main Clinic",
            "Home",
            "Hospital",
            //"MCU"
        };

        public static List<string> AccidentLocations = new List<string>
        {
            "Inside",
            "Outside"
        };

        public static string DefaultFormatDate => "dd MMMM yyyy";

        public static string DefaultFormatDateTime => "dd MMMM yyyy HH:mm:ss";

        public class AllergyType
        {
            public string Code { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        public static string GetDefaultValue(this string value, string defaultValue = "-")
        {
            return value ?? defaultValue;
        }

        public static List<AllergyType> _allergyTypes = [
            new()
            {
                Code = "01",
                Name = "Makanan"
            },
            new()
            {
                Code = "02",
                Name = "Udara"
            },
            new()
            {
                Code = "03",
                Name = "Obat"
            },
        ];

        public static async Task GenerateColumnImportTemplateExcelFileAsync(IJSRuntime jSRuntime, IFileExportService file, string fileName, List<ExportFileData> data, string? name = "downloadFileFromStream")
        {
            var fileContent = await file.GenerateColumnImportTemplateExcelFileAsync(data);

            using var streamRef = new DotNetStreamReference(new MemoryStream(fileContent));
            await jSRuntime.InvokeVoidAsync("saveFileExcellExporrt", fileName, streamRef);
        }

        public static readonly string VERSION = "2.0.0";
        public static readonly string APP_NAME = "McHealthCare";

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

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16]; // IV harus sama dengan yang digunakan saat enkripsi

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new(cipherBytes);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt);
            return srDecrypt.ReadToEnd();
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

        public static DateTime CalculateNewExpiryDate(DateTime? expired, int? number, string unit)
        {
            if (expired == null || number == null)
            {
                // Tangani jika salah satu parameter null
                throw new ArgumentNullException("Expired date and number must not be null.");
            }

            switch (unit?.ToLower())
            {
                case "days":
                    return expired.Value.AddDays(number.Value);
                case "weeks":
                    return expired.Value.AddDays(number.Value * 7);
                case "months":
                    return expired.Value.AddMonths(number.Value);
                case "years":
                    return expired.Value.AddYears(number.Value);
                default:
                    throw new ArgumentException("Unit not recognized.", nameof(unit));
            }
        }

    }
}