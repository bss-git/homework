using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth
{
    public class JwtProvider
    {
        public static string GetToken(string login, Guid userId)
        {
            var claimsIdentity = new ClaimsIdentity(ClaimsProvider.GetClaims(login, userId), "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

            var jwt = new JwtSecurityToken(
                    claims: claimsIdentity.Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(JwtAuthOptions.Ttl),
                    signingCredentials: new SigningCredentials(JwtAuthOptions.Key, SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
