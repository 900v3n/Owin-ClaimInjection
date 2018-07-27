namespace Owin.ClaimInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Owin.ClaimInjection.Model;

    internal sealed class ClaimInjectionMiddleware : OwinMiddleware
    {
        private const string GroupClaimIdentifier = "groups";

        private readonly IDictionary<GroupType, string> roleMapping;

        private readonly IList<User> users;

        private ClaimsIdentity defaultIdentity;

        public ClaimInjectionMiddleware(OwinMiddleware next, IDictionary<GroupType, string> roleMapping, IList<User> users)
            : base(next)
        {
            this.roleMapping = roleMapping;
            this.users = users;
        }

        public override Task Invoke(IOwinContext context)
        {
            if (defaultIdentity == null)
            {
                defaultIdentity = GetDefaultIdentity();
            }

            var userEmail = context.Request.Query.Get("user");

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                var user = users.FirstOrDefault(x => x.Email.Equals(userEmail, StringComparison.OrdinalIgnoreCase));

                if (user != null)
                {
                    defaultIdentity = ToClaimIdentity(user);
                }
            }

            context.Request.User = new ClaimsPrincipal(defaultIdentity);

            return Next.Invoke(context);
        }

        private ClaimsIdentity GetDefaultIdentity()
        {
            var identity = new ClaimsIdentity("DefaultIdentity");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString(), null));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Portal User", null));
            identity.AddClaim(new Claim(ClaimTypes.Email, "portaluser@experian.com", null));
            identity.AddClaim(new Claim(GroupClaimIdentifier, roleMapping[GroupType.InternalUser], null));

            return identity;
        }

        private ClaimsIdentity ToClaimIdentity(User user)
        {
            var identity = new ClaimsIdentity("LocalAuthentication");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString(), null));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name, null));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email, null));
            identity.AddClaim(new Claim(GroupClaimIdentifier, roleMapping[user.Group], null));

            return identity;
        }
    }
}