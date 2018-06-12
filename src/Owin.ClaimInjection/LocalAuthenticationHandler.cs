namespace Owin.ClaimInjection
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin.Infrastructure;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;
    using Owin.ClaimInjection.Model;

    public sealed class LocalAuthenticationHandler : AuthenticationHandler<LocalAuthenticationOptions>
    {
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var userEmail = Request.Query.Get("user");

            var user = Options.Users.FirstOrDefault(x => x.Email == userEmail);

            ClaimsIdentity identity;

            if (user != null)
            {
                identity = ToClaimIdentity(user);
            }
            else
            {
                identity = GetDefaultIdentity();
            }

            var properties = Options.StateDataFormat.Unprotect(Request.Query["state"]);

            return Task.FromResult(new AuthenticationTicket(identity, null));
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode == 401)
            {
                var challenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);

                //Only react to 401 if there is an authentication challenge for the authentication
                //type of this handler.
                if (challenge != null)
                {
                    var state = challenge.Properties;

                    if (string.IsNullOrEmpty(state.RedirectUri))
                    {
                        state.RedirectUri = Request.Uri.ToString();
                    }

                    var stateString = Options.StateDataFormat.Protect(state);

                    Response.Redirect(WebUtilities.AddQueryString(Options.SignInPath.Value, "state", stateString));
                }
            }

            return Task.FromResult<object>(null);
        }

        public override async Task<bool> InvokeAsync()
        {
            // This is always invoked on each request. For passive middleware, only do anything if this is
            // for our callback path when the user is redirected back from the authentication provider.
            if (Options.SignInPath.HasValue && Options.SignInPath == Request.Path)
            {
                var ticket = await AuthenticateAsync();

                if (ticket != null)
                {
                    Context.Authentication.SignIn(ticket.Properties, ticket.Identity);

                    Response.Redirect(ticket.Properties.RedirectUri);

                    // Prevent further processing by the owin pipeline.
                    return true;
                }
            }
            // Let the rest of the pipeline run.
            return false;
        }

        private static ClaimsIdentity ToClaimIdentity(User user)
        {
            var identity = new ClaimsIdentity("LocalAuthentication");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Name, null));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role, null));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email, null));

            return identity;
        }

        private static ClaimsIdentity GetDefaultIdentity()
        {
            var identity = new ClaimsIdentity("LocalAuthentication");
            identity.AddClaim(new Claim(ClaimTypes.Name, "DefaultUser", null));
            identity.AddClaim(new Claim(ClaimTypes.Role, "InternalUser", null));
            identity.AddClaim(new Claim(ClaimTypes.Email, "test@example.com", null));

            return identity;
        }
    }
}