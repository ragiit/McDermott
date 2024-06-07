using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace McDermott.Application.Extentions
{
    public static class EnumHelper
    {
        public static T EnumGetValue<T>(this Enum enumValue)
        {
            return (T)Convert.ChangeType(enumValue, typeof(T));
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.GetName() ?? enumValue.ToString();
        }

        public enum EnumStatusInventoryAdjusment
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "In-Progress")]
            InProgress = 2,

            [Display(Name = "Invalidate")]
            Invalidate = 0,
        }
    }
}
