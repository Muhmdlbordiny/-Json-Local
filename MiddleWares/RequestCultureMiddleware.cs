using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace json_based_localization.MiddleWares
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var currentlanguage = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var browserslanguage = context.Request.Headers[key: "Accept-language"].ToString()[..2];
            if (string.IsNullOrEmpty(currentlanguage)) // open App Dont Change language using hand
            {
                var culture = string.Empty;
                switch (browserslanguage)
                {
                    case "ar":
                        culture = "ar-EG";
                        break;

                    case "de":
                        culture = "de-DE";
                        break;

                    default:
                        culture = "en-US";
                        break;

                }
                var requestculture = new RequestCulture(culture, culture);
                context.Features.Set<IRequestCultureFeature>(new RequestCultureFeature(requestculture, null));
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                CultureInfo.CurrentUICulture = new CultureInfo(culture);
            }
            await _next(context);
        }
    }
}
