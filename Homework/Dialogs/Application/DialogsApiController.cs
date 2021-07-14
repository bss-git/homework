using Homework.Auth;
using Homework.Dialogs.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Dialogs.Application
{
    [Route("api/dialogs")]
    [ApiController]
    [Authorize]
    public class DialogsApiController : ControllerBase
    {
        private IDialogsRepository _dialogsRepository;

        [HttpPost("message")]
        public async Task<IActionResult> PostMessage(PostMessageCommand command)
        {
            await _dialogsRepository.Save(User.Id(), command.To, command.Text);
            return Ok();
        }

        [HttpGet("{interlocutorId}")]
        public async Task<IActionResult> GetMessages(Guid interlocutorId)
        {
            var messages = await _dialogsRepository.GetList(User.Id(), interlocutorId);
            return new JsonResult(messages);
        }
    }
}
