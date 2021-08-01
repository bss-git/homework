using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Auth
{
    public static class JwtAuthOptions
    {
        private const string _secret = "secret";

        internal static TimeSpan Ttl = TimeSpan.FromHours(10);

        private static Lazy<SymmetricSecurityKey> _key;

        public static SymmetricSecurityKey Key => _key.Value;

        static JwtAuthOptions()
        {
            _key = new Lazy<SymmetricSecurityKey>(() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)));
        }
    }
}
