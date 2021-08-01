using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Homework.Models;
using Homework.Persistence;
using Microsoft.AspNetCore.Authorization;
using Homework.Auth;
using Auth;

namespace Homework.Controllers
{   
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet("~/")]
        public IActionResult Index()
        {
            return Redirect($"~/users/{User.Login()}");
        }

        [Authorize]
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
