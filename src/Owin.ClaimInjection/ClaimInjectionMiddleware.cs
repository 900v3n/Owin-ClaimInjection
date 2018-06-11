namespace Owin.ClaimInjection
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Owin.ClaimInjection.Model;

    public sealed class ClaimInjectionMiddleware : OwinMiddleware
    {
        private readonly IList<User> users;

        public ClaimInjectionMiddleware(OwinMiddleware next, IList<User> users)
            : base (next)
        {
            this.users = users;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var query = context.Request.QueryString.Value;

            // TODO: Extract out user Id from query string
            if (query.Contains("LocalUser"))
            {
                if (!context.Request.User.Identity.IsAuthenticated)
                {
                    // TODO: Find the user from list and inject into context.

                    context.Request.User = new GenericPrincipal(new GenericIdentity("Admin User"), null);

                    var identity = context.Request.User.Identity as ClaimsIdentity;
                    identity.AddClaim(new Claim("role", "Administrator"));
                    identity.AddClaim(new Claim("email", "admin@example.com"));
                }
            }

            await Next.Invoke(context);
        }
    }
}