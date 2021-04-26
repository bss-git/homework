using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework.Auth;
using Homework.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpGet]
        public async Task<IActionResult> UsersList(int offset, int limit)
        {
            var adjustedOffset = Math.Max(0, offset);
            var adjustedLimit = Math.Max(Math.Min(limit, 50), 10);
            var users = await _userRepository.GetListAsync(adjustedOffset, adjustedLimit);

            return View(new UserListViewModel { Users = users, CurrentOffset = adjustedOffset, PageSize = adjustedLimit });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUser([FromQuery] string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                ViewData["hasInput"] = false;
                return View(Enumerable.Empty<UserShortDto>());
            }
            else
            {
                ViewData["hasInput"] = true;
            }

            var args = input.Split(" ");
            if (args.Length != 2)
            {
                ModelState.AddModelError("", "Введите часть имени и фамилии через пробел");

                return View(Enumerable.Empty<UserShortDto>());
            }

            var users = await _userRepository.SearchAsync(args[0], args[1]);

            return View(users);
        }

        [HttpGet("{login}")]
        public async Task<IActionResult> UserPage(string login)
        {
            var user = await _userRepository.GetAsync(login);
            if (user == null)
                return NotFound("Пользователь не найден");

            var dto = new UserViewModel(user);

            return View(dto);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> EditUser()
        {
            var login = User.Login();

            var user = await _userRepository.GetAsync(login);
            if (user == null)
                return NotFound("Пользователь не найден");

            var dto = new UserViewModel(user);

            return View(dto);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditUser(UserViewModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var login = User.Login();

            var user = await _userRepository.GetAsync(login);
            if (user == null)
                return NotFound("Пользователь не найден");

            user.Name = dto.Name ?? user.Name;
            user.Surname = dto.Surname ?? user.Surname;
            user.Gender = dto.Gender;
            user.BirthDate = dto.BirthDate;
            user.City = dto.City ?? user.City;
            user.Interest = dto.Interest ?? user.Interest;

            await _userRepository.SaveAsync(user);

            return Redirect($"~/users/{user.Login}");
        }
    }
}