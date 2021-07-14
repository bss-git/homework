using Homework.Auth;
using Homework.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Dialogs.Application
{
    [Route("dialogs")]
    [Authorize]
    public class DialogsController : Controller
    {
        private readonly IUserRepository _userRepository;

        public DialogsController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var friends = await _userRepository.GetFriendsAsync(User.Id());
            return View(friends);
        }
    }
}
