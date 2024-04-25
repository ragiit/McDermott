namespace McDermott.Application.Extentions
{
    public static class EnumHelper
    {
        public static T EnumGetValue<T>(this Enum enumValue)
        {
            return (T)Convert.ChangeType(enumValue, typeof(T));
        }

        public enum EnumStatusPharmacy
        {
            RecipeReceived = 1,
            RecipeInProcess = 2,
            RecipeCompleted = 3
        }

        public enum EnumStatusGeneralConsultantService
        {
            // nanti
            test = 1,
        }
    }
}
