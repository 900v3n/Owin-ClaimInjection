namespace Owin.ClaimInjection
{
    using Microsoft.Owin;
    using Microsoft.Owin.Security.DataHandler;
    using Microsoft.Owin.Security.DataProtection;
    using Microsoft.Owin.Security.Infrastructure;

    public sealed class ClaimInjectionMiddleware : AuthenticationMiddleware<LocalAuthenticationOptions>
    {
        public ClaimInjectionMiddleware(OwinMiddleware next, IAppBuilder appBuilder, LocalAuthenticationOptions options) 
            : base(next, options)
        {
            if (options.StateDataFormat == null)
            {
                var dataProtector = appBuilder.CreateDataProtector(
                    typeof(ClaimInjectionMiddleware).FullName,
                    options.AuthenticationType);

                options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }
        }

        protected override AuthenticationHandler<LocalAuthenticationOptions> CreateHandler()
        {
            return new LocalAuthenticationHandler();
        }
    }
}