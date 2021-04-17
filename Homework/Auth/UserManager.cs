using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Auth
{
    public class UserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLoggedIn => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string CurrentUserLogin => _httpContextAccessor.HttpContext.User.Login();

        public Guid CurrentUserId => _httpContextAccessor.HttpContext.User.Id();
    }
}
