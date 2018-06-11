namespace Owin.ClaimInjection
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Newtonsoft.Json;
    using Owin.ClaimInjection.Model;

    public static class ClaimInjectorExtensions
    {
        public static IAppBuilder UseLocalAuthentication(this IAppBuilder appBuilder, string configFilePath)
        {
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException(configFilePath);
            }

            var fileContent = File.ReadAllText(configFilePath);

            appBuilder.Use(typeof(ClaimInjectionMiddleware), JsonConvert.DeserializeObject<List<User>>(fileContent));

            return appBuilder;
        }

        public static IAppBuilder UseLocalAuthentication(this IAppBuilder appBuilder, IList<User> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            
            appBuilder.Use(typeof(ClaimInjectionMiddleware), users);

            return appBuilder;
        }
    }
}