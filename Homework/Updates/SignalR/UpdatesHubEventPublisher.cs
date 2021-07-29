using Homework.Updates.Dto;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates.SignalR
{
    public class UpdatesHubEventPublisher: IUpdatesSubscriber
    {
        private Channel<UpdateMessage> _messages = Channel.CreateUnbounded<UpdateMessage>();
        private IHubContext<UpdatesHub> _hubContext;
        private readonly ILogger<UpdatesHubEventPublisher> _logger;

        public UpdatesHubEventPublisher(IHubContext<UpdatesHub> hubContext, UpdatesMessageBus eventBus, ILogger<UpdatesHubEventPublisher> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
            eventBus.Subscribe(this);
            Task.Run(HandleUpdatesTask);
        }

        public void HandleEvent(UpdateMessage updateEvent)
        {
            _messages.Writer.WriteAsync(updateEvent);
        }

        private async Task HandleUpdatesTask()
        {
            await foreach (var updateMessage in _messages.Reader.ReadAllAsync())
            {
                try
                {
                    var update = updateMessage.Update;
                    var dto = new { UserName = update.UserName, Timestamp = update.Timestamp.ToString(), Message = update.Message };
                    await _hubContext.Clients.User(updateMessage.Recepient.ToString()).SendAsync("NewUpdate", dto);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                }
            }
        }

    }
}
