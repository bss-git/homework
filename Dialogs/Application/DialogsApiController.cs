using Auth;
using Dialogs.Application.Dto;
using Dialogs.Persistence.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedDto.UserCounters;
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
        private readonly KafkaProducer<Guid> _kafkaProducer;

        public DialogsApiController(IDialogsRepository dialogsRepository, KafkaProducer<Guid> kafkaProducer)
        {
            _dialogsRepository = dialogsRepository;
            _kafkaProducer = kafkaProducer;
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

            _ = _kafkaProducer.ProduceAsync("user_dialog", Guid.NewGuid(), new UserCounterEvent { UserId = userId, EventType = EventType.UserRead });

            return new JsonResult(messages);
        }
    }
}
