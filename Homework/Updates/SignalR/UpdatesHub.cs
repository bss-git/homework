using Homework.Auth;
using Homework.Updates.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates.SignalR
{
    [Authorize]
    public class UpdatesHub : Hub, IUpdatesSubscriber
    {
        private Channel<UpdateMessage> _messages = Channel.CreateUnbounded<UpdateMessage>();

        public UpdatesHub(UpdatesMessageBus eventBus)
        {
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
                await Clients.User(updateMessage.Recepient.ToString()).SendAsync("NewUpdate", updateMessage.Update);
            }
        }

        public async Task NewUpdate(string message)
        {
            await this.Clients.All.SendAsync("NewUpdate", message);
        }
    }
}
