using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Auth
{
    public class ClaimsProvider
    {
        public static List<Claim> GetClaims(string login, Guid userId)
        {
            return new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim("Id", userId.ToString())
            };
        }
    }
}
