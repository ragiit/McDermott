using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;

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

        public enum EnumStatusAccident
        {
            [Display(Name = "Return to work")]
            ReturnToWork = 1,

            [Display(Name = "Restricted work case")]
            RestrictedWorkCase = 2,

            [Display(Name = "Lost Work days case")]
            LostWorkDaysCase = 3,

            [Display(Name = "Fatality case")]
            FatalityCase = 4
        }

        public enum EnumStatusMCU
        {
            [Display(Name = "Draft")]
            Draft = 1,

            [Display(Name = "Employee Test")]
            EmployeeTest = 2,

            [Display(Name = "HR Candidat")]
            HRCandidat = 3,

            [Display(Name = "Examination")]
            Examination = 4,

            [Display(Name = "Result")]
            Result = 5,

            [Display(Name = "Done")]
            Done = 6
        }

        public enum EnumStatusGeneralConsultantService
        {
            // nanti
            test = 1,
        }

        public enum EnumStatusSickLeave
        {
            [Display(Name = "not-action")]
            NotAction = 0,

            [Display(Name = "not-send")]
            NotSend = 1,

            [Display(Name = "send")]
            Send = 2,

            [Display(Name = "not-printed")]
            NotPrinted = 3,

            [Display(Name = "printed")]
            Printed = 4,
        }
    }
}