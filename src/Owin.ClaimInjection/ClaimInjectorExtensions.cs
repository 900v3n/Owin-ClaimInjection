namespace Owin.ClaimInjection
{
    using System.Collections.Generic;
    using Owin.ClaimInjection.Model;

    public static class ClaimInjectorExtensions
    {
        public static IAppBuilder UseLocalAuthentication(this IAppBuilder appBuilder, IDictionary<GroupType, string> roleMapping, IList<User> users)
        {
            return appBuilder.Use(typeof(ClaimInjectionMiddleware), roleMapping, users);
        }
    }
}