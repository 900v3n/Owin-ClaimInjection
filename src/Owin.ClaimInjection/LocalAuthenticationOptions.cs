namespace Owin.ClaimInjection
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Owin.ClaimInjection.Model;

    public class LocalAuthenticationOptions : AuthenticationOptions
    {
        public LocalAuthenticationOptions(IList<User> users) 
            : base("Local")
        {
            this.Users = users ?? throw new ArgumentNullException(nameof(users));

            this.SignInPath = new PathString("/LocalSignIn");
        }

        public IList<User> Users { get; }

        public PathString SignInPath { get; set; }

        internal ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }
    }
}