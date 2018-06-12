namespace Owin.ClaimInjection
{
    public static class ClaimInjectorExtensions
    {
        public static IAppBuilder UseLocalAuthentication(this IAppBuilder appBuilder, LocalAuthenticationOptions options)
        {
            return appBuilder.Use(typeof(ClaimInjectionMiddleware), appBuilder, options);
        }
    }
}