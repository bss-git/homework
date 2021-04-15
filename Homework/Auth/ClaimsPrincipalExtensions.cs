using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Homework.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Login(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identity.Name;
        }

        public static Guid Id(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.Claims.First(x => x.Type == "Id").Value);
        }
    }
}
