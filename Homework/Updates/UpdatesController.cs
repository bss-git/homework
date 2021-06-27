using Homework.Auth;
using Homework.Updates.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Homework.Updates
{
    [Authorize]
    [Route("updates")]
    public class UpdatesController : Controller
    {
        private IUpdatesRepository _repo;

        public UpdatesController(IUpdatesRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("updatesList")]
        public async Task<IActionResult> UpdatesList()
        {
            return View(await _repo.GetListAsync(User.Id()));
        }
    }
}
