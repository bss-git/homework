using Homework.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Friends
{
    [Route("friends")]
    [Authorize]
    public class FriendsController : Controller
    {
        private IUserRepository _userRepository;

        public FriendsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> FriendsList()
        {
            return View();
        }
    }
}
