﻿namespace Owin.ClaimInjection.Model
{
    public sealed class User
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public GroupType Group { get; set; }
    }
}