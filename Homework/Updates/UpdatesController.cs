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

        [HttpPost("postUpdate")]
        public async Task<IActionResult> PostUpdate(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return Redirect($"~/updates/updatesList");

            var update = new UpdateViewModel
            {
                UserId = User.Id(),
                UserName = User.Login(),
                Timestamp = DateTime.UtcNow,
                Message = message
            };

            await _repo.SaveAsync(update);
            
            return Redirect($"~/updates/updatesList");
        }
    }
}
