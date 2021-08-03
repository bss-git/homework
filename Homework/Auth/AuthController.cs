using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth;
using Homework.Auth.Dto;
using Homework.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Homework.Auth
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IPasswordManager _passwordManager;
        private readonly IUserRepository _userRepository;
        private readonly UserCreator _userCreator;

        public AuthController(IPasswordManager passwordManager, IUserRepository userRepository, UserCreator userCreator)
        {
            _passwordManager = passwordManager;
            _userRepository = userRepository;
            _userCreator = userCreator;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginDto)
        {
            if (!ModelState.IsValid)
                return View(loginDto);

            var user = await _userRepository.GetAsync(loginDto.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");

                return View(loginDto);
            }

            if (!await _passwordManager.IsValidPasswordAsync(loginDto.Login, loginDto.Password))
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                
                return View(loginDto);
            }

            await Authenticate(user.Id, user.Login);

            return Redirect($"~/users/{user.Login}");
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerDto)
        {
            if (!ModelState.IsValid)
                return View(registerDto);

            try
            {
                var user = await _userCreator.CreateAsync(registerDto.Login);
                await _userRepository.SaveAsync(user);
                await _passwordManager.SavePasswordAsync(user.Login, registerDto.Password);
                await Authenticate(user.Id, user.Login);

                return RedirectToAction("EditUser", "Users");
            }
            catch (ExistingLoginException)
            {
                ModelState.AddModelError("", "Пользователь с таким именем для входа уже существует");
                
                return View(registerDto);
            }
        }

        private async Task Authenticate(Guid userId, string login)
        {
            var token = JwtProvider.GetToken(login, userId);
            HttpContext.Response.Cookies.Append("beareridentity", token, new CookieOptions { HttpOnly = true });
        }

        [HttpGet("jwt")]
        [Authorize]
        public IActionResult GetJwt()
        {
            return Ok(JwtProvider.GetToken(User.Login(), User.Id()));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("beareridentity");
            return RedirectToAction("login", "auth");
        }
    }
}