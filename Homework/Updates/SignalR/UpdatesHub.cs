using Auth;
using Homework.Auth;
using Homework.Updates.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Homework.Updates.SignalR
{
    [Authorize]
    public class UpdatesHub : Hub
    {
        private readonly UpdatesMessageBus _messageBus;

        public UpdatesHub(UpdatesHubEventPublisher publisher, UpdatesMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        public override Task OnConnectedAsync()
        {
            _messageBus.SubscribeRecipient(Context.User.Id().ToString());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _messageBus.UnSubscribeRecipient(Context.User.Id().ToString());
            return base.OnDisconnectedAsync(exception);
        }
    }
}
