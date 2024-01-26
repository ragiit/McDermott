using Blazored.LocalStorage;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
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

        public static async Task<User> GetUserInfo(this ILocalStorageService o)
        {
            return await o.GetItemAsync<User>("Info");
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