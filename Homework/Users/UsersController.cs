using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework.Auth;
using Homework.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Users
{
    [Route("users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{login}")]
        public async Task<IActionResult> UserPage(string login)
        {
            var user = await _userRepository.GetAsync(login);
            if (user == null)
                return NotFound("Пользователь не найден");
            
            var dto = new UserViewModel
            {
                User = user,
                IsMe = User.Id() == user.Id
            };

            return View(dto);
        }
    }
}