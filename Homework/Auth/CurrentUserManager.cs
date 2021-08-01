using Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Auth
{
    public class CurrentUserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string CurrentUserLogin => _httpContextAccessor.HttpContext.User.Login();

        public Guid CurrentUserId => _httpContextAccessor.HttpContext.User.Id();
    }
}
