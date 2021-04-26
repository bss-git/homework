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

        private class NameJson
        {
            public string text { get; set; }

            public string gender { get; set; }

            public int num { get; set; }
        }


        private class SurnameJson
        {
            public string text { get; set; }

            public string gender  { get; set; }

            public string m_form { get; set; }

            public string f_form { get; set; }
        }

        private static string Tr2(string s)
        {
            var ret = new StringBuilder();
            string[] rus = { "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й",
          "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц",
          "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
            string[] eng = { "A", "B", "V", "G", "D", "E", "E", "ZH", "Z", "I", "Y",
          "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "F", "KH", "TS",
          "CH", "SH", "SHCH", null, "Y", null, "E", "YU", "YA" };

            for (int j = 0; j < s.Length; j++)
                for (int i = 0; i < rus.Length; i++)
                    if (string.Equals(s.Substring(j, 1), rus[i], StringComparison.InvariantCultureIgnoreCase))
                        ret.Append(eng[i]?.ToLower());

            return ret.ToString();
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