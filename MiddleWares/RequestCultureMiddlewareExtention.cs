namespace json_based_localization.MiddleWares
{
    public static class RequestCultureMiddlewareExtention
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }
}
