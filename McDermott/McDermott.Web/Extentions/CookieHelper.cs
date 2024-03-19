namespace McDermott.Web.Extentions
{
    /*
     *  Cara pakai Cookie:
     *  1. Perlu panggil method SetCookie() nya dulu di method OnInitializedAsync() sebelum di pakai
     *  2. 
     */

    public static class CookieHelper
    {
        public static readonly string USER_INFO = "USER_INFO";
        public static readonly string USER_GROUP = "USER_GROUP";

        public static void SetCookie(IHttpContextAccessor httpContextAccessor, string key, string value)
        {
            if (httpContextAccessor.HttpContext is null)
                return;

            httpContextAccessor.HttpContext.Response.Cookies.Append(key, value);
        }

        public static async Task<string> GetCookie(this IJSRuntime JsRuntime, string key)
        {
            string value = await JsRuntime.InvokeAsync<string>("getCookie", key);

            if (value == null)
                return null;

            value = Helper.Decrypt(value);

            return value;

            //if (httpContextAccessor.HttpContext is null)
            //    return string.Empty;

            //return httpContextAccessor.HttpContext.Request.Cookies[key] ?? string.Empty;
        }

        public static async Task<User> GetCookieUserLogin(this IJSRuntime JsRuntime)
        {
            try
            {
                dynamic value = await JsRuntime.InvokeAsync<string>("getCookie", CookieHelper.USER_INFO);

                if (value == null)
                    return null;

                value = Helper.Decrypt(value);

                return JsonConvert.DeserializeObject<User>(value);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void RemoveCookie(this IHttpContextAccessor httpContextAccessor, string key)
        {
            if (httpContextAccessor.HttpContext is null)
                return;

            httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public static void UpdateCookie(IHttpContextAccessor httpContextAccessor, string key, string value)
        {
            if (httpContextAccessor.HttpContext is null)
                return;

            if (httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(key))
            {
                httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
                httpContextAccessor.HttpContext.Response.Cookies.Append(key, value);
            }
            else
            {
                SetCookie(httpContextAccessor, key, value);
            }
        }
    }


}
