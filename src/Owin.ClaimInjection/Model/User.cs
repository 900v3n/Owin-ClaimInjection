namespace Owin.ClaimInjection.Model
{
    public sealed class User
    {
        public string NameIdentifier { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}