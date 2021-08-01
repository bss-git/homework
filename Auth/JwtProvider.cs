using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Auth
{
    public class JwtProvider
    {
        public static string GetToken(string login, Guid userId)
        {
            var jwt = new JwtSecurityToken(
                    claims: ClaimsProvider.GetClaims(login, userId),
                    expires: DateTime.UtcNow.Add(JwtAuthOptions.Ttl),
                    signingCredentials: new SigningCredentials(JwtAuthOptions.Key, SecurityAlgorithms.HmacSha256));
            
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
