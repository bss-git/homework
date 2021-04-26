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

        [AllowAnonymous]
        [HttpGet("fillNames")]
        public async Task<IActionResult> UserGen()
        {
            var names = new List<NameJson>();
            var namesFile = System.IO.File.ReadLines("./names_table.jsonl");
            var maxScore = 0;
            foreach (var line in namesFile)
            {
                var o = JsonConvert.DeserializeObject<NameJson>(line);
                if (o.num < 1000 || string.IsNullOrWhiteSpace(o.text))
                    continue;

                names.Add(o);
                if (o.num > maxScore)
                    maxScore = o.num;
            }

            var surnames = new List<SurnameJson>();
            var surnamesFile = System.IO.File.ReadLines("./surnames_table.jsonl");
            foreach (var line in surnamesFile)
            {
                var o = JsonConvert.DeserializeObject<SurnameJson>(line);
                if (string.IsNullOrWhiteSpace(o.text))
                    continue;

                surnames.Add(o);
            }

            var users = new ConcurrentBag<User>();
            Parallel.For(0, 528_000, async (i) =>
            {
                var localList = new List<User>();

                var rng = new Random();

                NameJson name;
                while (true)
                {
                    name = names[rng.Next(names.Count)];
                    if (name.num >= rng.Next(maxScore))
                        break;
                }

                var surname = "";
                while (true)
                {
                    var surnameO = surnames[rng.Next(surnames.Count)];
                    if (surnameO.gender == "u" || surnameO.gender == name.gender)
                    {
                        surname = surnameO.text;
                        break;
                    }

                    if (name.gender == "m" && !string.IsNullOrWhiteSpace(surnameO.m_form))
                    {
                        surname = surnameO.m_form;
                        break;
                    }

                    if (name.gender == "f" && !string.IsNullOrWhiteSpace(surnameO.f_form))
                    {
                        surname = surnameO.f_form;
                        break;
                    }
                }

                var login = $"{Tr2(name.text)}_{Tr2(surname)}_{rng.Next(10000)}";

                var user = new User(Guid.NewGuid(), login)
                {
                    Gender = name.gender == "f" ? Gender.Female : Gender.Male,
                    Name = name.text,
                    Surname = surname
                };

                //users.Add(user);
                await _userRepository.SaveAsync(user);
                await Task.Delay(10);
            });

            return Ok();
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