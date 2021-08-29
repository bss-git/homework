﻿using Auth;
using Dialogs.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dialogs.Application
{
    [Route("api/dialogs")]
    [ApiController]
    [Authorize]
    public class DialogsApiController : ControllerBase
    {
        private readonly IDialogsRepository _dialogsRepository;

        public DialogsApiController(IDialogsRepository dialogsRepository)
        {
            _dialogsRepository = dialogsRepository;
        }

        [HttpPost("message")]
        public async Task<IActionResult> PostMessage(PostMessageCommand command)
        {
            var message = new Message(User.Id(), command.To, command.Text, DateTime.UtcNow);
            await _dialogsRepository.SaveAsync(message);

            return Ok();
        }

        [HttpGet("{interlocutorId}")]
        public async Task<IActionResult> GetMessages(Guid interlocutorId)
        {
            var userId = User.Id();
            var messages = (await _dialogsRepository.GetListAsync(userId, interlocutorId))
                .Select(x => new MessageClientDto(x, x.From == userId));

            return new JsonResult(messages);
        }
    }
}
