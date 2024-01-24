using Blazored.LocalStorage;
using McDermott.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using static McDermott.Web.Components.Pages.Config.CountryPage;

namespace McDermott.Web
{
    public static class Helper
    {
        public static User? UserLogin { get; set; }

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