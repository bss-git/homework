using Homework.Auth;
using Homework.Updates.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Homework.Updates
{
    [Authorize]
    [Route("api/updates")]
    public class UpdatesApiController : Controller
    {
        private IUpdatesRepository _repo;

        public UpdatesApiController(IUpdatesRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> PostUpdate([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return BadRequest("Заполните текст сообщения");

            var update = new UpdateViewModel
            {
                UserId = User.Id(),
                UserName = User.Login(),
                Timestamp = DateTime.UtcNow,
                Message = message
            };

            await _repo.SaveAsync(update);

            return Ok();
        }
    }
}
